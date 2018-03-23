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
        private bool _isHelperCompletedWork = false;
        private object _locker;

        public Seller(Stand stand)
        {
            _stand = stand ?? throw new ArgumentNullException("Stand can't be null");
            _stand.OpenStand += _stand_OpenStand;
            _stand.CloseStand += _stand_CloseStand;

            _locker = new object();
        }

        public bool IsWorkTime
        {
            get
            {
                lock (_locker)
                {
                    return _isWorkTime;
                }
            }
            private set
            {
                lock (_locker)
                {
                    _isWorkTime = value;
                }
            }
        }

        private void DoWork()
        {
            Thread sellerThread = new Thread(() =>
            {
                while (true)
                {
                    bool res = _stand.TryGetBuyerFromQueue(out Buyer buyer);                   
                    if (!IsWorkTime && !res && _isHelperCompletedWork) break;
                    if(res) ServeBuyer(buyer);
                }
                EventHelper.Invoke(WorkCompleted, this);
            });

            _helper = new SellerHelper(this);
            _helper.WorkCompleted += _helper_WorkCompleted;
            sellerThread.Start();
        }

        private void ServeBuyer(Buyer buyer)
        {
            Random rnd = new Random();
            int servingTime = rnd.Next(100, 500);
            
            //work imitation
            Thread.Sleep(servingTime);

            _stand.IncreaseProductsCount(buyer.ProductsNumber);
            _helper.TryAddBuyerToQueue(buyer);      
        }

        private void _stand_OpenStand(object sender, EventArgs e)
        {
            IsWorkTime = true;
            DoWork();
        }

        private void _stand_CloseStand(object sender, EventArgs e)
        {
            IsWorkTime = false;
        }

        private void _helper_WorkCompleted(object sender, EventArgs e)
        {
            _isHelperCompletedWork = true;
        }

        public event EventHandler WorkCompleted;
    }
}
