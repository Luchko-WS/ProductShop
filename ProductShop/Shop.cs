using Utils;
using System;
using System.Threading;
using Utils.Collections;

namespace ProductShop
{
    public class Shop
    {
        private CustomLinkedList<Stand> _stands;
        private object _standsListLocker;
        private EventWaitHandle _workCompleted;

        public Shop()
        {
            Product iceCream = new Product("Ice Cream", 12);
            Product cake = new Product("Cake", 35);
            Product chocolate = new Product("Chocolate", 24);

            Stand standWithIceCreams = new Stand(iceCream);
            standWithIceCreams.WorkCompleted += Stand_WorkCompleted;
            Stand standWithCakes = new Stand(cake);
            standWithCakes.WorkCompleted += Stand_WorkCompleted;
            Stand standWithChocolates = new Stand(chocolate);
            standWithChocolates.WorkCompleted += Stand_WorkCompleted;

            _stands = new CustomLinkedList<Stand>();
            _stands.Add(standWithIceCreams);
            _stands.Add(standWithCakes);
            _stands.Add(standWithChocolates);

            _standsListLocker = new object();
            _workCompleted = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public CustomLinkedList<Stand> Stands
        {
            get
            {
                return _stands;
            }
        }

        public EventWaitHandle WorkCompleted
        {
            get
            {
                return _workCompleted;
            }
        }

        public void Open()
        {
            ConsoleHelper.Debug("Shop is opened");
            foreach (var stand in _stands)
            {
                stand.Open();
            }
        }

        public void Close()
        {
            ConsoleHelper.Debug("Shop are closing... Please wait...");
            foreach (var stand in _stands)
            {
                stand.Close();
            }
        }

        private void Stand_WorkCompleted(object sender, EventArgs e)
        {
            Stand stand = sender as Stand;
            if (stand != null) stand.WorkCompleted -= Stand_WorkCompleted;

            lock (_standsListLocker)
            {
                _stands.Remove(stand);
                if (_stands.Count == 0)
                {
                    _workCompleted.Set();
                }
            }
        }
    }
}
