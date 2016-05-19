namespace SDK.Lib
{
    public class LockQueue<T>
    {
        protected LockList<T> m_list;

        public LockQueue(string name)
        {
            m_list = new LockList<T>("name");
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