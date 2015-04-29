namespace SDK.Common
{
    public class MsgBuffer
    {
        protected CirculeBuffer m_circuleBuffer;    // 环形缓冲区

        protected ByteBuffer m_headerBA;     // 主要是用来分析头的大小
        protected ByteBuffer m_msgBodyBA;        // 返回的字节数组

        public MsgBuffer(uint initCapacity = DataCV.INIT_CAPACITY, uint maxCapacity = DataCV.MAX_CAPACITY)
        {
            m_circuleBuffer = new CirculeBuffer(initCapacity, maxCapacity);
            m_headerBA = new ByteBuffer();
            m_msgBodyBA = new ByteBuffer();
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

        /**
         * @brief 检查 CB 中是否有一个完整的消息
         */
        protected bool checkHasMsg()
        {
            m_circuleBuffer.frontBA(m_headerBA, DataCV.HEADER_SIZE);  // 将数据读取到 m_headerBA
            uint msglen = 0;
            m_headerBA.readUnsignedInt32(ref msglen);
#if MSG_COMPRESS
            if ((msglen & DataCV.PACKET_ZIP) > 0)         // 如果有压缩标志
            {
                msglen &= (~DataCV.PACKET_ZIP);         // 去掉压缩标志位
            }
#endif
            if (msglen <= m_circuleBuffer.size - DataCV.HEADER_SIZE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * @brief 获取前面的第一个完整的消息数据块
         */
        public bool popFront()
        {
            bool ret = false;
            if (m_circuleBuffer.size > DataCV.HEADER_SIZE)         // 至少要是 DataCV.HEADER_SIZE 大小加 1 ，如果正好是 DataCV.HEADER_SIZE ，那只能说是只有大小字段，没有内容
            {
                m_circuleBuffer.frontBA(m_headerBA, DataCV.HEADER_SIZE);  // 如果不够整个消息的长度，还是不能去掉消息头的
                uint msglen = 0;
                m_headerBA.readUnsignedInt32(ref msglen);
#if MSG_COMPRESS
                if ((msglen & DataCV.PACKET_ZIP) > 0)         // 如果有压缩标志
                {
                    msglen &= (~DataCV.PACKET_ZIP);         // 去掉压缩标志位
                }
#endif

                if (msglen <= m_circuleBuffer.size - DataCV.HEADER_SIZE)
                {
                    m_circuleBuffer.popFrontLen(DataCV.HEADER_SIZE);
                    m_circuleBuffer.popFrontBA(m_msgBodyBA, msglen);
                    ret = true;
                }
            }

            if (m_circuleBuffer.empty())     // 如果已经清空，就直接重置
            {
                m_circuleBuffer.clear();    // 读写指针从头开始，方式写入需要写入两部分
            }

            return ret;
        }
    }
}