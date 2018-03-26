using System;
using System.Threading;
using Utils;

namespace ProductShop.Actors
{
    public class Seller
    {
        private Stand _stand;
        private SellerHelper _helper;
        private bool _isWorkTime = false;
        private object _isWorkTimeLocker;

        public Seller(Stand stand)
        {
            _stand = stand ?? throw new ArgumentNullException("Stand can't be null");
            _stand.OpenStand += _stand_OpenStand;
            _stand.CloseStand += _stand_CloseStand;

            _isWorkTimeLocker = new object();
        }

        public bool IsWorkTime
        {
            get
            {
                lock (_isWorkTimeLocker)
                {
                    return _isWorkTime;
                }
            }
            private set
            {
                lock (_isWorkTimeLocker)
                {
                    _isWorkTime = value;
                }
            }
        }

        private void DoWork()
        {
            Thread sellerThread = new Thread(() =>
            {
                while (IsWorkTime)
                {
                    bool res = _stand.TryGetBuyerFromQueue(out Buyer buyer);
                    if(res) ServeBuyer(buyer);
                }
                EventHelper.Invoke(WorkCompleted, this);
            });

            _helper = new SellerHelper(this);
            sellerThread.Start();
        }

        private void ServeBuyer(Buyer buyer)
        {
            Random rnd = new Random();
            int servingTime = rnd.Next(10, 50);

            //work imitation
            Thread.Sleep(servingTime);

            _stand.IncreaseProductsCount(buyer.ProductsCount);
            _helper.AddBuyerToQueue(buyer);      
        }

        private void _stand_OpenStand(object sender, EventArgs e)
        {
            IsWorkTime = true;
            DoWork();
        }

        private void _stand_CloseStand(object sender, EventArgs e)
        {
            IsWorkTime = false;
            _stand.OpenStand -= _stand_OpenStand;
            _stand.CloseStand -= _stand_CloseStand;
        }

        public event EventHandler WorkCompleted;
    }
}
