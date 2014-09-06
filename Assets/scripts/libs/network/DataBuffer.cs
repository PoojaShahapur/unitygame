using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    /**
     *@brief 网络数据缓冲区
     */
    public class DataBuffer
    {
        protected CirculeBuffer m_rawBuffer;      // 直接从服务器接收到的原始的数据，可能压缩和加密过
        protected CirculeBuffer m_msgBuffer;      // 可以使用的缓冲区
        protected CirculeBuffer m_sendBuffer;     // 发送缓冲区，压缩或者加密过的

        protected DynamicBuffer m_dynBuff;

        public DataBuffer()
        {
            m_rawBuffer = new CirculeBuffer();
            m_msgBuffer = new CirculeBuffer();
            m_sendBuffer = new CirculeBuffer();

            m_dynBuff = new DynamicBuffer();
        }

        public DynamicBuffer dynBuff
        {
            get
            {
                return m_dynBuff;
            }
        }

        public CirculeBuffer sendBuffer
        {
            get
            {
                return m_sendBuffer;
            }
        }

        public void moveDyn2Raw()
        {
            m_rawBuffer.pushBack(m_dynBuff.buff, 0, m_dynBuff.size);
        }

        public void moveRaw2Msg()
        {
            while (m_rawBuffer.popFront(true))
            {
                m_rawBuffer.headerBA.position = 0;
            }
        }
    }
}
