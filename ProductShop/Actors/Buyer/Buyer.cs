using System;
using System.Threading;
using Utils;
using Utils.Collections;

namespace ProductShop.Actors
{
    public class Buyer
    {
        private Shop _shop;
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
                int minCount = _standsToVisit.First.Value.GetCountOfBuyersInQueue();
                selectedStand = _standsToVisit.First.Value;

                var currentNode = _standsToVisit.First;
                while (currentNode.NextNode != null)
                {
                    currentNode = currentNode.NextNode;
                    if (minCount > currentNode.Value.GetCountOfBuyersInQueue())
                    {
                        minCount = currentNode.Value.GetCountOfBuyersInQueue();
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
            stand.AddBuyerToQueue(this);
        }
    }
}
