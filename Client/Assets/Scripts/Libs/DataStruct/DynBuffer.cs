using System;

namespace SDK.Lib
{
    /**
     * @brief 动态增长的缓冲区，不是环形的，从 0 开始增长的
     */
    public class DynBuffer<T>
    {
        public uint m_iCapacity;         // 分配的内存空间大小，单位大小是字节
        public uint m_iMaxCapacity;      // 最大允许分配的存储空间大小 
        public uint m_size;              // 存储在当前缓冲区中的数量
        public T[] m_buff;            // 当前环形缓冲区

        public DynBuffer(uint initCapacity = 1 * 1024/*DataCV.INIT_CAPACITY*/, uint maxCapacity = 8 * 1024 * 1024/*DataCV.MAX_CAPACITY*/)      // mono 模板类中使用常亮报错， vs 可以
        {
            m_iMaxCapacity = maxCapacity;
            m_iCapacity = initCapacity;
            m_size = 0;
            m_buff = new T[m_iCapacity];
        }

        public T[] buff
        {
            get
            {
                return m_buff;
            }
            set
            {
                m_buff = value;
                m_iCapacity = (uint)m_buff.Length;
            }
        }

        public uint maxCapacity
        {
            get
            {
                return m_iMaxCapacity;
            }
        }

        public uint capacity
        {
            get
            {
                return m_iCapacity;
            }
            set
            {
                if (value == m_iCapacity)
                {
                    return;
                }
                if (value < this.size)       // 不能分配比当前已经占有的空间还小的空间
                {
                    return;
                }
                T[] tmpbuff = new T[value];   // 分配新的空间
                Array.Copy(m_buff, 0, tmpbuff, 0, m_size);  // 这个地方是 m_size 还是应该是 m_iCapacity，如果是 CircleBuffer 好像应该是 m_iCapacity，如果是 ByteBuffer ，好像应该是 m_iCapacity
                m_buff = tmpbuff;
                m_iCapacity = value;
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
                if (value > this.capacity)
                {
                    extendDeltaCapicity(value - size);
                }
                m_size = value;
            }
        }

        public void extendDeltaCapicity(uint delta)
        {
            capacity = DynBufResizePolicy.getCloseSize(size + delta, capacity, maxCapacity);
        }
    }
}