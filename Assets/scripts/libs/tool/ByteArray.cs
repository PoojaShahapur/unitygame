using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    /**
     *@brief 类似 flash 的 ByteArray 功能
     */
    class ByteArray
    {
        static public Endian m_sEndian = Endian.NONE_ENDIAN;     // 当前机器的编码
        static public byte[] m_intByte = new byte[4];
        static public byte[] m_shortByte = new byte[2];

        protected byte[] m_buff;            // 当前的缓冲区
        protected uint m_length;              // 存储的数据的大小
        protected uint m_position;               // 当前可以读取的位置索引
        protected Endian m_endian;          // 大端小端

        public ByteArray()
        {
            m_endian = m_sEndian;
            m_buff = new byte[64 * 1024];       // 默认大小 64 K
        }

		public uint bytesAvailable
        {
            get
            {
                return (m_length - m_position);
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
                return m_length;
            }
            set
            {
                m_length = value;
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

        }

        public void advancePosition(uint num)
        {
            m_position += num;
        }

		//public function compress (algorithm:String="zlib") : void;

		public bool readBoolean ()
        {
            advancePosition(1);
            return System.BitConverter.ToBoolean(m_buff, (int)(m_position - 1));
        }

		public int readByte ()
        {
            advancePosition(1);
            return System.BitConverter.ToInt32(m_buff, (int)(m_position - 1));
        }

		public int readInt ()
        {
            advancePosition(4);
            if(m_endian == m_sEndian)
            {
                return System.BitConverter.ToInt32(m_buff, (int)(m_position - 4));
            }
            else
            {
                Array.Copy(m_buff, (int)(m_position - 4), m_intByte, 0, 4);
                Array.Reverse(m_intByte);
                return System.BitConverter.ToInt32(m_intByte, 0);
            }
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
            advancePosition(length);
            return charSet.GetString(m_buff, (int)m_position, (int)length);
        }

		public int readShort ()
        {
            advancePosition(2);
            if(m_endian == m_sEndian)
            {
                return System.BitConverter.ToInt32(m_buff, (int)(m_position - 2));
            }
            else
            {
                Array.Copy(m_buff, (int)(m_position - 2), m_shortByte, 0, 2);
                Array.Reverse(m_intByte);
                return System.BitConverter.ToInt16(m_shortByte, 0);
            }
        }

		public uint readUnsignedByte ()
        {
            advancePosition(1);
            return System.BitConverter.ToUInt32(m_buff, (int)(m_position - 1));
        }

		public uint readUnsignedInt ()
        {
            advancePosition(4);
            if(m_endian == m_sEndian)
            {
                return System.BitConverter.ToUInt32(m_buff, (int)(m_position - 4));
            }
            else
            {
                Array.Copy(m_buff, (int)(m_position - 4), m_intByte, 0, 4);
                Array.Reverse(m_intByte);
                return System.BitConverter.ToUInt32(m_intByte, 0);
            }
        }


		public uint readUnsignedShort ()
        {
            advancePosition(2);
            if(m_endian == m_sEndian)
            {
                return System.BitConverter.ToUInt32(m_buff, (int)(m_position - 2));
            }
            else
            {
                Array.Copy(m_buff, (int)(m_position - 2), m_shortByte, 0, 2);
                Array.Reverse(m_intByte);
                return System.BitConverter.ToUInt32(m_shortByte, 0);
            }
        }

		public void writeByte (byte value)
        {
            m_buff[m_position] = value;
            advancePosition(1);
        }

		public void writeInt (int value)
        {
            if (m_endian == m_sEndian)
            {
                m_intByte = System.BitConverter.GetBytes(value);
                Array.Copy(m_intByte, 0, m_buff, m_position, 4);
            }
            else
            {
                m_intByte = System.BitConverter.GetBytes(value);
                Array.Reverse(m_intByte);
                Array.Copy(m_intByte, 0, m_buff, m_position, 4);
            }
            advancePosition(4);
        }


		public void writeMultiByte (string value, Encoding charSet)
        {
            char[] charPtr = value.ToCharArray();
            int num = charSet.GetByteCount(charPtr);
            Array.Copy(charSet.GetBytes(charPtr), 0, m_buff, m_position, num);
            advancePosition((uint)num);
        }

		public void writeShort (short value)
        {
            if (m_endian == m_sEndian)
            {
                m_shortByte = System.BitConverter.GetBytes(value);
                Array.Copy(m_shortByte, 0, m_buff, m_position, 2);
            }
            else
            {
                m_shortByte = System.BitConverter.GetBytes(value);
                Array.Reverse(m_shortByte);
                Array.Copy(m_shortByte, 0, m_buff, m_position, 2);
            }
            advancePosition(2);
        }

		public void writeUnsignedInt (uint value)
        {
            if (m_endian == m_sEndian)
            {
                m_intByte = System.BitConverter.GetBytes(value);
                Array.Copy(m_intByte, 0, m_buff, m_position, 4);
            }
            else
            {
                m_intByte = System.BitConverter.GetBytes(value);
                Array.Reverse(m_intByte);
                Array.Copy(m_intByte, 0, m_buff, m_position, 4);
            }
            advancePosition(4);
        }

        public void writeBytes(byte[] value, uint start, uint length)
        {
            Array.Copy(value, start, m_buff, m_position, length);
            advancePosition(length);
        }
    }
}
