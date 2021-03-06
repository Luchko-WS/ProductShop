﻿using ProductShop.Actors;
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
            int sellersCount = rnd.Next(ProgramConfig.StandConfig.MinNumberOfSellers, ProgramConfig.StandConfig.MaxNumberOfSellers);

            for (int i = 0; i < sellersCount; i++)
            {
                var seller = new Seller(this);
                seller.OnWorkCompleted += Seller_OnWorkCompleted;
                _sellersCount++;
            }
            ConsoleHelper.WriteInfo(String.Format("Sellers created: {0}", sellersCount));

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

        public int SelledProductsCount
        {
            get
            {
                return _selledProductsCount;
            }
        }

        public void Open()
        {
            EventHelper.Invoke(OnStandOpening, this);
        }

        public void Close()
        {
            EventHelper.Invoke(OnStandClosing, this);
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

        public void IncreaseProductsCount(int number)
        {
            lock (_productCountLocker)
            {
                _selledProductsCount += number;
            }
        }

        public void ShowStatistic()
        {
            var productCount = _selledProductsCount;
            ConsoleHelper.WriteSuccess(String.Format("The statistic of stand with {0}s.\n", _product.Name) +
                String.Format("Selled product: {0}, count: {1}, profit: {2}", _product.Name, productCount, productCount * _product.Price));
        }

        private void Seller_OnWorkCompleted(object sender, EventArgs e)
        {
            Seller seller = sender as Seller;
            if (seller != null)
            {
                seller.OnWorkCompleted -= Seller_OnWorkCompleted;

                lock (_sellersCountLocker)
                {
                    _sellersCount--;
#if DEBUG
                    ConsoleHelper.WriteInfo(String.Format("The seller of stand with {0}s has completed the job. {1} left.", _product.Name, _sellersCount));
#endif
                    if (_sellersCount == 0)
                    {
                        EventHelper.Invoke(OnWorkCompleted, this);
                    }
                }
            }
        }

        public event EventHandler OnStandOpening;
        public event EventHandler OnStandClosing;
        public event EventHandler OnWorkCompleted;
    }
}
