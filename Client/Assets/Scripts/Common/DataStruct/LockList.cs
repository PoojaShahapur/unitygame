using SDK.Lib;
using System;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 线程安全列表， T 是 Object ，便于使用 Equal 比较地址
     */
    public class LockList<T>
    {
        protected DynamicBuffer<T> m_dynamicBuffer;
        protected MMutex m_visitMutex;
        protected T m_retItem;

        public LockList(string name, int sizePerElement, uint initCapacity = 32/*DataCV.INIT_ELEM_CAPACITY*/, uint maxCapacity = 8 * 1024 * 1024/*DataCV.MAX_CAPACITY*/)
        {
            m_dynamicBuffer = new DynamicBuffer<T>(sizePerElement, initCapacity, maxCapacity);
            m_visitMutex = new MMutex(false, name);
        }

        public uint Count 
        { 
            get
            {
                using (MLock mlock = new MLock(m_visitMutex))
                {
                    return m_dynamicBuffer.m_size;
                }
            }
        }

        public T this[int index] 
        { 
            get
            {
                using (MLock mlock = new MLock(m_visitMutex))
                {
                    if (index < m_dynamicBuffer.m_size)
                    {
                        return m_dynamicBuffer.m_buff[index];
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
                    m_dynamicBuffer.m_buff[index] = value;
                }
            }
        }

        public void Add(T item)
        {
            using (MLock mlock = new MLock(m_visitMutex))
            {
                if (m_dynamicBuffer.m_size >= m_dynamicBuffer.m_iCapacity)
                {
                    m_dynamicBuffer.extendDeltaCapicity(1);
                }

                m_dynamicBuffer.m_buff[m_dynamicBuffer.m_size] = item;
                ++m_dynamicBuffer.m_size;
            }
        }

        public bool Remove(T item)
        {
            using (MLock mlock = new MLock(m_visitMutex))
            {
                int idx = 0;
                foreach (var elem in m_dynamicBuffer.m_buff)
                {
                    if(item.Equals(elem))       // 地址比较
                    {
                        this.RemoveAt(idx);
                        return true;
                    }

                    ++idx;
                }
                return false;
            }
        }

        public T RemoveAt(int index)
        {
            using (MLock mlock = new MLock(m_visitMutex))
            {
                if (index < m_dynamicBuffer.m_size)
                {
                    m_retItem = m_dynamicBuffer.m_buff[index];

                    if (index < m_dynamicBuffer.m_size)
                    {
                        if (index != m_dynamicBuffer.m_size - 1 && 1 != m_dynamicBuffer.m_size) // 如果删除不是最后一个元素或者总共就大于一个元素
                        {
                            Array.Copy(m_dynamicBuffer.m_buff, (index + 1) * m_dynamicBuffer.m_size, m_dynamicBuffer.m_buff, index * m_dynamicBuffer.m_size, (m_dynamicBuffer.m_size - 1 - index) * m_dynamicBuffer.m_sizePerElement);
                            --m_dynamicBuffer.m_size;
                        }

                        --m_dynamicBuffer.m_size;
                    }
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