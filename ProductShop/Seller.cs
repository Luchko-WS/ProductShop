using System;
using System.Threading;
using Utils;

namespace ProductShop
{
    public class Seller
    {
        private Stand _stand;
        private bool _isWorkTime = false;
        private object _locker;

        public Seller(Stand stand)
        {
            _stand = stand ?? throw new ArgumentNullException("Stand can't be null");
            _stand.OpenStand += _stand_OpenStand;
            _stand.CloseStand += _stand_CloseStand;

            _locker = new object();
        }

        private void DoWork()
        {
            Thread sellerThread = new Thread(() =>
            {
                while (true)
                {
                    bool isWorking;
                    lock (_locker)
                    {
                        isWorking = _isWorkTime;
                    }

                    bool res = _stand.TryGetBuyerFromQueue(out Buyer buyer);                   

                    if (!isWorking && !res) break;
                    ServeBuyer(buyer);
                }
                EventHelper.Invoke(WorkCompleted, this);
            });
        }

        private void ServeBuyer(Buyer buyer)
        {
            Random rnd = new Random();
            int servingTime = rnd.Next(100, 500);
            
            //work imitation
            Thread.Sleep(servingTime);

            _stand.IncreaseProductsCount(buyer.ProductsNumber);

            //!!!!!!!!!!!!!!!!! recieve buyer to helper
        }

        private void _stand_OpenStand(object sender, EventArgs e)
        {
            _isWorkTime = true;
            DoWork();
        }

        private void _stand_CloseStand(object sender, EventArgs e)
        {
            lock (_locker)
            {
                _isWorkTime = false;
            }
        }

        public event EventHandler WorkCompleted;
    }
}
