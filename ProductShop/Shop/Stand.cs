using ProductShop.Actors;
using System;
using Utils;
using Utils.Collections;

namespace ProductShop
{
    public class Stand
    {
        private Product _product;
        private int _sellersCount = 0;
        private CustomConcurrentQueue<Buyer> _buyerQueue;
        private int _selledProductsCount = 0;

        private object _sellersCountLocker;
        private object _productCountLocker;

        public Stand(Product product)
        {
            //product init
            _product = product ?? throw new ArgumentNullException("Product can't be null");

            //sellers init
            Random rnd = new Random(DateTime.Now.Millisecond);
            int sellersCount = rnd.Next(3, 7);

            for (int i = 0; i < sellersCount; i++)
            {
                var seller = new Seller(this);
                seller.WorkCompleted += Seller_WorkCompleted;
                _sellersCount++;
            }
            ConsoleHelper.WriteInfo($"Sellers created: {sellersCount}");

            //buyers queue init
            _buyerQueue = new CustomConcurrentQueue<Buyer>();

            _sellersCountLocker = new object();
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

        public int GetBuyersCountInQueue()
        {
            return _buyerQueue.Count;
        }

        public void AddBuyerToQueue(Buyer buyer)
        {
            _buyerQueue.Enqueue(buyer);
        }

        public bool TryGetBuyerFromQueue(out Buyer buyer)
        {
            buyer = null;
            try
            {
                buyer = _buyerQueue.Dequeue();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
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

                lock (_sellersCountLocker)
                {
                    _sellersCount--;
#if DEBUG
                    ConsoleHelper.WriteInfo($"The seller of stand with {_product.Name}s has completed the job. {_sellersCount} left.");
#endif
                    if (_sellersCount == 0)
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
