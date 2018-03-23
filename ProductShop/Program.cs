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
        static private EventWaitHandle _isWorking;

        static void Main(string[] args)
        {
            //init
            _shop = new Shop();
            _isWorking = new EventWaitHandle(false, EventResetMode.ManualReset);

            //start
            ConsoleHelper.WhiteTips("Press ENTER to start");
            Console.ReadLine();

            DoWork(10, 500);

            //stop
            ConsoleHelper.WhiteTips("Pres ENTER to stop");
            Console.ReadLine();
            _isWorking.Set();
            ConsoleHelper.WhiteTips("Please wait...");
        }

        static void DoWork(int buyersCount, int delay)
        {
            Thread generateThread = new Thread(() =>
            {
                Console.WriteLine("Shop opening...");
                _shop.Open();
                Console.WriteLine("Shop is opened");

                do
                {
                    Console.WriteLine("Buyers creating...");
                    Parallel.For(0, buyersCount, (i) =>
                    {
                        Buyer buyer = new Buyer(_shop.Stands);
                        buyer.GoHome += Buyer_GoHome;
                        _shop.ActiveVisitorsCount++;
                        _shop.Visitors++;
                    });
                    Console.WriteLine("Iteration is completed");
                }
                while (!_isWorking.WaitOne(delay));

                while (_shop.ActiveVisitorsCount > 0) { }
                _shop.Close();

                _shop.WorkCompleted.WaitOne(-1);
                ConsoleHelper.WhiteSuccess($"Visitors: {_shop.Visitors}");
                ConsoleHelper.WhiteSuccess($"Total profit: {_shop.TotalProfit}");
                Console.WriteLine("Shop is closed");
            });

            generateThread.Start();
        }

        private static void Buyer_GoHome(object sender, EventArgs e)
        {
            _shop.ActiveVisitorsCount--;
        }
    }
}
