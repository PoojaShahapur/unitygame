using SDK.Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

/**
 *@brief 环形缓冲区，支持多线程写操作
 */
namespace SDK.Common
{
    // 必须是线程安全的，否则很多地方都需要加锁
    public class CirculeBuffer
    {
        // 这里面的 byte[] 会频繁操作，就直接写在这里
        protected DynamicBuffer m_dynamicBuffer;

        protected uint m_first;             // 当前缓冲区数据的第一个索引
        protected uint m_last;              // 当前缓冲区数据的最后一个索引的后面一个索引
        protected ByteBuffer m_tmpBA;        // 临时数据

        public CirculeBuffer(uint initCapacity = DynamicBuffer.INIT_CAPACITY, uint maxCapacity = DynamicBuffer.MAX_CAPACITY)
        {
            m_dynamicBuffer = new DynamicBuffer(initCapacity, maxCapacity);

            m_first = 0;
            m_last = 0;

            m_tmpBA = new ByteBuffer();
        }

        public bool isLinearized()
        {
            return m_first < m_last;
        }

        public byte[] buff
        {
            get
            {
                return m_dynamicBuffer.m_buff;
            }
        }

        public uint size
        {
            get
            {
                return m_dynamicBuffer.m_size;
            }
            set
            {
                m_dynamicBuffer.size = value;
            }
        }

        public bool empty()
        {
            return m_dynamicBuffer.m_size == 0;
        }

        public uint capacity()
        {
            return m_dynamicBuffer.m_iCapacity;
        }

        public bool full()
        { 
            return capacity() == size;
        }

        public uint first
        {
            get
            {
                return m_first;
            }
        }

        public uint last
        {
            get
            {
                return m_last;
            }
        }

        /**
         * @brief 将数据尽量按照存储地址的从小到大排列
         */
        protected void linearize()
        {
            if (empty())        // 没有数据
            {
                return;
            }
            if (isLinearized())      // 数据已经是在一块连续的内存空间
            {
                return;
            }
            else
            {
                // 数据在两个不连续的内存空间中
                char[] tmp = new char[m_last];
                Array.Copy(m_dynamicBuffer.m_buff, 0, tmp, 0, m_last);  // 拷贝一段内存空间中的数据到 tmp
                Array.Copy(m_dynamicBuffer.m_buff, m_first, m_dynamicBuffer.m_buff, 0, m_dynamicBuffer.m_iCapacity - m_first);
            }
        }

        /**
         * @brief 更改存储内容空间大小
         */
        protected void setCapacity(uint newCapacity) 
        {
            if (newCapacity == capacity())
            {
                return;
            }
            if (newCapacity < size)       // 不能分配比当前已经占有的空间还小的空间
            {
                return;
            }
            byte[] tmpbuff = new byte[newCapacity];   // 分配新的空间
            if (isLinearized()) // 如果是在一段内存空间
            {
                Array.Copy(m_dynamicBuffer.m_buff, 0, tmpbuff, 0, m_dynamicBuffer.m_size);
            }
            else    // 如果在两端内存空间
            {
                Array.Copy(m_dynamicBuffer.m_buff, m_first, tmpbuff, 0, m_dynamicBuffer.m_iCapacity - m_first);
                Array.Copy(m_dynamicBuffer.m_buff, 0, tmpbuff, m_dynamicBuffer.m_iCapacity - m_first, m_last);
            }

            m_first = 0;
            m_last = m_dynamicBuffer.m_size;
            m_dynamicBuffer.m_iCapacity = newCapacity;
        }

        /**
         *@brief 向存储空尾部添加一段内容
         */
        public void pushBackArr(byte[] items, uint start, uint len)
        {
            if (!canAddData(len)) // 存储空间必须要比实际数据至少多 1
            {
                uint closeSize = UtilMath.getCloseSize(len + m_dynamicBuffer.m_size, m_dynamicBuffer.m_iCapacity, m_dynamicBuffer.m_iMaxCapacity);
                setCapacity(closeSize);
            }

            if (isLinearized())
            {
                if (len <= (m_dynamicBuffer.m_iCapacity - m_last))
                {
                    Array.Copy(items, start, m_dynamicBuffer.m_buff, m_last, len);
                }
                else
                {
                    Array.Copy(items, start, m_dynamicBuffer.m_buff, m_last, m_dynamicBuffer.m_iCapacity - m_last);
                    Array.Copy(items, m_dynamicBuffer.m_iCapacity - m_last, m_dynamicBuffer.m_buff, 0, len - (m_dynamicBuffer.m_iCapacity - m_last));
                }
            }
            else
            {
                Array.Copy(items, start, m_dynamicBuffer.m_buff, m_last, len);
            }

            m_last += len;
            m_last %= m_dynamicBuffer.m_iCapacity;

            m_dynamicBuffer.m_size += len;
        }

        public void pushBackBA(ByteBuffer ba)
        {
            //pushBack(ba.dynBuff.buff, ba.position, ba.bytesAvailable);
            pushBackArr(ba.dynBuff.buff, 0, ba.length);
        }

        /**
         *@brief 向存储空头部添加一段内容
         */
        protected void pushFrontArr(byte[] items)
        {
            if (!canAddData((uint)items.Length)) // 存储空间必须要比实际数据至少多 1
            {
                uint closeSize = UtilMath.getCloseSize((uint)items.Length + m_dynamicBuffer.m_size, m_dynamicBuffer.m_iCapacity, m_dynamicBuffer.m_iMaxCapacity);
                setCapacity(closeSize);
            }

            if (isLinearized())
            {
                if (items.Length <= m_first)
                {
                    Array.Copy(items, 0, m_dynamicBuffer.m_buff, m_first - items.Length, items.Length);
                }
                else
                {
                    Array.Copy(items, items.Length - m_first, m_dynamicBuffer.m_buff, 0, m_first);
                    Array.Copy(items, 0, m_dynamicBuffer.m_buff, m_dynamicBuffer.m_iCapacity - (items.Length - m_first), items.Length - m_first);
                }
            }
            else
            {
                Array.Copy(items, 0, m_dynamicBuffer.m_buff, m_first - items.Length, items.Length);
            }

            if (items.Length <= m_first)
            {
                m_first -= (uint)items.Length;
            }
            else
            {
                m_first = m_dynamicBuffer.m_iCapacity - ((uint)items.Length - m_first);
            }
            m_dynamicBuffer.m_size += (uint)items.Length;
        }

        /**
         *@brief 能否添加 num 长度的数据
         */
        protected bool canAddData(uint num)
        {
            if (m_dynamicBuffer.m_iCapacity - m_dynamicBuffer.m_size > num)
            {
                return true;
            }

            return false;
        }

        /**
         * @brief 从 CB 中读取内容，并且将数据删除
         */
        public void popFrontBA(ByteBuffer bytearray, uint len)
        {
            frontBA(bytearray, len);
            popFrontLen(len);
        }

        // 仅仅是获取数据，并不删除
        public void frontBA(ByteBuffer bytearray, uint len)
        {
            bytearray.clear();          // 设置数据为初始值
            if (m_dynamicBuffer.m_size >= len)          // 头部占据 4 个字节
            {
                if (isLinearized())      // 在一段连续的内存
                {
                    bytearray.writeBytes(m_dynamicBuffer.m_buff, m_first, len);
                }
                else if (m_dynamicBuffer.m_iCapacity - m_first >= len)
                {
                    bytearray.writeBytes(m_dynamicBuffer.m_buff, m_first, len);
                }
                else
                {
                    bytearray.writeBytes(m_dynamicBuffer.m_buff, m_first, m_dynamicBuffer.m_iCapacity - m_first);
                    bytearray.writeBytes(m_dynamicBuffer.m_buff, 0, len - (m_dynamicBuffer.m_iCapacity - m_first));
                }
            }

            bytearray.position = 0;        // 设置数据读取起始位置
        }

        /**
         * @brief 从 CB 头部删除数据
         */
        public void popFrontLen(uint len)
        {
            if (isLinearized())  // 在一段连续的内存
            {
                m_first += len;
            }
            else if (m_dynamicBuffer.m_iCapacity - m_first >= len)
            {
                m_first += len;
            }
            else
            {
                m_first = len - (m_dynamicBuffer.m_iCapacity - m_first);
            }

            m_dynamicBuffer.m_size -= len;
        }

        // 向自己尾部添加一个 CirculeBuffer 
        public void pushBackCB(CirculeBuffer rhv)
        {
            if(m_dynamicBuffer.m_iCapacity - m_dynamicBuffer.m_size < rhv.size)
            {
                uint closeSize = UtilMath.getCloseSize(rhv.size + m_dynamicBuffer.m_size, m_dynamicBuffer.m_iCapacity, m_dynamicBuffer.m_iMaxCapacity);
                setCapacity(closeSize);
            }
            //this.m_size += rhv.size;
            //this.m_last = this.m_size;

            //m_tmpBA.clear();
            rhv.frontBA(m_tmpBA, rhv.size);
            pushBackBA(m_tmpBA);

            //if (rhv.isLinearized()) // 如果是在一段内存空间
            //{
            //    Array.Copy(rhv.buff, rhv.first, m_buff, 0, rhv.size);
            //}
            //else    // 如果在两端内存空间
            //{
            //    Array.Copy(rhv.buff, rhv.first, m_buff, 0, rhv.capacity() - rhv.first);
            //    Array.Copy(m_buff, 0, m_buff, rhv.capacity() - rhv.first, rhv.last);
            //}
            //rhv.clear();
        }

        // 清空缓冲区
        public void clear()
        {
            m_dynamicBuffer.m_size = 0;
            m_first = 0;
            m_last = 0;
        }
    }
}