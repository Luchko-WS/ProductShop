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
            _stand.OnStandOpening += _stand_OnStandOpening;
            _stand.OnStandClosing += _stand_OnStandClosing;

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
                    Buyer buyer;
                    bool res = _stand.TryGetBuyerFromQueue(out buyer);
                    if(res) ServeBuyer(buyer);
                }
                EventHelper.Invoke(OnWorkCompleted, this);
            });

            _helper = new SellerHelper(this);
            sellerThread.Start();
        }

        private void ServeBuyer(Buyer buyer)
        {
            Random rnd = new Random();
            int servingTime = rnd.Next(ProgramConfig.SellerConfig.MinServingTime, ProgramConfig.SellerConfig.MaxServingTime);

            //work imitation
            Thread.Sleep(servingTime);

            _stand.IncreaseProductsCount(buyer.ProductsCount);
            _helper.AddBuyerToQueue(buyer);      
        }

        private void _stand_OnStandOpening(object sender, EventArgs e)
        {
            IsWorkTime = true;
            DoWork();
        }

        private void _stand_OnStandClosing(object sender, EventArgs e)
        {
            IsWorkTime = false;
            _stand.OnStandOpening -= _stand_OnStandOpening;
            _stand.OnStandClosing -= _stand_OnStandClosing;
        }

        public event EventHandler OnWorkCompleted;
    }
}
