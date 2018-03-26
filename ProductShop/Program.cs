using System;
using System.Threading;
using ProductShop.Actors;
using System.Threading.Tasks;
using Utils;

namespace ProductShop
{
    public class Program
    {
        static private Shop _shop;
        static object _locker = new object();
        static private EventWaitHandle _isWorkingEventWaitHandle;

        static void Main(string[] args)
        {
            int X;
            double Y;

            //init
            _shop = new Shop();
            _isWorkingEventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

            while (true)
            {
                try
                {
                    ConsoleHelper.WhiteTips("Enter numbers of buyers (X):");
                    X = Convert.ToInt32(Console.ReadLine());
                    ConsoleHelper.WhiteTips("Enter delay (Y) (in seconds):");
                    Y = Convert.ToDouble(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    ConsoleHelper.WhiteDanger("Wrong format of input value.");
                }
            }

            //start
            ConsoleHelper.WhiteTips("Press ENTER to start");
            Console.ReadLine();

            DoWork(X, (int)(Y * 1000));

            //stop
            ConsoleHelper.WhiteTips("Pres ENTER to stop");
            Console.ReadLine();
            _isWorkingEventWaitHandle.Set();

            ConsoleHelper.WhiteTips("Please wait...");
            _shop.WorkCompleted.WaitOne(-1);

            ConsoleHelper.WhiteSuccess($"Visitors: {_shop.Visitors}");
            ConsoleHelper.WhiteSuccess($"Total profit: {_shop.TotalProfit}");
#if DEBUG
                Console.WriteLine("Shop is closed");
#endif
        }

        static void DoWork(int buyersCount, int delay)
        {
            Thread generateThread = new Thread(() =>
            {
                Console.WriteLine("Shop opening...");
                _shop.Open();
                Console.WriteLine("Shop is opened. Work is started.");

                do
                {
#if DEBUG
                    Console.WriteLine("Buyers creating...");
#endif
                    Parallel.For(0, buyersCount, (i) =>
                    {
                        Buyer buyer = new Buyer(_shop.Stands);
                        buyer.WorkCompleted += Buyer_WorkCompleted;
                        _shop.ActiveVisitorsCount++;
                        _shop.Visitors++;
                    });
#if DEBUG
                    Console.WriteLine("Iteration is completed");
#endif
                }
                while (!_isWorkingEventWaitHandle.WaitOne(delay));

                _shop.Close();
            });

            generateThread.Start();
        }

        private static void Buyer_WorkCompleted(object sender, EventArgs e)
        {
            if (sender is Buyer)
            {
                _shop.ActiveVisitorsCount--;
                ((Buyer)sender).WorkCompleted -= Buyer_WorkCompleted;
            }
        }
    }
}
