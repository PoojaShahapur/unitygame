namespace SDK.Lib
{
    public class MsgBuffer
    {
        protected CircularBuffer mCircularBuffer;  // 环形缓冲区

        protected ByteBuffer mHeaderBA;            // 主要是用来分析头的大小
        protected ByteBuffer mMsgBodyBA;           // 返回的字节数组

        public MsgBuffer(uint initCapacity = BufferCV.INIT_CAPACITY, uint maxCapacity = BufferCV.MAX_CAPACITY)
        {
            mCircularBuffer = new CircularBuffer(initCapacity, maxCapacity);
            mHeaderBA = new ByteBuffer();
            mMsgBodyBA = new ByteBuffer();
        }

        public ByteBuffer headerBA
        {
            get
            {
                return mHeaderBA;
            }
        }

        public ByteBuffer msgBodyBA
        {
            get
            {
                return mMsgBodyBA;
            }
        }

        public CircularBuffer circularBuffer
        {
            get
            {
                return mCircularBuffer;
            }
        }

        // 设置网络字节序
        public void setEndian(EEndian end)
        {
            mHeaderBA.setEndian(end);
            mMsgBodyBA.setEndian(end);
        }

        /**
         * @brief 检查 CB 中是否有一个完整的消息
         */
        protected bool checkHasMsg()
        {
            mCircularBuffer.frontBA(mHeaderBA, MsgCV.HEADER_SIZE);  // 将数据读取到 mHeaderBA
            uint msglen = 0;
            mHeaderBA.readUnsignedInt32(ref msglen);
            if (MacroDef.MSG_COMPRESS)
            {
                if ((msglen & MsgCV.PACKET_ZIP) > 0)         // 如果有压缩标志
                {
                    msglen &= (~MsgCV.PACKET_ZIP);         // 去掉压缩标志位
                }
            }
            if (msglen <= mCircularBuffer.size - MsgCV.HEADER_SIZE)
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
            if (mCircularBuffer.size > MsgCV.HEADER_SIZE)         // 至少要是 DataCV.HEADER_SIZE 大小加 1 ，如果正好是 DataCV.HEADER_SIZE ，那只能说是只有大小字段，没有内容
            {
                mCircularBuffer.frontBA(mHeaderBA, MsgCV.HEADER_SIZE);  // 如果不够整个消息的长度，还是不能去掉消息头的
                uint msglen = 0;
                mHeaderBA.readUnsignedInt32(ref msglen);
                if (MacroDef.MSG_COMPRESS)
                {
                    if ((msglen & MsgCV.PACKET_ZIP) > 0)         // 如果有压缩标志
                    {
                        msglen &= (~MsgCV.PACKET_ZIP);         // 去掉压缩标志位
                    }
                }

                if (msglen <= mCircularBuffer.size - MsgCV.HEADER_SIZE)
                {
                    mCircularBuffer.popFrontLen(MsgCV.HEADER_SIZE);
                    mCircularBuffer.popFrontBA(mMsgBodyBA, msglen);
                    ret = true;
                }
            }

            if (mCircularBuffer.empty())     // 如果已经清空，就直接重置
            {
                mCircularBuffer.clear();    // 读写指针从头开始，方式写入需要写入两部分
            }

            return ret;
        }

        /**
         * @brief KBEngine 引擎消息处理
         */
        public bool popFrontAll()
        {
            bool ret = false;

            if (!mCircularBuffer.empty())
            {
                ret = true;
                mCircularBuffer.linearize();
                mCircularBuffer.popFrontBA(mMsgBodyBA, mCircularBuffer.size);
                mCircularBuffer.clear();
            }

            return ret;
        }
    }
}