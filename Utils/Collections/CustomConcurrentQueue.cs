using System;

namespace Utils.Collections
{
    public class CustomConcurrentQueue<T> : CustomLinkedListBase<T>
    {
        private object _locker;

        public CustomConcurrentQueue()
        {
            _locker = new object();
        }

        public int Count
        {
            get
            {
                lock (_locker)
                {
                    return GetCount();
                }
            }
        }

        public void Enqueue(T value)
        {
            lock (_locker)
            {
                AddToEnd(value);
            }
        }

        public T Dequeue()
        {
            lock (_locker)
            {
                if (_head == null) throw new InvalidOperationException("Queue is empty");
                var item = _head.Value;
                RemoveNode(_head);
                return item;
            }
        }

        public T Peek()
        {
            lock (_locker)
            {
                if (_head == null) throw new InvalidOperationException("Queue is empty");
                return _head.Value;
            }
        }
    }
}