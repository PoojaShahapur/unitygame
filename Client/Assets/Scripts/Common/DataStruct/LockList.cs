using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 线程安全列表
     */
    public class LockList<T>
    {
        protected List<T> m_list = new List<T>();
        protected MMutex m_visitMutex;
        protected T m_retItem;

        public LockList(string name)
        {
            m_visitMutex = new MMutex(false, name);
        }

        public int Count 
        { 
            get
            {
                using (MLock mlock = new MLock(m_visitMutex))
                {
                    return m_list.Count;
                }
            }
        }

        public void Add(T item)
        {
            using (MLock mlock = new MLock(m_visitMutex))
            {
                m_list.Add(item);
            }
        }

        public T this[int index] 
        { 
            get
            {
                using (MLock mlock = new MLock(m_visitMutex))
                {
                    if (index < m_list.Count)
                    {
                        return m_list[index];
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }

            set
            {
                using (MLock mlock = new MLock(m_visitMutex))
                {
                    m_list[index] = value;
                }
            }
        }

        public bool Remove(T item)
        {
            using (MLock mlock = new MLock(m_visitMutex))
            {
                return m_list.Remove(item);
            }
        }

        public T RemoveAt(int index)
        {
            using (MLock mlock = new MLock(m_visitMutex))
            {
                if (index < m_list.Count)
                {
                    m_retItem = m_list[index];
                    m_list.RemoveAt(index);
                }
                else
                {
                    m_retItem = default(T);
                }

                return m_retItem;
            }
        }
    }
}