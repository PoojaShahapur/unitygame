using SDK.Common;
using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SDK.Common
{
    /**
     *@brief 网络数据缓冲区
     */
    public class DataBuffer
    {
        protected CirculeBuffer m_rawBuffer;      // 直接从服务器接收到的原始的数据，可能压缩和加密过
        protected CirculeBuffer m_msgBuffer;      // 可以使用的缓冲区
        protected CirculeBuffer m_sendTmpBuffer;     // 发送临时缓冲区，发送的数据都暂时放在这里
        protected CirculeBuffer m_sendBuffer;     // 发送缓冲区，压缩或者加密过的

        protected DynamicBuffer m_dynBuff;         // 接收到的临时数据，将要放到 m_rawBuffer 中去
        protected ByteArray m_unCompressHeaderBA;  // 存放解压后的头的长度
        protected ByteArray m_sendData;            // 存放将要发送的数据，将要放到 m_sendBuffer 中去
        protected ByteArray m_tmpData;             // 临时需要转换的数据放在这里

        private MMutex m_readMutex = new MMutex(false, "ReadMutex");   // 读互斥
        private MMutex m_writeMutex = new MMutex(false, "WriteMutex");   // 写互斥

        public DataBuffer()
        {
            m_rawBuffer = new CirculeBuffer();
            m_msgBuffer = new CirculeBuffer();
            m_sendTmpBuffer = new CirculeBuffer();
            m_sendBuffer = new CirculeBuffer();

            m_dynBuff = new DynamicBuffer();
            m_unCompressHeaderBA = new ByteArray();
            m_sendData = new ByteArray();
            m_tmpData = new ByteArray();
        }

        public DynamicBuffer dynBuff
        {
            get
            {
                return m_dynBuff;
            }
        }

        public CirculeBuffer sendTmpBuffer
        {
            get
            {
                return m_sendTmpBuffer;
            }
        }

        public CirculeBuffer sendBuffer
        {
            get
            {
                return m_sendBuffer;
            }
        }

        public ByteArray sendData
        {
            get
            {
                return m_sendData;
            }
        }

        public void moveDyn2Raw()
        {
            m_rawBuffer.pushBackArr(m_dynBuff.buff, 0, m_dynBuff.size);
        }

        public void moveRaw2Msg()
        {
            while (m_rawBuffer.popFront())  // 如果有数据
            {
                m_rawBuffer.retBA.uncompress();
                m_unCompressHeaderBA.clear();
                m_unCompressHeaderBA.writeUnsignedInt(m_rawBuffer.retBA.length);
                m_unCompressHeaderBA.position = 0;

                MLock mlock = new MLock(m_readMutex);

                m_msgBuffer.pushBackBA(m_unCompressHeaderBA);             // 保存消息大小字段
                m_msgBuffer.pushBackBA(m_rawBuffer.retBA);      // 保存消息大小字段

                mlock.unlock();
            }

            //m_rawBuffer.clear();
        }

        public void send()
        {
            m_tmpData.clear();
            m_tmpData.writeUnsignedInt(m_sendData.length);      // 填充长度

            MLock mlock = new MLock(m_writeMutex);

            m_sendTmpBuffer.pushBackBA(m_tmpData);
            m_sendTmpBuffer.pushBackBA(m_sendData);

            mlock.unlock();
        }

        public ByteArray getMsg()
        {
            MLock mlock = new MLock(m_readMutex);

            if(m_msgBuffer.popFront())
            {
                mlock.unlock();

                return m_msgBuffer.retBA;
            }

            mlock.unlock();

            return null;
        }

        // 获取数据，然后压缩加密
        public void getSendData()
        {
            m_sendBuffer.clear();           // 清理之前的缓冲区

            MLock mlock = new MLock(m_writeMutex);

            m_sendBuffer.pushBackCB(m_sendTmpBuffer);
            m_sendTmpBuffer.clear();

            mlock.unlock();
        }
    }
}