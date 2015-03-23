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
        protected ByteArray m_socketSendBA;      // 真正发送缓冲区

        protected DynamicBuffer m_dynBuff;         // 接收到的临时数据，将要放到 m_rawBuffer 中去
        protected ByteArray m_unCompressHeaderBA;  // 存放解压后的头的长度
        protected ByteArray m_sendData;            // 存放将要发送的数据，将要放到 m_sendBuffer 中去
        protected ByteArray m_tmpData;             // 临时需要转换的数据放在这里

        private MMutex m_readMutex = new MMutex(false, "ReadMutex");   // 读互斥
        private MMutex m_writeMutex = new MMutex(false, "WriteMutex");   // 写互斥

#if MSG_ENCRIPT
        protected CryptAlgorithm m_cryptAlgorithm = CryptAlgorithm.RC5;      // 当前是否需要加密解密
        protected byte[] m_cryptKey;            // 秘钥

        protected CryptKeyBase[] m_cryptKeyArr = new CryptKeyBase[(int)CryptAlgorithm.eTotal];
#endif

        public DataBuffer()
        {
            m_rawBuffer = new CirculeBuffer();
            m_msgBuffer = new CirculeBuffer();
            m_sendTmpBA = new ByteArray();
            m_socketSendBA = new ByteArray();

            m_dynBuff = new DynamicBuffer();
            m_unCompressHeaderBA = new ByteArray();
            m_sendData = new ByteArray();
            m_tmpData = new ByteArray();

#if MSG_ENCRIPT
            m_cryptKeyArr[(int)CryptAlgorithm.RC5] = new RC5_32_KEY();
            m_cryptKeyArr[(int)CryptAlgorithm.DES] = new DES_key_schedule();
            RC5.RC5_32_set_key(m_cryptKeyArr[(int)CryptAlgorithm.RC5] as RC5_32_KEY, 16, Crypt.RC5_KEY, RC5.RC5_16_ROUNDS);     // 生成秘钥
#endif
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

#if MSG_ENCRIPT
        public CryptAlgorithm cryptAlgorithm
        {
            set
            {
                m_cryptAlgorithm = value;
            }
        }

        public void setCryptKey(byte[] encrypt)
        {
            cryptAlgorithm = CryptAlgorithm.DES;
            m_cryptKey = encrypt;
            Dec.DES_set_key_unchecked(m_cryptKey, m_cryptKeyArr[(int)CryptAlgorithm.DES] as DES_key_schedule);
        }
#endif

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
            m_socketSendBA.clear();

            // 这个操作不改变 m_sendTmpBA 内部数据，因此不用加锁
            m_socketSendBA.writeBytes(m_sendTmpBA.dynBuff.buff, 0, (uint)m_sendTmpBA.length);

            // 获取完数据，就解锁
            using (MLock mlock = new MLock(m_writeMutex))
            {
                m_sendTmpBA.clear();
            }

#if MSG_COMPRESS || MSG_ENCRIPT
            m_socketSendBA.setPos(0);
            CompressAndEncryptEveryOne();
            // CompressAndEncryptAllInOne();
#endif
            m_socketSendBA.position = 0;        // 设置指针 pos
        }

        // 压缩加密每一个包
        protected void CompressAndEncryptEveryOne()
        {
            uint origMsgLen = 0;    // 原始的消息长度，后面判断头部是否添加压缩标志
            uint compressMsgLen = 0;
#if MSG_ENCRIPT
            uint cryptLen = 0;
#endif
            bool bHeaderChange = false;
            uint totalLen = 0;
            while (m_socketSendBA.bytesAvailable > 0)
            {
                bHeaderChange = false;

                origMsgLen = m_socketSendBA.readUnsignedInt();    // 读取一个消息包头

#if MSG_COMPRESS
                if (origMsgLen > DataCV.PACKET_ZIP_MIN)
                {
                    compressMsgLen = m_socketSendBA.compress(origMsgLen);
                }
                else
                {
                    m_socketSendBA.position += origMsgLen;
                    compressMsgLen = origMsgLen;
                }
#endif
#if MSG_ENCRIPT
                m_socketSendBA.position -= compressMsgLen;      // 移动加密指针位置
                cryptLen = m_socketSendBA.encrypt(m_cryptKeyArr[(int)m_cryptAlgorithm], compressMsgLen, m_cryptAlgorithm);
                if (compressMsgLen != cryptLen)
                {
                    bHeaderChange = true;
                }
                compressMsgLen = cryptLen;
#endif

                // 加密如果系统补齐字节，长度可能会变成 8 字节的证书倍，因此需要等加密完成后再写入长度
#if MSG_COMPRESS
                if (origMsgLen > DataCV.PACKET_ZIP_MIN)    // 如果原始长度需要压缩
                {
                    bHeaderChange = true;
                    origMsgLen = compressMsgLen;                // 压缩后的长度
                    origMsgLen |= DataCV.PACKET_ZIP;            // 添加
                }
#endif
                if(bHeaderChange)
                {
                    totalLen = m_socketSendBA.length;                       // 保存长度
                    m_socketSendBA.position -= (compressMsgLen + 4);        // 移动到头部位置
                    m_socketSendBA.writeUnsignedInt(origMsgLen);            // 写入压缩或者加密后的消息长度
                    m_socketSendBA.position += compressMsgLen;              // 移动到下一个位置
                    m_socketSendBA.length = totalLen;                       // 回复长度
                }
            }
        }

        // 压缩解密作为一个包
        protected void CompressAndEncryptAllInOne()
        {
#if MSG_COMPRESS
            uint origMsgLen = m_socketSendBA.length;       // 原始的消息长度，后面判断头部是否添加压缩标志
            uint compressMsgLen = 0;
            if (origMsgLen > DataCV.PACKET_ZIP_MIN)
            {
                compressMsgLen = m_socketSendBA.compress();
            }
#endif

#if MSG_ENCRIPT
            else
            {
                compressMsgLen = origMsgLen;
                m_socketSendBA.position += origMsgLen;
            }

            m_socketSendBA.position -= compressMsgLen;
            compressMsgLen = m_socketSendBA.encrypt(m_cryptKeyArr[(int)m_cryptAlgorithm], 0, m_cryptAlgorithm);
#endif

#if MSG_COMPRESS || MSG_ENCRIPT             // 如果压缩或者加密，需要再次添加压缩或者加密后的头长度
            if (origMsgLen > DataCV.PACKET_ZIP_MIN)    // 如果原始长度需要压缩
            {
                origMsgLen = compressMsgLen;
                origMsgLen |= DataCV.PACKET_ZIP;            // 添加
            }
            else
            {
                origMsgLen = compressMsgLen;
            }

            m_socketSendBA.position = 0;
            m_socketSendBA.insertUnsignedInt32(origMsgLen);            // 写入压缩或者加密后的消息长度
#endif
        }

        protected void UnCompressAndDecryptEveryOne()
        {
#if MSG_ENCRIPT
            m_rawBuffer.retBA.decrypt(m_cryptKeyArr[(int)m_cryptAlgorithm], 0, m_cryptAlgorithm);
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
            m_rawBuffer.retBA.decrypt(m_cryptKeyArr[(int)m_cryptAlgorithm], 0, m_cryptAlgorithm);
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