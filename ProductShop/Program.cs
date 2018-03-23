using System;
using Utils;
using System.Threading;

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

            DoWork(100, 1000);

            //stop
            Console.WriteLine("Pres ENTER to stop");
            Console.ReadLine();
            _isWorking.Set();

            _shop.WorkCompleted.WaitOne(-1);
            Console.WriteLine("Shop is closed");
        }

        static void DoWork(int buyersCount, int delay)
        {
            Thread generateThread = new Thread(() =>
            {
                _shop.Open();
                do
                {
                    for (int i = 0; i < buyersCount; i++)
                    {
                        //create buyer
                    }
                    ConsoleHelper.Debug("Iteration is completed");
                }
                while (!_isWorking.WaitOne(delay));
                _shop.Close();
            });
            generateThread.Start();
        }

        private static void Shop_WorkCompleted(object sender, EventArgs e)
        {
            ConsoleHelper.Debug("Shop is complete a work");
        }
    }
}
