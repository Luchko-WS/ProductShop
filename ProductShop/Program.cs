using System;
using Utils;
using System.Threading;
using ProductShop.Actors;
using System.Threading.Tasks;

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
            Console.WriteLine("Press ENTER to start");
            Console.ReadLine();

            DoWork(200, 1000);

            //stop
            Console.WriteLine("Pres ENTER to stop");
            Console.ReadLine();
            _isWorking.Set();
            Console.WriteLine("Please wait...");

            _shop.WorkCompleted.WaitOne(-1);
            Console.WriteLine($"Visitors: {_shop.Visitors}");
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
                    Parallel.For(1, buyersCount, (i) =>
                    {
                        Buyer buyer = new Buyer(_shop.Stands);
                    });
                    _shop.Visitors += buyersCount;
                    Console.WriteLine("Iteration is completed");
                }
                while (!_isWorking.WaitOne(delay));
                _shop.Close();
            });

            generateThread.Start();
        }
    }
}
