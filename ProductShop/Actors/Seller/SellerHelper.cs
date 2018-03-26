using System;
using System.Threading;
using Utils;
using Utils.Collections;

namespace ProductShop.Actors
{
    public class SellerHelper
    {
        private Seller _seller;
        private CustomConcurrentQueue<Buyer> _buyerQueue;

        public SellerHelper(Seller seller)
        {
            _seller = seller ?? throw new ArgumentNullException("Seller can't be null");
            _buyerQueue = new CustomConcurrentQueue<Buyer>();
            DoWork();
        }

        public void TryAddBuyerToQueue(Buyer buyer)
        {
            _buyerQueue.Enqueue(buyer);
        }

        private void DoWork()
        {
            Thread sellerHelperThread = new Thread(() =>
            {
                while (_seller.IsWorkTime)
                {
                    bool res = TryGetBuyerFromQueue(out Buyer buyer);
                    if (res) buyer.DoWork();
                }
            });
            sellerHelperThread.Start();
        }

        private bool TryGetBuyerFromQueue(out Buyer buyer)
        {
            return _buyerQueue.Dequeue(out buyer);
        }
    }
}
