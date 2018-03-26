using System;
using System.Threading;
using Utils;
using Utils.Collections;

namespace ProductShop.Actors
{
    public class Buyer
    {
        private CustomLinkedList<Stand> _standsToVisit;
        private int _productsCount;

        public int ProductsCount
        {
            get
            {
                return _productsCount;
            }
        }
        
        public Buyer(CustomLinkedList<Stand> stands)
        {
            if(stands == null) throw new ArgumentNullException("Stands list can't be null");

            _standsToVisit = new CustomLinkedList<Stand>();
            foreach (var stand in stands)
            {
                _standsToVisit.Add(stand);
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
                EventHelper.Invoke(WorkCompleted, this);
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

        public event EventHandler WorkCompleted;
    }
}
