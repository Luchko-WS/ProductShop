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
                while (true)
                {
                    bool res = TryGetBuyerFromQueue(out Buyer buyer);
                    if (!_seller.IsWorkTime && !res) break;
                    if (res) buyer.DoWork();
                }
                EventHelper.Invoke(WorkCompleted, this);
                ConsoleHelper.WhiteDanger(_buyerQueue.Count.ToString());
            });
            sellerHelperThread.Start();
        }

        private bool TryGetBuyerFromQueue(out Buyer buyer)
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

        public event EventHandler WorkCompleted;
    }
}
