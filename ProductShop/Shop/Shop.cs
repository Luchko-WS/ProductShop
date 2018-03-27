using Utils;
using System;
using System.Threading;
using Utils.Collections;

namespace ProductShop
{
    public class Shop
    {
        private CustomLinkedList<Stand> _stands;
        private object _standsListLocker;
        
        private int _visitors = 0;
        private int _activeVisitorsCount = 0;
        private object _activeVisitorsCountLocker;

        private EventWaitHandle _workCompleted;

        public Shop()
        {
            Product iceCream = new Product("Ice Cream", 12);
            Product cake = new Product("Cake", 35);
            Product chocolate = new Product("Chocolate", 24);

            Stand standWithIceCreams = new Stand(iceCream);
            standWithIceCreams.WorkCompleted += Stand_WorkCompleted;
            Stand standWithCakes = new Stand(cake);
            standWithCakes.WorkCompleted += Stand_WorkCompleted;
            Stand standWithChocolates = new Stand(chocolate);
            standWithChocolates.WorkCompleted += Stand_WorkCompleted;

            _stands = new CustomLinkedList<Stand>
            {
                standWithIceCreams,
                standWithCakes,
                standWithChocolates
            };

            _standsListLocker = new object();
            _activeVisitorsCountLocker = new object();
            _workCompleted = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public CustomLinkedList<Stand> Stands
        {
            get
            {
                return _stands;
            }
        }

        public int ActiveVisitorsCount
        {
            get
            {
                lock (_activeVisitorsCountLocker)
                {
                    return _activeVisitorsCount;
                }
            }
        }

        public int Visitors
        {
            get
            {
                return _visitors;
            }
        }

        public double TotalProfit { get; set; }

        public EventWaitHandle WorkCompleted
        {
            get
            {
                return _workCompleted;
            }
        }

        public void Open()
        {
            foreach (var stand in _stands)
            {
                stand.Open();
            }
        }

        public void Close()
        {
            while (ActiveVisitorsCount > 0)
            {
                Thread.Sleep(1000);
                ConsoleHelper.WriteInfo($"Active buyers: {ActiveVisitorsCount}");
            }

            foreach (var stand in _stands)
            {
                stand.Close();
            }
        }

        public void RegisterBuyer()
        {
            lock (_activeVisitorsCountLocker)
            {
                _activeVisitorsCount++;
                _visitors++;
            }
        }

        public void UnregisterBuyer()
        {
            lock (_activeVisitorsCountLocker)
            {
                _activeVisitorsCount--;
            }
        }

        //change this method
        private void Stand_WorkCompleted(object sender, EventArgs e)
        {
            Stand stand = sender as Stand;
            if (stand != null)
            {
                stand.WorkCompleted -= Stand_WorkCompleted;

                lock (_standsListLocker)
                {
                    int selledProductsCount = stand.GetSelledProductsCount();

                    ConsoleHelper.WriteInfo($"The statistic of stand with {stand.Product.Name}s.");
                    ConsoleHelper.WriteSuccess($"Selled products: {stand.Product.Name}, count: {selledProductsCount}, profit: {selledProductsCount * stand.Product.Price}");
#if DEBUG
                    ConsoleHelper.WriteInfo($"The stand with {stand.Product.Name}s is closed.");
#endif
                    TotalProfit += selledProductsCount * stand.Product.Price;

                    _stands.Remove(stand);
                    if (_stands.Count == 0)
                    {
                        _workCompleted.Set();
                    }
                }
            }
        }
    }
}
