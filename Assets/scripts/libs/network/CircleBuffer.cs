using System;
using System.Collections.Generic;

/**
 *@brief 环形缓冲区，不支持多线程写操作，但是支持单线程写，单线程读操作
 */
namespace San.Guo
{
    class CirculeBuffer
    {
        protected uint m_iCapacity;         // 分配的内存空间大小，单位大小是字节
        protected uint m_iMaxCapacity;      // 最大允许分配的存储空间大小 
        protected uint m_size;              // 存储在当前环形缓冲区中的数量

        protected char[] m_buff;            // 当前环形缓冲区
        protected uint m_begin;             // 存储空间的第一个索引
        protected uint m_end;               // 存储空间的最后一个索引
        protected uint m_first;             // 当前缓冲区数据的第一个索引
        protected uint m_last;              // 当前缓冲区数据的最后一个索引的后面一个索引

        public CirculeBuffer()
        {
            m_iMaxCapacity = 8 * 1024 * 1024;      // 最大允许分配 8 M
            m_iCapacity = 64 * 1024;               // 默认分配 64 K
            m_size = 0;
            m_buff = new char[m_iCapacity];

            m_begin = 0;
            m_end = m_iCapacity - 1;
            m_first = 0;
            m_last = 0;
        }

        public bool isLinearized()
        {
            return m_first < m_last || m_last == m_begin;
        }

        public uint size()
        {
            return m_size;
        }

        public bool empty()
        {
            return size() == 0;
        }

        public uint capacity()
        {
            return m_iCapacity;
        }

        public bool full()
        { 
            return capacity() == size();
        }

        /**
         * @brief 将数据尽量按照存储地址的从小到大排列
         */
        public void linearize()
        {
            if (empty())        // 没有数据
            {
                return;
            }
            if (m_first < m_last || m_last == m_begin)      // 数据已经是在一块连续的内存空间
            {
                return;
            }
            // 数据在两个不连续的内存空间中
            char[] tmp = new char[m_last];
            Array.Copy(m_buff, 0, tmp, 0, m_last);  // 拷贝一段内存空间中的数据到 tmp
            Array.Copy(m_buff, m_first, m_buff, 0, m_iCapacity - m_first);
        }

        /**
         * @brief 更改存储内容空间大小
         */
        public void setCapacity(uint newCapacity) 
        {
            if (newCapacity == capacity())
            {
                return;
            }
            if (newCapacity < size())       // 不能分配比当前已经占有的空间还小的空间
            {
                return;
            }
            char[] buff = new char[newCapacity];   // 分配新的空间
            if (isLinearized()) // 如果是在一段内存空间
            {
                Array.Copy(m_buff, 0, buff, 0, m_size);
            }
            else    // 如果在两端内存空间
            {
                Array.Copy(m_buff, m_first, buff, 0, m_iCapacity - m_first);
                Array.Copy(m_buff, 0, buff, m_iCapacity - m_first, m_last);
            }

            m_first = 0;
            m_last = m_size;
            m_iCapacity = newCapacity;
        }

        /**
         *@brief 向存储空尾部添加一段内容
         */
        public void pushBack(char[] items)
        {
            if (!canAddData((uint)items.Length)) // 存储空间必须要比实际数据至少多 1
            {
                if(2 * m_iCapacity <= m_iMaxCapacity)
                {
                    setCapacity(2 * m_iCapacity);
                }
                else
                {
                    setCapacity(m_iMaxCapacity);
                }
            }

            if (isLinearized())
            {
                if(items.Length <= (m_iCapacity - m_last))
                {
                    Array.Copy(items, 0, m_buff, m_last, items.Length);
                }
                else
                {
                    Array.Copy(items, 0, m_buff, m_last, m_iCapacity - m_last);
                    Array.Copy(items, m_iCapacity - m_last, m_buff, 0, items.Length - (m_iCapacity - m_last));
                }
            }
            else
            {
                Array.Copy(items, 0, m_buff, m_last, items.Length);
            }

            m_last += (uint)items.Length;
            m_last %= m_iCapacity;
            m_size += (uint)items.Length;
        }

        /**
         *@brief 向存储空头部添加一段内容
         */
        public void pushFront(char[] items)
        {
            if (!canAddData((uint)items.Length)) // 存储空间必须要比实际数据至少多 1
            {
                if (2 * m_iCapacity <= m_iMaxCapacity)
                {
                    setCapacity(2 * m_iCapacity);
                }
                else
                {
                    setCapacity(m_iMaxCapacity);
                }
            }

            if (isLinearized())
            {
                if (items.Length <= m_first)
                {
                    Array.Copy(items, 0, m_buff, m_first - items.Length, items.Length);
                }
                else
                {
                    Array.Copy(items, items.Length - m_first, m_buff, 0, m_first);
                    Array.Copy(items, 0, m_buff, m_iCapacity - (items.Length - m_first), items.Length - m_first);
                }
            }
            else
            {
                Array.Copy(items, 0, m_buff, m_first - items.Length, items.Length);
            }

            if (items.Length <= m_first)
            {
                m_first -= (uint)items.Length;
            }
            else
            {
                m_first = m_iCapacity - ((uint)items.Length - m_first);
            }
            m_size += (uint)items.Length;
        }

        /**
         *@brief 能否添加 num 长度的数据
         */
        public bool canAddData(uint num)
        {
            if (m_iCapacity - m_size > num)
            {
                return true;
            }

            return false;
        }

        /**
         *@brief 获取前面的数据
         */
        public  popFront()
    }
}