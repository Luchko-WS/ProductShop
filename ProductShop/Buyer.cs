using System;
using System.Threading;
using Utils.Collections;

namespace ProductShop
{
    public class Buyer
    {
        private Shop _shop;
        private CustomLinkedList<Stand> _standToVisit;
        private int _productsNumber;

        public int ProductsNumber
        {
            get
            {
                return _productsNumber;
            }
        }
        
        public Buyer(Shop shop)
        {
            _shop = shop ?? throw new ArgumentNullException("Shop can't be null");

            _standToVisit = new CustomLinkedList<Stand>();
            foreach (var stand in _shop.Stands)
            {
                _standToVisit.Add(stand);
            }

            Thread sellerThread = new Thread(() =>
            {
                DoWork();
            });
        }

        public void DoWork()
        {
            Stand stand = SelectStand();

            Random rnd = new Random();
            _productsNumber = rnd.Next(1, 3);

            BuyProductsFromStand(stand);
        }

        private Stand SelectStand()
        {
            //to do
            return _shop.Stands.First.Value;
        }

        private void BuyProductsFromStand(Stand stand)
        {
            //to do
        }
    }
}
