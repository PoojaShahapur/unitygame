using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    /**
     *@brief 类似 flash 的 ByteArray 功能
     */
    public class ByteArray
    {
        static public Endian m_sEndian = Endian.NONE_ENDIAN;     // 当前机器的编码
        static public byte[] m_intByte = new byte[4];
        static public byte[] m_shortByte = new byte[2];

        //protected byte[] m_buff;            // 当前的缓冲区
        //protected uint m_length;            // 存储的数据的大小
        protected DynamicBuffer m_dynBuff;
        protected uint m_position;          // 当前可以读取的位置索引
        protected Endian m_endian;          // 大端小端

        public ByteArray()
        {
            m_endian = m_sEndian;
            //m_buff = new byte[64 * 1024];       // 默认大小 64 K
            m_dynBuff = new DynamicBuffer();
        }

        //public byte[] buff
        //{
        //    get
        //    {
        //        return m_buff;
        //    }
        //}

        public DynamicBuffer dynBuff
        {
            get
            {
                return m_dynBuff;
            }
        }

		public uint bytesAvailable
        {
            get
            {
                return (m_dynBuff.size - m_position);
            }
        }

		public Endian endian
        {
            get
            {
                return m_endian;
            }
            set
            {
                m_endian = value;
            }
        }

		public uint length
        {
            get
            {
                return m_dynBuff.size;
            }
            set
            {
                m_dynBuff.size = value;
            }
        }

		public uint position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
            }
        }

		public void clear ()
        {
            m_position = 0;
            m_dynBuff.size = 0;
        }

        // 检查是否有足够的大小可以扩展
        protected bool checkDelta(uint delta)
        {
            if(m_dynBuff.size + delta > m_dynBuff.capacity)
            {
                return false;
            }

            return true;
        }

        protected void extendDeltaCapicity(uint delta)
        {
            if (m_dynBuff.capacity * 2 >= m_dynBuff.capacity + delta)
            {
                m_dynBuff.capacity *= 2;
            }
            else
            {
                m_dynBuff.capacity += delta;
            }
        }

        protected void advancePosition(uint num)
        {
            m_position += num;
            length = m_position;
        }

        public void compress(CompressionAlgorithm algorithm = CompressionAlgorithm.ZLIB)
        {

        }

        public void uncompress ()
        {
    
        }

		public bool readBoolean ()
        {
            if (checkDelta(1))
            {
                advancePosition(1);
                return System.BitConverter.ToBoolean(m_dynBuff.buff, (int)(m_position - 1));
            }

            return false;
        }

		public int readByte ()
        {
            if (checkDelta(1))
            {
                advancePosition(1);
                return System.BitConverter.ToInt32(m_dynBuff.buff, (int)(m_position - 1));
            }

            return 0;
        }

		public int readInt ()
        {
            if (checkDelta(1))
            {
                advancePosition(4);
                if (m_endian == m_sEndian)
                {
                    return System.BitConverter.ToInt32(m_dynBuff.buff, (int)(m_position - 4));
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)(m_position - 4), m_intByte, 0, 4);
                    Array.Reverse(m_intByte);
                    return System.BitConverter.ToInt32(m_intByte, 0);
                }
            }

            return 0;
        }

		public string readMultiByte (uint length, Encoding charSet)
        {
            // http://blog.sina.com.cn/s/blog_6e51df7f0100tj9z.html
            // gbk和utf-8都是以单个字节表示数字的，所以不存在字节序问题，在多个不同系统架构都用。对于utf-16，则是以双字节表示一个整数，所以为会有字节序问题，分大小端unicode
            // http://msdn.microsoft.com/zh-cn/library/system.text.encoding.bigendianunicode%28v=vs.80%29.aspx
            // Encoding  u7    = Encoding.UTF7;
            // Encoding  u8    = Encoding.UTF8;
            // Encoding  u16LE = Encoding.Unicode;
            // Encoding  u16BE = Encoding.BigEndianUnicode;
            // Encoding  u32   = Encoding.UTF32;
            // 如果是 unicode ，需要大小端判断
            if (checkDelta(length))
            {
                advancePosition(length);
                return charSet.GetString(m_dynBuff.buff, (int)m_position, (int)length);
            }

            return "";
        }

		public int readShort ()
        {
            if (checkDelta(2))
            {
                advancePosition(2);
                if (m_endian == m_sEndian)
                {
                    return System.BitConverter.ToInt32(m_dynBuff.buff, (int)(m_position - 2));
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)(m_position - 2), m_shortByte, 0, 2);
                    Array.Reverse(m_intByte);
                    return System.BitConverter.ToInt16(m_shortByte, 0);
                }
            }

            return 0;
        }

		public uint readUnsignedByte ()
        {
            if (checkDelta(1))
            {
                advancePosition(1);
                return System.BitConverter.ToUInt32(m_dynBuff.buff, (int)(m_position - 1));
            }

            return 0;
        }

		public uint readUnsignedInt ()
        {
            if (checkDelta(4))
            {
                advancePosition(4);
                if (m_endian == m_sEndian)
                {
                    return System.BitConverter.ToUInt32(m_dynBuff.buff, (int)(m_position - 4));
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)(m_position - 4), m_intByte, 0, 4);
                    Array.Reverse(m_intByte);
                    return System.BitConverter.ToUInt32(m_intByte, 0);
                }
            }

            return 0;
        }

		public uint readUnsignedShort ()
        {
            if (checkDelta(4))
            {
                advancePosition(2);
                if (m_endian == m_sEndian)
                {
                    return System.BitConverter.ToUInt32(m_dynBuff.buff, (int)(m_position - 2));
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)(m_position - 2), m_shortByte, 0, 2);
                    Array.Reverse(m_intByte);
                    return System.BitConverter.ToUInt32(m_shortByte, 0);
                }
            }

            return 0;
        }

		public void writeByte (byte value)
        {
            if (!checkDelta(1))
            {
                extendDeltaCapicity(1);
            }
            m_dynBuff.buff[m_position] = value;
            advancePosition(1);
        }

		public void writeInt (int value)
        {
            if (!checkDelta(4))
            {
                extendDeltaCapicity(4);
            }
            if (m_endian == m_sEndian)
            {
                m_intByte = System.BitConverter.GetBytes(value);
                Array.Copy(m_intByte, 0, m_dynBuff.buff, m_position, 4);
            }
            else
            {
                m_intByte = System.BitConverter.GetBytes(value);
                Array.Reverse(m_intByte);
                Array.Copy(m_intByte, 0, m_dynBuff.buff, m_position, 4);
            }
            advancePosition(4);
        }

		public void writeMultiByte (string value, Encoding charSet)
        {
            char[] charPtr = value.ToCharArray();
            int num = charSet.GetByteCount(charPtr);

            if (!checkDelta((uint)num))
            {
                extendDeltaCapicity((uint)num);
            }

            Array.Copy(charSet.GetBytes(charPtr), 0, m_dynBuff.buff, m_position, num);
            advancePosition((uint)num);
        }

		public void writeShort (short value)
        {
            if (!checkDelta(2))
            {
                extendDeltaCapicity(2);
            }

            if (m_endian == m_sEndian)
            {
                m_shortByte = System.BitConverter.GetBytes(value);
                Array.Copy(m_shortByte, 0, m_dynBuff.buff, m_position, 2);
            }
            else
            {
                m_shortByte = System.BitConverter.GetBytes(value);
                Array.Reverse(m_shortByte);
                Array.Copy(m_shortByte, 0, m_dynBuff.buff, m_position, 2);
            }
            advancePosition(2);
        }

		public void writeUnsignedInt (uint value)
        {
            if (!checkDelta(4))
            {
                extendDeltaCapicity(4);
            }
            if (m_endian == m_sEndian)
            {
                m_intByte = System.BitConverter.GetBytes(value);
                Array.Copy(m_intByte, 0, m_dynBuff.buff, m_position, 4);
            }
            else
            {
                m_intByte = System.BitConverter.GetBytes(value);
                Array.Reverse(m_intByte);
                Array.Copy(m_intByte, 0, m_dynBuff.buff, m_position, 4);
            }
            advancePosition(4);
        }

        public void writeBytes(byte[] value, uint start, uint length)
        {
            if (!checkDelta(length))
            {
                extendDeltaCapicity(length);
            }
            Array.Copy(value, start, m_dynBuff.buff, m_position, length);
            advancePosition(length);
        }
    }
}
