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

            _shop.WorkCompleted.WaitOne(-1);
            ConsoleHelper.WhiteSuccess($"Visitors: {_shop.Visitors}");
            Console.WriteLine("Shop is closed");
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
                        _shop.Visitors++;
                    });
                    Console.WriteLine("Iteration is completed");
                }
                while (!_isWorking.WaitOne(delay));
                _shop.Close();
            });

            generateThread.Start();
        }
    }
}
