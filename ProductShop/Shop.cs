using Utils;
using System;
using System.Threading;

namespace ProductShop
{
    public class Shop
    {
        public event EventHandler WorkCompleted;
        public void Open()
        {
            UtilConsole.Debug("Shop is opened");
        }

        public void Close()
        {
            UtilConsole.Debug("Shop are closing... Please wait");
            Thread.Sleep(10000);
            var handler = WorkCompleted;
            if (handler != null)
            {
                handler.Invoke(this, null);
            }
        }
    }
}
