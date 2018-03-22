using System.Collections;
using System.Collections.Generic;

namespace ProductShop.Utils.Collections
{
    public class LinkedListBase<T> : IEnumerable<T>
    {
        protected Node<T> _head;
        protected Node<T> _tail;
        protected int _count;

        public LinkedListBase()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        public int Count
        {
            get
            {
                return _count;
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

        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        public bool IsEmpty()
        {
            return _count == 0;
        }

        protected void AddToEnd(T value)
        {
            var node = new Node<T>
            {
                Value = value,
                PrevNode = _tail,
                NextNode = null
            };

            if (_tail != null)
            {
                _tail.NextNode = node;
            }
            else
            {
                _head = node;
            }
            _tail = node;
            _count++;
        }

        protected void RemoveNode(Node<T> node)
        {
            var prevNode = node.PrevNode;
            var nextNode = node.NextNode;
            if (prevNode != null)
            {
                prevNode.NextNode = nextNode;
            }
            else
            {
                _head = nextNode;
            }
            if (nextNode != null)
            {
                nextNode.PrevNode = prevNode;
            }
            else
            {
                _tail = prevNode;
            }
            _count--;
        }
    }
}