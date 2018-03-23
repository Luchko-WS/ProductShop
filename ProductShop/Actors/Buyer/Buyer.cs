using System;
using System.Threading;
using Utils;
using Utils.Collections;

namespace ProductShop.Actors
{
    public class Buyer
    {
        private CustomLinkedList<Stand> _standToVisit;
        private int _productsNumber;

        public int ProductsNumber
        {
            get
            {
                return _productsNumber;
            }
        }
        
        public Buyer(CustomLinkedList<Stand> stands)
        {
            if(stands == null) throw new ArgumentNullException("Stands list can't be null");

            _standToVisit = new CustomLinkedList<Stand>();
            foreach (var stand in stands)
            {
                _standToVisit.Add(stand);
            }

            Thread buyerThread = new Thread(() =>
            {
                DoWork();
            });

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
                EventHelper.Invoke(GoHome, this);
            }
        }

        private Stand SelectStand()
        {
            Stand selectedStand = null;
            if (_standToVisit.Count != 0)
            {
                int minCount = _standToVisit.First.Value.GetCountOfBuyersInQueue();
                selectedStand = _standToVisit.First.Value;

                var currentNode = _standToVisit.First;
                while (currentNode.NextNode != null)
                {
                    currentNode = currentNode.NextNode;
                    if (minCount > currentNode.Value.GetCountOfBuyersInQueue())
                    {
                        minCount = currentNode.Value.GetCountOfBuyersInQueue();
                        selectedStand = currentNode.Value;
                    }
                }
                _standToVisit.Remove(selectedStand);
            }
            return selectedStand;
        }

        private void BuyProductsFromStand(Stand stand)
        {
            Random rnd = new Random();
            _productsNumber = rnd.Next(1, 3);
            stand.TryAddBuyerToQueue(this);
        }

        public event EventHandler GoHome;
    }
}
