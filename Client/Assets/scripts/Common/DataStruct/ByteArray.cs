using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDK.Common
{
    /**
     * @brief 每种类型占用的字节大小
     */
    public enum TypeBytes
    {
        eBOOL = 1,
        eBYTE = 1,
        eSHORT = 2,
        eINT = 4,
        eFLOAT = 4,
        eDOUBLE = 8,
        eLONG = 8,
    }

    /**
     *@brief ByteArray 功能
     */
    public class ByteArray : IByteArray
    {
        static public Endian m_sEndian = Endian.LITTLE_ENDIAN;     // 当前机器的编码

        public byte[] m_intByte = new byte[(int)TypeBytes.eINT];
        public byte[] m_shortByte = new byte[(int)TypeBytes.eSHORT];
        public byte[] m_longByte = new byte[(int)TypeBytes.eLONG];

        protected DynamicBuffer m_dynBuff;
        protected uint m_position;          // 当前可以读取的位置索引
        protected Endian m_endian;          // 大端小端

        protected string m_tmpStr;
        protected int m_tmpInt;
        protected uint m_tmpUint;
        protected ushort m_tmpUshort;
        protected short m_tmpShort;
        protected ulong m_tmpUlong;
        protected bool m_tmpBool;
        protected byte m_tmpByte;

        protected float m_tmpFloat;
        protected double m_tmpDouble;

        protected byte[] m_tmpBytes;

        public ByteArray(uint initSize = DynamicBuffer.INIT_CAPACITY)
        {
            m_endian = m_sEndian;
            m_dynBuff = new DynamicBuffer(initSize);
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

        public void setEndian(Endian end)
        {
            m_endian = end;
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

        public uint getPos()
        {
            return m_position;
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
            m_dynBuff.extendDeltaCapicity(delta);
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

        // 压缩
        public uint compress(uint len_ = 0, CompressionAlgorithm algorithm = CompressionAlgorithm.ZLIB)
        {
            len_ = (len_ == 0 ? length : len_);

            byte[] retByte = null;
            uint retSize = 0;
            Compress.CompressData(m_dynBuff.buff, position, len_, ref retByte, ref retSize, algorithm);

            replace(retByte, 0, retSize, position, len_);

            return retSize;
        }

        // 解压
        public uint uncompress(uint len_ = 0, CompressionAlgorithm algorithm = CompressionAlgorithm.ZLIB)
        {
            len_ = (len_ == 0 ? length : len_);

            byte[] retByte = null;
            uint retSize = 0;
            Compress.DecompressData(m_dynBuff.buff, position, length, ref retByte, ref retSize, algorithm);

            replace(retByte, 0, retSize, position, len_);

            return retSize;
        }

        // 加密，使用 des 对称数字加密算法，加密8字节补齐，可能会导致变长
        public uint encrypt(byte[] cryptKey, uint len_ = 0)
        {
            len_ = (len_ == 0 ? length : len_);

            byte[] retByte = null;
            // 只有 8 个字节的时候才加密
            uint leftCnt = len_ % 8;  // 剩余的数量
            if (len_ >= 8)
            {
                Crypt.DES_ECB_Symmetry_Encode_Byte(m_dynBuff.buff, position, len_ - leftCnt, ref retByte, cryptKey);
            }

            writeBytes(retByte, 0, (uint)retByte.Length);

            if(leftCnt > 0) // 如果还有剩余的字节没有加密，还需要增加长度
            {
                position += leftCnt;
            }

            return (uint)(retByte.Length + leftCnt);
        }

        // 解密
        public void decrypt(byte[] cryptKey, uint len_ = 0)
        {
            len_ = (len_ == 0 ? length : len_);

            byte[] retByte = null;
            uint leftCnt = len_ % 8;  // 剩余的数量
            if (len_ >= 8)
            {
                Crypt.DES_ECB_Symmetry_Decode_Byte(m_dynBuff.buff, position, len_ - leftCnt, ref retByte, cryptKey);
            }

            writeBytes(retByte, 0, (uint)retByte.Length);

            if (leftCnt > 0) // 如果还有剩余的字节没有加密，还需要增加长度
            {
                position += leftCnt;
            }
        }

		public bool readBoolean ()
        {
            m_tmpBool = false;

            if (canRead((int)TypeBytes.eBOOL))
            {
                m_tmpBool = System.BitConverter.ToBoolean(m_dynBuff.buff, (int)m_position);
                advPos((int)TypeBytes.eBOOL);
            }

            return m_tmpBool;
        }

		public byte readByte ()
        {
            m_tmpByte = 0;

            if (canRead((int)TypeBytes.eBYTE))
            {
                m_tmpByte = (byte)System.BitConverter.ToInt32(m_dynBuff.buff, (int)m_position);
                advPos((int)TypeBytes.eBYTE);
            }

            return m_tmpByte;
        }

        public byte readUnsignedByte()
        {
            m_tmpByte = 0;

            if (canRead((int)TypeBytes.eBYTE))
            {
                m_tmpByte = (byte)System.BitConverter.ToChar(m_dynBuff.buff, (int)m_position);
                advPos((int)TypeBytes.eBYTE);
            }

            return m_tmpByte;
        }

		public short readShort ()
        {
            m_tmpShort = 0;

            if (canRead((int)TypeBytes.eSHORT))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpShort = System.BitConverter.ToInt16(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_shortByte, 0, (int)TypeBytes.eSHORT);
                    Array.Reverse(m_shortByte);
                    m_tmpShort = System.BitConverter.ToInt16(m_shortByte, 0);
                }

                advPos((int)TypeBytes.eSHORT);
            }

            return m_tmpShort;
        }

        public ushort readUnsignedShort()
        {
            m_tmpUshort = 0;

            if (canRead((int)TypeBytes.eSHORT))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpUshort = System.BitConverter.ToUInt16(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_shortByte, 0, (int)TypeBytes.eSHORT);
                    Array.Reverse(m_shortByte);
                    m_tmpUshort = System.BitConverter.ToUInt16(m_shortByte, 0);
                }

                advPos((int)TypeBytes.eSHORT);
            }

            return m_tmpUshort;
        }

        public int readInt()
        {
            m_tmpInt = 0;
            if (canRead((int)TypeBytes.eINT))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpInt = System.BitConverter.ToInt32(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_intByte, 0, (int)TypeBytes.eINT);
                    Array.Reverse(m_intByte);
                    m_tmpInt = System.BitConverter.ToInt32(m_intByte, 0);
                }
                advPos((int)TypeBytes.eINT);
            }

            return m_tmpInt;
        }

        public float readFloat()
        {
            m_tmpFloat = 0;
            if (canRead((int)TypeBytes.eFLOAT))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpFloat = System.BitConverter.ToInt32(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_intByte, 0, (int)TypeBytes.eFLOAT);
                    Array.Reverse(m_intByte);
                    m_tmpFloat = System.BitConverter.ToInt32(m_intByte, 0);
                }
                advPos((int)TypeBytes.eFLOAT);
            }

            return m_tmpFloat;
        }

        public double readDouble()
        {
            m_tmpDouble = 0;
            if (canRead((int)TypeBytes.eDOUBLE))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpDouble = System.BitConverter.ToInt32(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_longByte, 0, (int)TypeBytes.eDOUBLE);
                    Array.Reverse(m_longByte);
                    m_tmpDouble = System.BitConverter.ToInt32(m_longByte, 0);
                }
                advPos((int)TypeBytes.eDOUBLE);
            }

            return m_tmpDouble;
        }

		public uint readUnsignedInt ()
        {
            m_tmpUint = 0;

            if (canRead((int)TypeBytes.eINT))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpUint = System.BitConverter.ToUInt32(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_intByte, 0, (int)TypeBytes.eINT);
                    Array.Reverse(m_intByte);
                    m_tmpUint = System.BitConverter.ToUInt32(m_intByte, 0);
                }

                advPos((int)TypeBytes.eINT);
            }

            return m_tmpUint;
        }

        public ulong readUnsignedLong()
        {
            m_tmpUlong = 0;

            if (canRead((int)TypeBytes.eLONG))
            {
                if (m_endian == m_sEndian)
                {
                    m_tmpUlong = System.BitConverter.ToUInt64(m_dynBuff.buff, (int)m_position);
                }
                else
                {
                    Array.Copy(m_dynBuff.buff, (int)m_position, m_longByte, 0, (int)TypeBytes.eLONG);
                    Array.Reverse(m_longByte);
                    m_tmpUlong = System.BitConverter.ToUInt64(m_longByte, 0);
                }

                advPos((int)TypeBytes.eLONG);
            }

            return m_tmpUlong;
        }

        public string readMultiByte(uint len, Encoding charSet)
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

        // 这个是字节读取，没有大小端的区别
        public byte[] readBytes(uint len)
        {
            m_tmpBytes = new byte[len];

            if (canRead(len))
            {
                Array.Copy(m_dynBuff.buff, (int)m_position, m_tmpBytes, 0, (int)len);
                advPos(len);
            }

            return m_tmpBytes;
        }

        public void readBytes(byte[] outBytes, uint len)
        {
            m_tmpBytes = outBytes;

            if (canRead(len))
            {
                Array.Copy(m_dynBuff.buff, (int)m_position, m_tmpBytes, 0, (int)len);
                advPos(len);
            }
        }

		public void writeByte (byte value)
        {
            if (!canWrite((int)TypeBytes.eBYTE))
            {
                extendDeltaCapicity((int)TypeBytes.eBYTE);
            }
            m_dynBuff.buff[m_position] = value;
            advPosAndLen((int)TypeBytes.eBYTE);
        }

		public void writeShort (short value)
        {
            if (!canWrite((int)TypeBytes.eSHORT))
            {
                extendDeltaCapicity((int)TypeBytes.eSHORT);
            }

            m_shortByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_shortByte);
            }
            Array.Copy(m_shortByte, 0, m_dynBuff.buff, m_position, (int)TypeBytes.eSHORT);

            advPosAndLen((int)TypeBytes.eSHORT);
        }

        public void writeUnsignedShort(ushort value)
        {
            if (!canWrite((int)TypeBytes.eSHORT))
            {
                extendDeltaCapicity((int)TypeBytes.eSHORT);
            }

            m_shortByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_shortByte);
            }
            Array.Copy(m_shortByte, 0, m_dynBuff.buff, m_position, (int)TypeBytes.eSHORT);

            advPosAndLen((int)TypeBytes.eSHORT);
        }

        public void writeInt(int value)
        {
            if (!canWrite((int)TypeBytes.eINT))
            {
                extendDeltaCapicity((int)TypeBytes.eINT);
            }

            m_intByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_intByte);
            }
            Array.Copy(m_intByte, 0, m_dynBuff.buff, m_position, (int)TypeBytes.eINT);

            advPosAndLen((int)TypeBytes.eINT);
        }

		public void writeUnsignedInt (uint value)
        {
            if (!canWrite((int)TypeBytes.eINT))
            {
                extendDeltaCapicity((int)TypeBytes.eINT);
            }

            m_intByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_intByte);
            }
            Array.Copy(m_intByte, 0, m_dynBuff.buff, m_position, (int)TypeBytes.eINT);

            advPosAndLen((int)TypeBytes.eINT);
        }

        public void writeUnsignedLong(ulong value)
        {
            if (!canWrite((int)TypeBytes.eLONG))
            {
                extendDeltaCapicity((int)TypeBytes.eLONG);
            }

            m_longByte = System.BitConverter.GetBytes(value);
            if (m_endian != m_sEndian)
            {
                Array.Reverse(m_longByte);
            }
            Array.Copy(m_longByte, 0, m_dynBuff.buff, m_position, (int)TypeBytes.eLONG);

            advPosAndLen((int)TypeBytes.eLONG);
        }

        // 写入字节
        public void writeBytes(byte[] value, uint start, uint len)
        {
            if (len > 0)            // 如果有长度才写入
            {
                if (!canWrite(len))
                {
                    extendDeltaCapicity(len);
                }
                Array.Copy(value, start, m_dynBuff.buff, m_position, len);
                advPosAndLen(len);
            }
        }

        // 写入字符串
        public void writeMultiByte(string value, Encoding charSet, int len)
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

        // 替换已经有的一段数据
        protected void replace(byte[] srcBytes, uint srcStartPos = 0, uint srclen_ = 0, uint destStartPos = 0, uint destlen_ = 0)
        {
            uint curPos = position;     // 保存当前位置

            uint lastLeft = length - destStartPos - destlen_;        // 最后一段的长度
            length = destStartPos + srclen_ + lastLeft;      // 设置大小，保证足够大小空间

            position = destStartPos + srclen_;
            writeBytes(m_dynBuff.buff, destStartPos + destlen_, lastLeft);          // 这个地方自己区域覆盖自己区域，函数是不是能保证覆盖拷贝，经测试可以保证自己不覆盖自己区域

            position = destStartPos;
            writeBytes(srcBytes, srcStartPos, srclen_);

            length = destStartPos + srclen_ + lastLeft;      // 设置大小，保证足够大小空间
            position = curPos + srclen_;
        }

        public void insertUnsignedInt32(uint value)
        {
            length += sizeof(int);       // 扩大长度
            writeUnsignedInt(value);     // 写入
        }

        public ulong readUnsignedLongByOffset(uint offset)
        {
            position = offset;
            return readUnsignedLong();
        }
    }
}