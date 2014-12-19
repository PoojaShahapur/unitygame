using SDK.Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

/**
 *@brief 环形缓冲区，不支持多线程写操作，但是支持单线程写，单线程读操作
 */
namespace SDK.Lib
{
    // 必须是线程安全的，否则很多地方都需要加锁
    public class CirculeBuffer
    {
        static public uint m_sHeaderSize = 4;   // 包长度占据几个字节

        // 这里面的 byte[] 会频繁操作，就直接写在这里
        protected uint m_iCapacity;         // 分配的内存空间大小，单位大小是字节
        protected uint m_iMaxCapacity;      // 最大允许分配的存储空间大小 
        protected uint m_size;              // 存储在当前环形缓冲区中的数量
        protected byte[] m_buff;            // 当前环形缓冲区

        protected uint m_begin;             // 存储空间的第一个索引
        protected uint m_end;               // 存储空间的最后一个索引
        protected uint m_first;             // 当前缓冲区数据的第一个索引
        protected uint m_last;              // 当前缓冲区数据的最后一个索引的后面一个索引

        protected ByteArray m_headerBA;     // 主要是用来分析头的大小
        protected ByteArray m_retBA;        // 返回的字节数组
        protected ByteArray m_tmpBA;        // 临时数据

        private Mutex m_visitMutex = new Mutex();   // 主要是添加和获取数据互斥

        public CirculeBuffer()
        {
            m_iMaxCapacity = 8 * 1024 * 1024;      // 最大允许分配 8 M
            m_iCapacity = 1 * 1024;               // 默认分配 1 K
            m_size = 0;
            m_buff = new byte[m_iCapacity];

            m_begin = 0;
            m_end = m_iCapacity - 1;
            m_first = 0;
            m_last = 0;

            m_headerBA = new ByteArray();
            m_retBA = new ByteArray();
            m_tmpBA = new ByteArray();
        }

        public bool isLinearized()
        {
            return m_first < m_last || m_last == m_begin;
        }

        public byte[] buff
        {
            get
            {
                return m_buff;
            }
        }

        public uint size
        {
            get
            {
                return m_size;
            }
            set
            {
                m_size = value;
            }
        }

        public bool empty()
        {
            return size == 0;
        }

        public uint capacity()
        {
            return m_iCapacity;
        }

        public bool full()
        { 
            return capacity() == size;
        }

        public ByteArray headerBA
        {
            get
            {
                return m_headerBA;
            }
        }

        public ByteArray retBA
        {
            get
            {
                return m_retBA;
            }
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
            if (m_first < m_last || m_last == m_begin)      // 数据已经是在一块连续的内存空间
            {
                return;
            }
            else
            {
                // 数据在两个不连续的内存空间中
                char[] tmp = new char[m_last];
                Array.Copy(m_buff, 0, tmp, 0, m_last);  // 拷贝一段内存空间中的数据到 tmp
                Array.Copy(m_buff, m_first, m_buff, 0, m_iCapacity - m_first);
            }
        }

        /**
         * @brief 更改存储内容空间大小
         */
        protected void setCapacity(uint newCapacity) 
        {
            m_visitMutex.WaitOne();
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
                Array.Copy(m_buff, 0, tmpbuff, 0, m_size);
            }
            else    // 如果在两端内存空间
            {
                Array.Copy(m_buff, m_first, tmpbuff, 0, m_iCapacity - m_first);
                Array.Copy(m_buff, 0, tmpbuff, m_iCapacity - m_first, m_last);
            }

            m_first = 0;
            m_last = m_size;
            m_iCapacity = newCapacity;
            m_visitMutex.ReleaseMutex();
        }

        /**
         *@brief 向存储空尾部添加一段内容
         */
        public void pushBackArr(byte[] items, uint start, uint len)
        {
            m_visitMutex.WaitOne();
            if (!canAddData(len)) // 存储空间必须要比实际数据至少多 1
            {
                uint closeSize = UtilMath.getCloseSize(len + this.m_size, m_iCapacity, m_iMaxCapacity);
                setCapacity(closeSize);
            }

            if (isLinearized())
            {
                if (len <= (m_iCapacity - m_last))
                {
                    Array.Copy(items, start, m_buff, m_last, len);
                }
                else
                {
                    Array.Copy(items, start, m_buff, m_last, m_iCapacity - m_last);
                    Array.Copy(items, m_iCapacity - m_last, m_buff, 0, len - (m_iCapacity - m_last));
                }
            }
            else
            {
                Array.Copy(items, start, m_buff, m_last, len);
            }

            m_last += len;
            m_last %= m_iCapacity;

            m_size += len;
            m_visitMutex.ReleaseMutex();
        }

        public void pushBackBA(ByteArray ba)
        {
            m_visitMutex.WaitOne();
            //pushBack(ba.dynBuff.buff, ba.position, ba.bytesAvailable);
            pushBackArr(ba.dynBuff.buff, 0, ba.length);
            m_visitMutex.ReleaseMutex();
        }

        /**
         *@brief 向存储空头部添加一段内容
         */
        protected void pushFrontArr(byte[] items)
        {
            m_visitMutex.WaitOne();
            if (!canAddData((uint)items.Length)) // 存储空间必须要比实际数据至少多 1
            {
                uint closeSize = UtilMath.getCloseSize((uint)items.Length + this.m_size, m_iCapacity, m_iMaxCapacity);
                setCapacity(closeSize);
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
            m_visitMutex.ReleaseMutex();
        }

        /**
         *@brief 能否添加 num 长度的数据
         */
        protected bool canAddData(uint num)
        {
            if (m_iCapacity - m_size > num)
            {
                return true;
            }

            return false;
        }

        /**
         * @brief 从 CB 中读取内容
         * @param movefirst 是否将数据移除 CB
         */
        protected void readByteToByteArray(ByteArray bytearray, uint len, bool movefirst)
        {
            m_visitMutex.WaitOne();
            bytearray.clear();          // 设置数据为初始值
            if (m_size >= len)          // 头部占据 4 个字节
            {
                if(isLinearized())      // 在一段连续的内存
                {
                    bytearray.writeBytes(m_buff, m_first, len);
                    if (movefirst)
                    {
                        m_first += len;
                        m_size -= len;
                    }
                }
                else if (m_iCapacity - m_first >= len)
                {
                    bytearray.writeBytes(m_buff, m_first, len);
                    if (movefirst)
                    {
                        m_first += len;
                        m_size -= len;
                    }
                }
                else
                {
                    bytearray.writeBytes(m_buff, m_first, m_iCapacity - m_first);
                    bytearray.writeBytes(m_buff, 0, len - (m_iCapacity - m_first));
                    if (movefirst)
                    {
                        m_first = len - (m_iCapacity - m_first);
                        m_size -= len;
                    }
                }
            }

            bytearray.position = 0;        // 设置数据读取起始位置
            m_visitMutex.ReleaseMutex();
        }

        /**
         * @brief 检查 CB 中是否有一个完整的消息
         */
        protected bool checkHasMsg()
        {
            readByteToByteArray(m_headerBA, m_sHeaderSize, false);  // 将数据读取到 m_headerBA
            if (m_headerBA.readUnsignedInt() <= m_size - m_sHeaderSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * @brief 从 CB 头部删除数据
         */
        protected void removeByLen(uint len)
        {
            if (isLinearized())  // 在一段连续的内存
            {
                m_first += len;
            }
            else if (m_iCapacity - m_first >= len)
            {
                m_first += len;
            }
            else
            {
                m_first = len - (m_iCapacity - m_first);
            }

            m_size -= len;
        }

        /**
         *@brief 获取前面的第一个完整的消息数据块
         */
        public bool popFront()
        {
            m_visitMutex.WaitOne();
            bool ret = false;
            if (m_size > m_sHeaderSize)         // 至少要是 m_sHeaderSize 大小加 1 ，如果正好是 m_sHeaderSize ，那只能说是只有大小字段，没有内容
            {
                readByteToByteArray(m_headerBA, m_sHeaderSize, false);  // 如果不够整个消息的长度，还是不能去掉消息头的
                uint msglen = m_headerBA.readUnsignedInt();

                if (msglen <= m_size - m_sHeaderSize)
                {
                    removeByLen(m_sHeaderSize);
                    readByteToByteArray(m_retBA, msglen, true);
                    ret = true;
                }
            }

            if(empty())     // 如果已经清空，就直接重置
            {
                clear();
            }

            m_visitMutex.ReleaseMutex();
            return ret;
        }

        /**
         *@brief 数据放到流中
         */
        //public void getByte2Stream(NetworkStream ns, System.AsyncCallback cb)
        //{
        //    m_visitMutex.WaitOne();
        //    linearize();
        //    ns.BeginWrite(m_buff, (int)m_first, (int)m_size, cb, ns);

        //    // 清空数据
        //    m_first = 0;
        //    m_size = 0;
        //    m_last = 0;
        //    m_visitMutex.ReleaseMutex();
        //}

        // 向自己尾部添加一个 CirculeBuffer 
        public void pushBackCB(CirculeBuffer rhv)
        {
            m_visitMutex.WaitOne();
            if(this.m_iCapacity - this.m_size < rhv.size)
            {
                uint closeSize = UtilMath.getCloseSize(rhv.size + this.m_size, m_iCapacity, m_iMaxCapacity);
                setCapacity(closeSize);
            }
            //this.m_size += rhv.size;
            //this.m_last = this.m_size;

            //m_tmpBA.clear();
            rhv.readByteToByteArray(m_tmpBA, rhv.size, false);
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
            m_visitMutex.ReleaseMutex();
        }

        // 清空缓冲区
        public void clear()
        {
            m_visitMutex.WaitOne();
            m_size = 0;
            m_first = 0;
            m_last = 0;
            m_visitMutex.ReleaseMutex();
        }
    }
}