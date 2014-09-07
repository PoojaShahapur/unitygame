using System;
using System.Collections.Generic;

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

        protected DynamicBuffer m_dynBuff;         // 接收到的临时数据，将要放到 m_rawBuffer 中去
        protected ByteArray m_unCompressHeaderBA;  // 存放解压后的头的长度
        protected ByteArray m_sendData;            // 存放将要发送的数据，将要放到 m_sendBuffer 中去

        public DataBuffer()
        {
            m_rawBuffer = new CirculeBuffer();
            m_msgBuffer = new CirculeBuffer();
            m_sendBuffer = new CirculeBuffer();

            m_dynBuff = new DynamicBuffer();
            m_unCompressHeaderBA = new ByteArray();
            m_sendData = new ByteArray();
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
            while (m_rawBuffer.popFront(true))  // 如果有数据
            {
                m_rawBuffer.retBA.uncompress();
                m_unCompressHeaderBA.writeUnsignedInt(m_rawBuffer.retBA.length);
                m_unCompressHeaderBA.position = 0;
                m_msgBuffer.pushBackBA(m_unCompressHeaderBA);             // 保存消息大小字段
                m_msgBuffer.pushBackBA(m_rawBuffer.retBA);      // 保存消息大小字段
            }
        }

        public void send()
        {
            m_sendData.position = 0;
            m_sendBuffer.pushBackBA(m_sendData);
        }

        public ByteArray getMsg()
        {
            if(m_msgBuffer.popFront(true))
            {
                return m_msgBuffer.retBA;
            }

            return null;
        }
    }
}
