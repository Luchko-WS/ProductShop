namespace Utils.Collections
{
    public class CustomList<T> : LinkedListBase<T>
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
    }
}