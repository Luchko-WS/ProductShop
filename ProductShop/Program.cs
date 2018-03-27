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
        static private EventWaitHandle _isFinishOfWorkEventWaitHandle;

        static void Main(string[] args)
        {
            int X;
            double Y;

            //init
            _shop = new Shop();
            _isFinishOfWorkEventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

            while (true)
            {
                try
                {
                    ConsoleHelper.WriteTips("Enter numbers of buyers (X):");
                    X = Convert.ToInt32(Console.ReadLine());
                    ConsoleHelper.WriteTips("Enter delay (Y) (in seconds):");
                    Y = Convert.ToDouble(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    ConsoleHelper.WriteDanger("Wrong format of input value.");
                }
            }

            //start
            ConsoleHelper.WriteTips("Press ENTER to open the shop");
            Console.ReadLine();

            DoWork(X, (int)(Y * 1000));

            //stop
            ConsoleHelper.WriteTips("Pres ENTER to close the shop");
            Console.ReadLine();
            _isFinishOfWorkEventWaitHandle.Set();

            ConsoleHelper.WriteTips("Please wait...");
            _shop.WorkCompleted.WaitOne(-1);
#if DEBUG
            ConsoleHelper.WriteInfo("Shop is closed");
#endif
            ConsoleHelper.WriteSuccess($"Visitors: {_shop.VisitorsCount}");
            ConsoleHelper.WriteSuccess($"Total profit: {_shop.TotalProfit}");
        }

        static void DoWork(int buyersCount, int timeout)
        {
            Thread generateBuyersThread = new Thread(() =>
            {
                ConsoleHelper.WriteInfo("Shop is opening...");
                _shop.Open();
                ConsoleHelper.WriteInfo("Shop is opened. Work is started.");

                do
                {
#if DEBUG
                    Console.WriteLine("Buyers creating...");
#endif
                    Parallel.For(0, buyersCount, body: (i) =>
                    {
                        Buyer buyer = new Buyer(_shop);
                    });
#if DEBUG
                    Console.WriteLine("Iteration is completed");
#endif
                }
                while (!_isFinishOfWorkEventWaitHandle.WaitOne(timeout));

                _shop.Close();
            });

            generateBuyersThread.Start();
        }
    }
}
