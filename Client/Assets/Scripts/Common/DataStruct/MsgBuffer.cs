namespace SDK.Common
{
    public class MsgBuffer
    {
        protected CirculeBuffer m_circuleBuffer;    // 环形缓冲区

        // 当前获取的完整消息
        protected ByteBuffer m_headerBA;     // 主要是用来分析头的大小
        protected ByteBuffer m_msgBodyBA;    // 返回的字节数组
        protected ByteBuffer m_CryptoPadBU;           // 加密填充
        protected bool m_bMsgCompress = false;      // 当前消息是否压缩
        protected uint m_msgLen = 0;                // 消息的原始长度，没有标志位

        public MsgBuffer(uint initCapacity = DataCV.INIT_CAPACITY, uint maxCapacity = DataCV.MAX_CAPACITY)
        {
            m_circuleBuffer = new CirculeBuffer(initCapacity, maxCapacity);
            m_headerBA = new ByteBuffer(4);
            m_msgBodyBA = new ByteBuffer(initCapacity);
            m_CryptoPadBU = new ByteBuffer(1);
        }

        public ByteBuffer headerBA
        {
            get
            {
                return m_headerBA;
            }
        }

        public ByteBuffer msgBodyBA
        {
            get
            {
                return m_msgBodyBA;
            }
        }

        public CirculeBuffer circuleBuffer
        {
            get
            {
                return m_circuleBuffer;
            }
        }

        public bool bMsgCompress
        {
            get
            {
                return m_bMsgCompress;
            }
            set
            {
                m_bMsgCompress = value;
            }
        }

        /**
         * @brief 检查 CB 中是否有一个完整的消息
         */
        protected bool checkHasMsg()
        {
            bool ret = false;
            m_bMsgCompress = false;
            m_msgLen = 0;
            if (m_circuleBuffer.size > DataCV.HEADER_SIZE)         // 至少要是 DataCV.HEADER_SIZE 大小加 1 ，如果正好是 DataCV.HEADER_SIZE ，那只能说是只有大小字段，没有内容
            {
                m_circuleBuffer.frontBA(m_headerBA, DataCV.HEADER_SIZE);  // 如果不够整个消息的长度，还是不能去掉消息头的
                m_headerBA.readUnsignedInt32(ref m_msgLen);
                m_headerBA.setPos(0);      // 设置消息起始位置

#if MSG_COMPRESS
                if ((m_msgLen & DataCV.PACKET_ZIP) > 0)         // 如果有压缩标志
                {
                    m_msgLen &= (~DataCV.PACKET_ZIP);         // 去掉压缩标志位
                    m_bMsgCompress = true;
                }
#endif
                if (m_msgLen <= m_circuleBuffer.size - DataCV.HEADER_SIZE)          // 确实有一个消息
                {
                    if(!m_bMsgCompress) // 如果没有压缩，如果有压缩，后面要写入解压缩后的大小
                    {
                        m_headerBA.setPos(0);      // 设置消息起始位置
                    }
                    ret = true;
                }
            }

            return ret;
        }

        /**
         * @brief 获取前面的第一个完整的消息数据块
         */
        public bool popFront()
        {
            bool ret = false;
            if (checkHasMsg())      // 如果有完整的消息
            {
                m_circuleBuffer.popFrontLen(DataCV.HEADER_SIZE);
                if (m_msgLen > m_msgBodyBA.capacity)        // 如果查出已经分配的内存大小
                {
                    m_msgBodyBA.length = m_msgLen;
                }
                m_circuleBuffer.popFrontBA(m_msgBodyBA, m_msgLen);
#if MSG_COMPRESS
                if (m_bMsgCompress)
                {
                    m_msgBodyBA.uncompress();
                    m_msgLen = m_msgBodyBA.length;

                    m_headerBA.clear();
                    m_headerBA.writeUnsignedInt32(m_msgLen);      // 写入解压缩后正常的大小
                    m_headerBA.setPos(0);      // 设置消息起始位置
                }
#endif
                m_msgBodyBA.setPos(0);      // 设置消息起始位置
                ret = true;

                removeCryptoPad();      // 查看是否有加密填充内容需要移除

                if (m_circuleBuffer.empty())     // 如果已经清空，就直接重置
                {
                    m_circuleBuffer.clear();    // 读写指针从头开始，方式写入需要写入两部分
                }
            }

            return ret;
        }

        // 清理加密填充内容
        public void removeCryptoPad()
        {
            byte pad = 0;
            while(m_circuleBuffer.size > 0)
            {
                m_circuleBuffer.frontBA(m_CryptoPadBU, 1);
                m_CryptoPadBU.readInt8(ref pad);
                if(pad == 0)
                {
                    m_circuleBuffer.popFrontLen(1);
                }
                else        // 如果不是 0 ，就不再移除
                {
                    break;
                }
            }
        }
    }
}