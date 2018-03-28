using System;
using System.Threading;
using Utils;
using Utils.Collections;

namespace ProductShop.Actors
{
    public class Buyer
    {
        private Guid _guid;
        private Shop _shop;
        private Stand _currentStand;
        private CustomLinkedList<Stand> _standsToVisit;
        private int _productsCount;

        public int ProductsCount
        {
            get
            {
                return _productsCount;
            }
        }
        
        public Buyer(Shop shop)
        {
            _shop = shop ?? throw new ArgumentNullException("Shop can't be null");

            _guid = Guid.NewGuid();

            _standsToVisit = new CustomLinkedList<Stand>();
            foreach (var stand in shop.Stands)
            {
                _standsToVisit.Add(stand);
            }

            Thread buyerThread = new Thread(() =>
            {
                DoWork();
            });

            _shop.RegisterBuyer();
            buyerThread.Start();
        }

        public void DoWork()
        {
            LeaveCurrentStand();
            Stand stand = SelectStand();
            if (stand != null)
            {
                BuyProductsFromStand(stand);
            }
            else
            {
                _shop.UnregisterBuyer();
            }
        }

        private Stand SelectStand()
        {
            Stand selectedStand = null;
            if (_standsToVisit.Count != 0)
            {
                int minCount = _standsToVisit.First.Value.GetBuyersCountInQueue();
                selectedStand = _standsToVisit.First.Value;

                var currentNode = _standsToVisit.First;
                while (currentNode.NextNode != null)
                {
                    currentNode = currentNode.NextNode;
                    int buyersCountInQueue = currentNode.Value.GetBuyersCountInQueue();
                    if (minCount > buyersCountInQueue)
                    {
                        minCount = buyersCountInQueue;
                        selectedStand = currentNode.Value;
                    }
                }
                _standsToVisit.Remove(selectedStand);
            }
            return selectedStand;
        }

        private void BuyProductsFromStand(Stand stand)
        {
            Random rnd = new Random();
            _productsCount = rnd.Next(1, 3);
            SetCurrentStand(stand, _productsCount);
            stand.AddBuyerToQueue(this);
        }

        private void SetCurrentStand(Stand stand, int productCount)
        {
            _currentStand = stand;
            ConsoleHelper.WriteInfo($"\nBuyer {_guid.ToString()} say:\nI am coming to stand with {stand.Product.Name}s." +
                $" I want to buy {productCount} {stand.Product.Name}(s).");
        }

        private void LeaveCurrentStand()
        {
            if (_currentStand != null)
            {
                ConsoleHelper.WriteInfo($"\nBayer {_guid.ToString()} say:\nI am leaving the stand with {_currentStand.Product.Name}s.");
            }
        }
    }
}
