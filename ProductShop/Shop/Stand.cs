using ProductShop.Actors;
using System;
using Utils;
using Utils.Collections;

namespace ProductShop
{
    public class Stand
    {
        private Product _product;
        private CustomLinkedList<Seller> _sellers;
        private CustomConcurrentQueue<Buyer> _buyerQueue;
        private int _selledProductsCount;

        private object _sellersListLocker;
        private object _productCountLocker;

        public Stand(Product product)
        {
            //product init
            _product = product ?? throw new ArgumentNullException("Product can't be null");
            _selledProductsCount = 0;

            //sellers init
            Random rnd = new Random(DateTime.Now.Millisecond);
            int sellersCount = rnd.Next(3, 7);

            _sellers = new CustomLinkedList<Seller>();
            for(int i = 0; i < sellersCount; i++)
            {
                var seller = new Seller(this);
                seller.WorkCompleted += Seller_WorkCompleted;
                _sellers.Add(seller);
            }
            ConsoleHelper.WhiteInfo($"Sellers created: {sellersCount}");

            //buyers queue init
            _buyerQueue = new CustomConcurrentQueue<Buyer>();

            _sellersListLocker = new object();
            _productCountLocker = new object();
        }

        public Product Product
        {
            get
            {
                return _product;
            }
        }

        public void Open()
        {
            EventHelper.Invoke(OpenStand, this);
        }

        public void Close()
        {
            EventHelper.Invoke(CloseStand, this);
        }

        public int GetCountOfBuyersInQueue()
        {
            return _buyerQueue.Count;
        }

        public void TryAddBuyerToQueue(Buyer buyer)
        {
            _buyerQueue.Enqueue(buyer);
        }

        public bool TryGetBuyerFromQueue(out Buyer buyer)
        {
            return _buyerQueue.Dequeue(out buyer);
        }

        public int GetSelledProductsCount()
        {
            lock (_productCountLocker)
            {
                return _selledProductsCount;
            }
        }

        public void IncreaseProductsCount(int number)
        {
            lock (_productCountLocker)
            {
                _selledProductsCount += number;
            }
        }

        private void Seller_WorkCompleted(object sender, EventArgs e)
        {
            Seller seller = sender as Seller;
            if (seller != null)
            {
                seller.WorkCompleted -= Seller_WorkCompleted;

                lock (_sellersListLocker)
                {
                    _sellers.Remove(seller);
#if DEBUG
                    ConsoleHelper.WhiteInfo($"The seller of stand with {_product.Name}s has completed the job. {_sellers.Count} left.");
#endif
                    if (_sellers.Count == 0)
                    {
                        EventHelper.Invoke(WorkCompleted, this);
                    }
                }
            }
        }

        public event EventHandler OpenStand;
        public event EventHandler CloseStand;
        public event EventHandler WorkCompleted;
    }
}
