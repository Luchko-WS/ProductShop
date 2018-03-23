namespace Utils.Collections
{
    public class CustomLinkedListBase<T>
    {
        protected Node<T> _head;
        protected Node<T> _tail;
        protected int _count;

        public CustomLinkedListBase()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        protected int GetCount()
        {
            return _count;
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