namespace ProductShop.Utils.Collections
{
    public class CustomQueue<T> : LinkedListBase<T>
    {
        public void Push(T value)
        {
            AddToEnd(value);
        }

        public T Pop()
        {
            var item = _head.Value;
            RemoveNode(_head);
            return item;
        }

        public T Peek()
        {
            return _head.Value;
        }
    }
}