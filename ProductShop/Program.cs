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
            int numbersOfBuyers;
            double generatingDelay;

            //init
            _shop = new Shop();
            _isFinishOfWorkEventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

            while (true)
            {
                try
                {
                    ConsoleHelper.WriteTips("Enter numbers of buyers (X):");
                    numbersOfBuyers = Convert.ToInt32(Console.ReadLine());
                    ConsoleHelper.WriteTips("Enter delay (Y) (in seconds):");
                    generatingDelay = Convert.ToDouble(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    ConsoleHelper.WriteDanger("Wrong format of input value.");
                }
            }

            //start
            ConsoleHelper.WriteTips("Press <ENTER> to open the shop.");
            Console.ReadLine();

            DoWork(numbersOfBuyers, (int)(generatingDelay * 1000));

            //stop
            ConsoleHelper.WriteTips("Pres <S> to show the statistic.");
            ConsoleHelper.WriteTips("Pres <ENTER> to close the shop.");

            while(true)
            {
                var consoleKey = Console.ReadKey(true).Key;
                if (consoleKey == ConsoleKey.S)
                {
                    ConsoleHelper.WriteTips("Statistic is loading. Please wait...");
                    _shop.ShowStatistic();
                }
                else if (consoleKey == ConsoleKey.Enter)
                {
                    break;
                }
            }
            ConsoleHelper.WriteTips("Shop is closing. Please wait...");

            _isFinishOfWorkEventWaitHandle.Set();
            _shop.WorkCompleted.WaitOne(-1);
            ConsoleHelper.WriteSuccess($"Visitors: {_shop.VisitorsCount}");
            ConsoleHelper.WriteSuccess($"Total profit: {_shop.TotalProfit}");
        }

        static void DoWork(int buyersCount, int timeout)
        {
            Thread generateBuyersThread = new Thread(() =>
            {
                _shop.Open();
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
