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

        public bool Dequeue(out T item)
        {
            lock (_locker)
            {
                if (_head == null)
                {
                    item = default(T);
                    return false;
                }
                item = _head.Value;
                RemoveNode(_head);
                return true;
            }
        }

        public bool Peek(out T item)
        {
            lock (_locker)
            {
                if (_head == null)
                {
                    item = default(T);
                    return false;
                }
                item = _head.Value;
                return true;
            }
        }
    }
}