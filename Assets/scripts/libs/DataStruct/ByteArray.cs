using System;
using System.Collections.Generic;
using System.Text;
using SDK.Common;

namespace SDK.Lib
{
    /**
     *@brief ByteArray 功能
     */
    public class ByteArray : IByteArray
    {
        static public Endian m_sEndian = Endian.NONE_ENDIAN;     // 当前机器的编码

        public byte[] m_intByte = new byte[4];
        public byte[] m_shortByte = new byte[2];
        public byte[] m_longByte = new byte[8];

        protected DynamicBuffer m_dynBuff;
        protected uint m_position;          // 当前可以读取的位置索引
        protected Endian m_endian;          // 大端小端

        protected string m_tmpStr;
        protected int m_tmpInt;
        protected uint m_tmpUint;
        protected ushort m_tmpUshort;
        protected ulong m_tmpUlong;
        protected bool m_tmpBool;
        protected byte m_tmpByte;

        public ByteArray()
        {
            m_endian = m_sEndian;
            m_dynBuff = new DynamicBuffer();
        }

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

        public void setPos(uint pos)
        {
            m_position = pos;
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
        protected bool canWrite(uint delta)
        {
            if(m_dynBuff.size + delta > m_dynBuff.capacity)
            {
                return false;
            }

            return true;
        }

        // 读取检查
        protected bool canRead(uint delta)
        {
            if (m_position + delta > m_dynBuff.size)
            {
                return false;
            }

            return true;
        }

        protected void extendDeltaCapicity(uint delta)
        {
            m_dynBuff.capacity = UtilMath.getCloseSize(m_dynBuff.size + delta, m_dynBuff.capacity, m_dynBuff.maxCapacity);
        }

        protected void advPos(uint num)
        {
            m_position += num;
        }

        protected void advPosAndLen(uint num)
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
            m_tmpBool = false;

            if (canRead(1))
            {
                m_tmpBool = System.BitConverter.ToBoolean(m_dynBuff.buff, (int)m_position);
                advPos(1);
            }

            return m_tmpBool;
        }

		public byte readByte ()
        {
            m_tmpByte = 0;

            if (canRead(1))
            {
                m_tmpByte = (byte)System.BitConverter.ToInt32(m_dynBuff.buff, (int)m_position);
                advPos(1);
            }

            return m_tmpByte;
        }

		public int readInt ()
        {
            m_tmpInt = 0;
            if (canRead(4))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpInt = System.BitConverter.ToInt32(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_intByte, 0, 4);
                    Array.Reverse(m_intByte);
                    m_tmpInt = System.BitConverter.ToInt32(m_intByte, 0);
                }
                advPos(4);
            }

            return m_tmpInt;
        }

		public string readMultiByte (uint len, Encoding charSet)
        {
            m_tmpStr = "";
            // http://blog.sina.com.cn/s/blog_6e51df7f0100tj9z.html
            // gbk和utf-8都是以单个字节表示数字的，所以不存在字节序问题，在多个不同系统架构都用。对于utf-16，则是以双字节表示一个整数，所以为会有字节序问题，分大小端unicode
            // http://msdn.microsoft.com/zh-cn/library/system.text.encoding.bigendianunicode%28v=vs.80%29.aspx
            // Encoding  u7    = Encoding.UTF7;
            // Encoding  u8    = Encoding.UTF8;
            // Encoding  u16LE = Encoding.Unicode;
            // Encoding  u16BE = Encoding.BigEndianUnicode;
            // Encoding  u32   = Encoding.UTF32;
            // 如果是 unicode ，需要大小端判断
            if (canRead(len))
            {
                m_tmpStr = charSet.GetString(m_dynBuff.buff, (int)m_position, (int)len);
                advPos(len);
            }

            return m_tmpStr;
        }

		public int readShort ()
        {
            m_tmpInt = 0;

            if (canRead(2))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpInt = System.BitConverter.ToInt32(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_shortByte, 0, 2);
                    Array.Reverse(m_intByte);
                    m_tmpInt = System.BitConverter.ToInt16(m_shortByte, 0);
                }

                advPos(2);
            }

            return m_tmpInt;
        }

		public uint readUnsignedByte ()
        {
            if (canRead(1))
            {
                advPos(1);
                return System.BitConverter.ToUInt32(m_dynBuff.buff, (int)(m_position - 1));
            }

            return 0;
        }

		public uint readUnsignedInt ()
        {
            m_tmpUint = 0;

            if (canRead(4))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpUint = System.BitConverter.ToUInt32(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_intByte, 0, 4);
                    Array.Reverse(m_intByte);
                    m_tmpUint = System.BitConverter.ToUInt32(m_intByte, 0);
                }

                advPos(4);
            }

            return m_tmpUint;
        }

		public ushort readUnsignedShort ()
        {
            m_tmpUshort = 0;

            if (canRead(2))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpUshort = System.BitConverter.ToUInt16(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_shortByte, 0, 2);
                    Array.Reverse(m_intByte);
                    m_tmpUshort = System.BitConverter.ToUInt16(m_shortByte, 0);
                }

                advPos(2);
            }

            return m_tmpUshort;
        }

        public ulong readUnsignedLong()
        {
            m_tmpUlong = 0;

            if (canRead(8))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpUlong = System.BitConverter.ToUInt64(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_shortByte, 0, 8);
                    Array.Reverse(m_intByte);
                    m_tmpUlong = System.BitConverter.ToUInt64(m_shortByte, 0);
                }

                advPos(8);
            }

            return m_tmpUlong;
        }

		public void writeByte (byte value)
        {
            if (!canWrite(1))
            {
                extendDeltaCapicity(1);
            }
            m_dynBuff.buff[m_position] = value;
            advPosAndLen(1);
        }

		public void writeInt (int value)
        {
            if (!canWrite(4))
            {
                extendDeltaCapicity(4);
            }

            m_intByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_intByte);
            }
            Array.Copy(m_intByte, 0, m_dynBuff.buff, m_position, 4);

            advPosAndLen(4);
        }

		public void writeMultiByte (string value, Encoding charSet, int len)
        {
            int num = 0;

            if (null != value)
            {
                char[] charPtr = value.ToCharArray();
                num = charSet.GetByteCount(charPtr);

                if (0 == len)
                {
                    len = num;
                }

                if (!canWrite((uint)len))
                {
                    extendDeltaCapicity((uint)len);
                }

                if (num <= len)
                {
                    Array.Copy(charSet.GetBytes(charPtr), 0, m_dynBuff.buff, m_position, num);
                }
                else
                {
                    Array.Copy(charSet.GetBytes(charPtr), 0, m_dynBuff.buff, m_position, len);
                }
                advPosAndLen((uint)len);
            }
            else
            {
                if (!canWrite((uint)len))
                {
                    extendDeltaCapicity((uint)len);
                }

                advPosAndLen((uint)len);
            }
        }

		public void writeShort (short value)
        {
            if (!canWrite(2))
            {
                extendDeltaCapicity(2);
            }

            m_shortByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_shortByte);
            }
            Array.Copy(m_shortByte, 0, m_dynBuff.buff, m_position, 2);

            advPosAndLen(2);
        }

		public void writeUnsignedInt (uint value)
        {
            if (!canWrite(4))
            {
                extendDeltaCapicity(4);
            }

            m_intByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_intByte);
            }
            Array.Copy(m_intByte, 0, m_dynBuff.buff, m_position, 4);

            advPosAndLen(4);
        }

        public void writeBytes(byte[] value, uint start, uint length)
        {
            if (!canWrite(length))
            {
                extendDeltaCapicity(length);
            }
            Array.Copy(value, start, m_dynBuff.buff, m_position, length);
            advPosAndLen(length);
        }

        public void writeUnsignedShort(ushort value)
        {
            if (!canWrite(2))
            {
                extendDeltaCapicity(2);
            }

            m_shortByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_shortByte);
            }
            Array.Copy(m_shortByte, 0, m_dynBuff.buff, m_position, 2);

            advPosAndLen(2);
        }

        public void writeUnsignedLong(ulong value)
        {
            if (!canWrite(8))
            {
                extendDeltaCapicity(8);
            }

            m_longByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_longByte);
            }
            Array.Copy(m_longByte, 0, m_dynBuff.buff, m_position, 8);

            advPosAndLen(8);
        }
    }
}