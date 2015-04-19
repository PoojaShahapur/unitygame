namespace SDK.Common
{
    public class LockQueue<T>
    {
        protected LockList<T> m_list;

        public LockQueue(string name, int sizePerElement)
        {
            m_list = new LockList<T>("name", sizePerElement);
        }

        public void push(T item)
        {
            m_list.Add(item);
        }

        public T pop()
        {
            return m_list.RemoveAt(0);
        }
    }
}