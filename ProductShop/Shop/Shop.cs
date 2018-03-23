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
        private EventWaitHandle _workCompleted;

        private int _activeVisitorsCount;
        private object _visitorsLocker;

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

            _stands = new CustomLinkedList<Stand>();
            _stands.Add(standWithIceCreams);
            _stands.Add(standWithCakes);
            _stands.Add(standWithChocolates);

            _standsListLocker = new object();
            _visitorsLocker = new object();
            _workCompleted = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public int Visitors { get; set; }
        public double TotalProfit { get; set; }

        public int ActiveVisitorsCount
        {
            get
            {
                lock (_visitorsLocker)
                {
                    return _activeVisitorsCount;
                }
            }
            set
            {
                lock (_visitorsLocker)
                {
                    _activeVisitorsCount = value;
                }
            }
        }

        public CustomLinkedList<Stand> Stands
        {
            get
            {
                return _stands;
            }
        }

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
            foreach (var stand in _stands)
            {
                stand.Close();
            }
        }

        //change this method
        private void Stand_WorkCompleted(object sender, EventArgs e)
        {
            Stand stand = sender as Stand;
            if (stand != null) stand.WorkCompleted -= Stand_WorkCompleted;

            lock (_standsListLocker)
            {
                int selledProductsCount = stand.GetSelledProductsCount();

                ConsoleHelper.WhiteInfo($"The statistic of stand with {stand.Product.Name}s.");
                ConsoleHelper.WhiteSuccess($"Selled products: {stand.Product.Name}, count: {selledProductsCount}, profit: {selledProductsCount * stand.Product.Price}");
                ConsoleHelper.WhiteInfo($"The stand with {stand.Product.Name}s is closed.");

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
