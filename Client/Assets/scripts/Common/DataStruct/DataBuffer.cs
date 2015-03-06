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
        protected ByteArray m_sendTmpBA;          // 发送临时缓冲区，发送的数据都暂时放在这里
        protected ByteArray m_encryptSendBA;      // 发送缓冲区，压缩或者加密过的
        protected ByteArray m_socketSendBA;      // 真正发送缓冲区

        protected DynamicBuffer m_dynBuff;         // 接收到的临时数据，将要放到 m_rawBuffer 中去
        protected ByteArray m_unCompressHeaderBA;  // 存放解压后的头的长度
        protected ByteArray m_sendData;            // 存放将要发送的数据，将要放到 m_sendBuffer 中去
        protected ByteArray m_tmpData;             // 临时需要转换的数据放在这里

        private MMutex m_readMutex = new MMutex(false, "ReadMutex");   // 读互斥
        private MMutex m_writeMutex = new MMutex(false, "WriteMutex");   // 写互斥

        protected bool m_bEncrypt = false;      // 当前是否需要加密解密

        protected ByteArray m_everyOneBodyBA;   // 压缩加密每一个包数据

        public DataBuffer()
        {
            m_rawBuffer = new CirculeBuffer();
            m_msgBuffer = new CirculeBuffer();
            m_sendTmpBA = new ByteArray();
            m_encryptSendBA = new ByteArray();
            m_socketSendBA = new ByteArray();

            m_dynBuff = new DynamicBuffer();
            m_unCompressHeaderBA = new ByteArray();
            m_sendData = new ByteArray();
            m_tmpData = new ByteArray();

            m_bEncrypt = true;
            m_everyOneBodyBA = new ByteArray();
        }

        public DynamicBuffer dynBuff
        {
            get
            {
                return m_dynBuff;
            }
        }

        public ByteArray sendTmpBA
        {
            get
            {
                return m_sendTmpBA;
            }
        }

        public ByteArray sendBuffer
        {
            get
            {
                return m_socketSendBA;
            }
        }

        public ByteArray sendData
        {
            get
            {
                return m_sendData;
            }
        }

        public bool bEncrypt
        {
            set
            {
                m_bEncrypt = value;
            }
        }

        public CirculeBuffer rawBuffer
        {
            get
            {
                return m_rawBuffer;
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
                //UnCompressAndDecryptAllInOne();
                UnCompressAndDecryptEveryOne();
            }
        }

        public void send(bool bnet = true)
        {
            if (bnet)       // 从 socket 发送出去
            {
                using (MLock mlock = new MLock(m_writeMutex))
                {
                    m_sendTmpBA.writeUnsignedInt(m_sendData.length);                            // 写入头部长度
                    m_sendTmpBA.writeBytes(m_sendData.dynBuff.buff, 0, m_sendData.length);      // 写入内容
                }
            }
            else        // 直接放入接收消息缓冲区
            {
                using (MLock mlock = new MLock(m_readMutex))
                {
                    m_tmpData.clear();
                    m_tmpData.writeUnsignedInt(m_sendData.length);      // 填充长度

                    m_msgBuffer.pushBackBA(m_tmpData);              // 保存消息大小字段
                    m_msgBuffer.pushBackBA(m_sendData);             // 保存消息大小字段
                }
            }
        }

        public ByteArray getMsg()
        {
            using (MLock mlock = new MLock(m_readMutex))
            {
                if (m_msgBuffer.popFront())
                {
                    return m_msgBuffer.retBA;
                }
            }

            return null;
        }

        // 获取数据，然后压缩加密
        public void getSendData()
        {
            m_encryptSendBA.clear();           // 清理之前的缓冲区
            m_socketSendBA.clear();

            // 这个操作不改变 m_sendTmpBA 内部数据，因此不用加锁
            m_encryptSendBA.writeBytes(m_sendTmpBA.dynBuff.buff, 0, (uint)m_sendTmpBA.length);

            // 获取完数据，就解锁
            using (MLock mlock = new MLock(m_writeMutex))
            {
                m_sendTmpBA.clear();
            }

            m_encryptSendBA.setPos(0);
            CompressAndEncryptEveryOne();
            // CompressAndEncryptAllInOne();
        }

        // 压缩加密每一个包
        protected void CompressAndEncryptEveryOne()
        {
            uint origMsgLen = 0;    // 原始的消息长度，后面判断头部是否添加压缩标志
            while(m_encryptSendBA.bytesAvailable > 0)
            {
                origMsgLen = m_encryptSendBA.readUnsignedInt();    // 读取一个消息包头
                m_everyOneBodyBA.length = origMsgLen;
                m_encryptSendBA.readBytes(m_everyOneBodyBA.dynBuff.buff, origMsgLen);

#if MSG_COMPRESS
                if (origMsgLen > DataCV.PACKET_ZIP_MIN)
                {
                    m_everyOneBodyBA.compress();
                }
#endif
#if MSG_ENCRIPT
                if (m_bEncrypt)
                {
                    m_everyOneBodyBA.encrypt();
                }
#endif

                if (origMsgLen > DataCV.PACKET_ZIP_MIN)    // 如果原始长度需要压缩
                {
                    origMsgLen = m_everyOneBodyBA.length;
                    origMsgLen |= DataCV.PACKET_ZIP;            // 添加
                }
                else
                {
                    origMsgLen = m_everyOneBodyBA.length;
                }

                m_socketSendBA.writeUnsignedInt(origMsgLen);            // 写入压缩或者加密后的消息长度

                m_socketSendBA.writeBytes(m_everyOneBodyBA.dynBuff.buff, 0, m_everyOneBodyBA.length); // 写入压缩或者加密后的消息内容
            }
        }

        // 压缩解密作为一个包
        protected void CompressAndEncryptAllInOne()
        {
#if MSG_COMPRESS
            uint origMsgLen = m_encryptSendBA.length;       // 原始的消息长度，后面判断头部是否添加压缩标志
            if (origMsgLen > DataCV.PACKET_ZIP_MIN)
            {
                m_encryptSendBA.compress();
            }
#endif
#if MSG_ENCRIPT
            if (m_bEncrypt)
            {
                m_encryptSendBA.encrypt();
            }
#endif

#if MSG_COMPRESS || MSG_ENCRIPT             // 如果压缩或者加密，需要再次添加压缩或者加密后的
            if (origMsgLen > DataCV.PACKET_ZIP_MIN)    // 如果原始长度需要压缩
            {
                origMsgLen = m_encryptSendBA.length;
                origMsgLen |= DataCV.PACKET_ZIP;            // 添加
            }
            else
            {
                origMsgLen = m_encryptSendBA.length;
            }

            m_socketSendBA.writeUnsignedInt(origMsgLen);            // 写入压缩或者加密后的消息长度
#endif
            m_socketSendBA.writeBytes(m_encryptSendBA.dynBuff.buff, 0, m_encryptSendBA.length); // 写入压缩或者加密后的消息内容
        }

        protected void UnCompressAndDecryptEveryOne()
        {
#if MSG_ENCRIPT
            if (m_bEncrypt)
            {
                m_rawBuffer.retBA.decrypt();
            }
#endif
#if MSG_COMPRESS
            m_rawBuffer.headerBA.setPos(0);
            uint msglen = m_rawBuffer.headerBA.readUnsignedInt();
            if ((msglen & DataCV.PACKET_ZIP) > 0)
            {
                m_rawBuffer.retBA.uncompress();
            }
#endif

            m_unCompressHeaderBA.clear();
            m_unCompressHeaderBA.writeUnsignedInt(m_rawBuffer.retBA.length);        // 写入解压后的消息的长度，不要写入 msglen ，如果压缩，再加密，解密后，再解压后的长度才是真正的长度
            m_unCompressHeaderBA.position = 0;

            using (MLock mlock = new MLock(m_readMutex))
            {
                m_msgBuffer.pushBackBA(m_unCompressHeaderBA);             // 保存消息大小字段
                m_msgBuffer.pushBackBA(m_rawBuffer.retBA);      // 保存消息大小字段
            }
        }

        protected void UnCompressAndDecryptAllInOne()
        {
#if MSG_ENCRIPT
            if (m_bEncrypt)
            {
                m_rawBuffer.retBA.decrypt();
            }
#endif
#if MSG_COMPRESS
            m_rawBuffer.headerBA.setPos(0);
            uint msglen = m_rawBuffer.headerBA.readUnsignedInt();
            if ((msglen & DataCV.PACKET_ZIP) > 0)
            {
                m_rawBuffer.retBA.uncompress();
            }
#endif

#if !MSG_COMPRESS && !MSG_ENCRIPT
            m_unCompressHeaderBA.clear();
            m_unCompressHeaderBA.writeUnsignedInt(m_rawBuffer.retBA.length);
            m_unCompressHeaderBA.position = 0;
#endif

            using (MLock mlock = new MLock(m_readMutex))
            {
#if !MSG_COMPRESS && !MSG_ENCRIPT
                m_msgBuffer.pushBackBA(m_unCompressHeaderBA);             // 保存消息大小字段
#endif
                m_msgBuffer.pushBackBA(m_rawBuffer.retBA);      // 保存消息大小字段
            }
        }
    }
}