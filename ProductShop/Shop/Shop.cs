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
        
        private int _visitorsCount = 0;
        private int _activeVisitorsCount = 0;
        private object _activeVisitorsCountLocker;

        private EventWaitHandle _workCompleted;

        public Shop()
        {
            Product iceCream = new Product("Ice Cream", 12);
            Product cake = new Product("Cake", 35);
            Product chocolate = new Product("Chocolate", 24);

            Stand standWithIceCreams = new Stand(iceCream);
            standWithIceCreams.OnWorkCompleted += Stand_OnWorkCompleted;
            Stand standWithCakes = new Stand(cake);
            standWithCakes.OnWorkCompleted += Stand_OnWorkCompleted;
            Stand standWithChocolates = new Stand(chocolate);
            standWithChocolates.OnWorkCompleted += Stand_OnWorkCompleted;

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

        public int VisitorsCount
        {
            get
            {
                return _visitorsCount;
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
            ConsoleHelper.WriteInfo("Shop is opening...");
            foreach (var stand in _stands)
            {
                stand.Open();
            }
            ConsoleHelper.WriteInfo("Shop is opened. Work is started.");
        }

        public void Close()
        {
            ConsoleHelper.WriteInfo("Shop is closing...");
            while (ActiveVisitorsCount > 0)
            {
                Thread.Sleep(1000);
                ConsoleHelper.WriteInfo(String.Format("Active buyers: {0}", ActiveVisitorsCount));
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
                _visitorsCount++;
            }
        }

        public void UnregisterBuyer()
        {
            lock (_activeVisitorsCountLocker)
            {
                _activeVisitorsCount--;
            }
        }

        public void ShowStatistic()
        {
            ConsoleHelper.WriteSuccess(String.Format("Visitors: {0}", VisitorsCount));
            foreach (var stand in _stands)
            {
                stand.ShowStatistic();
            }
        }

        private void Stand_OnWorkCompleted(object sender, EventArgs e)
        {
            Stand stand = sender as Stand;
            if (stand != null)
            {
                stand.OnWorkCompleted -= Stand_OnWorkCompleted;

                lock (_standsListLocker)
                {
                    TotalProfit += stand.SelledProductsCount * stand.Product.Price;
                    stand.ShowStatistic();
#if DEBUG
                    ConsoleHelper.WriteInfo(String.Format("The stand with {0}s is closed.", stand.Product.Name));
#endif
                    _stands.Remove(stand);
                    if (_stands.Count == 0)
                    {
                        ConsoleHelper.WriteInfo("Shop is closed");
                        _workCompleted.Set();
                    }
                }
            }
        }
    }
}
