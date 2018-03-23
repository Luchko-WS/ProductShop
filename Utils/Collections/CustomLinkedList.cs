using System.Collections;
using System.Collections.Generic;

namespace Utils.Collections
{
    public class CustomLinkedList<T> : CustomLinkedListBase<T>, IEnumerable<T>
    {
        public Node<T> First
        {
            get
            {
                return _head;
            }
        }
        
        public Node<T> Last
        {
            get
            {
                return _tail;
            }
        }

        public int Count
        {
            get
            {
                return GetCount();
            }
        }

        public void Add(T value)
        {
            AddToEnd(value);
        }

        public bool Remove(T item)
        {
            if (_head != null)
            {
                Node<T> current = _head;
                while (current != null)
                {
                    if (current.Value.Equals(item))
                    {
                        RemoveNode(current);
                    }
                    current = current.NextNode;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_head != null)
            {
                Node<T> _current = _head;
                while (_current != null)
                {
                    var val = _current.Value;
                    _current = _current.NextNode;
                    yield return val;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}