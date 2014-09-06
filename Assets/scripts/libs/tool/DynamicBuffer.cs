using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    /**
     * @brief 动态增长的缓冲区
     */
    public class DynamicBuffer : Object
    {
        protected uint m_iCapacity;         // 分配的内存空间大小，单位大小是字节
        protected uint m_iMaxCapacity;      // 最大允许分配的存储空间大小 
        protected uint m_size;              // 存储在当前环形缓冲区中的数量

        protected byte[] m_buff;            // 当前环形缓冲区

        public DynamicBuffer()
        {
            m_iMaxCapacity = 8 * 1024 * 1024;      // 最大允许分配 8 M
            m_iCapacity = 64 * 1024;               // 默认分配 64 K
            m_size = 0;
            m_buff = new byte[m_iCapacity];
        }

        public byte[] buff
        {
            get
            {
                return m_buff;
            }
        }

        public uint capacity
        {
            get
            {
                return m_iCapacity;
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
    }
}
