namespace SDK.Lib
{
    /**
     *@brief 网络数据缓冲区
     */
    public class ClientBuffer
    {
        protected MsgBuffer m_rawBuffer;      // 直接从服务器接收到的原始的数据，可能压缩和加密过
        protected MsgBuffer m_msgBuffer;      // 可以使用的缓冲区
        //protected ByteBuffer m_sendTmpBA;  // 发送临时缓冲区，发送的数据都暂时放在这里
        protected MsgBuffer m_sendTmpBuffer;  // 发送临时缓冲区，发送的数据都暂时放在这里
        protected ByteBuffer m_socketSendBA;       // 真正发送缓冲区

        protected DynBuffer<byte> m_dynBuff;         // 接收到的临时数据，将要放到 m_rawBuffer 中去
        protected ByteBuffer m_unCompressHeaderBA;  // 存放解压后的头的长度
        protected ByteBuffer m_sendData;            // 存放将要发送的数据，将要放到 m_sendBuffer 中去
        protected ByteBuffer m_tmpData;             // 临时需要转换的数据放在这里
        protected ByteBuffer m_tmp1fData;           // 临时需要转换的数据放在这里

        private MMutex m_readMutex;   // 读互斥
        private MMutex m_writeMutex;   // 写互斥

        protected CryptContext m_cryptContext;

        public ClientBuffer()
        {
            m_rawBuffer = new MsgBuffer();
            m_msgBuffer = new MsgBuffer();
            //m_sendTmpBA = new ByteBuffer();
            m_sendTmpBuffer = new MsgBuffer();
            m_socketSendBA = new ByteBuffer();
            //m_socketSendBA.mId = 1000;

            //m_dynBuff = new DynamicBuffer<byte>(8096);
            m_unCompressHeaderBA = new ByteBuffer();
            m_sendData = new ByteBuffer();
            m_tmpData = new ByteBuffer(4);
            m_tmp1fData = new ByteBuffer(4);

            m_readMutex = new MMutex(false, "ReadMutex");
            m_writeMutex = new MMutex(false, "WriteMutex");

            if (MacroDef.MSG_ENCRIPT)
            {
                m_cryptContext = new CryptContext();
            }
        }

        public DynBuffer<byte> dynBuff
        {
            get
            {
                return m_dynBuff;
            }
        }

        //public ByteBuffer sendTmpBA
        //{
        //    get
        //    {
        //        return m_sendTmpBA;
        //    }
        //}

        public MsgBuffer sendTmpBuffer
        {
            get
            {
                return m_sendTmpBuffer;
            }
        }

        public ByteBuffer sendBuffer
        {
            get
            {
                return m_socketSendBA;
            }
        }

        public ByteBuffer sendData
        {
            get
            {
                return m_sendData;
            }
        }

        // 设置 ClientBuffer 字节序
        public void setEndian(EEndian end)
        {
            m_rawBuffer.setEndian(end);
            m_msgBuffer.setEndian(end);

            m_sendTmpBuffer.setEndian(end);
            m_socketSendBA.setEndian(end);

            m_unCompressHeaderBA.setEndian(end);
            m_sendData.setEndian(end);
            m_tmpData.setEndian(end);
            m_tmp1fData.setEndian(end);
        }

        public void setCryptKey(byte[] encrypt)
        {
            //m_cryptContext.cryptAlgorithm = CryptAlgorithm.DES;
            m_cryptContext.m_cryptKey = encrypt;
            Dec.DES_set_key_unchecked(m_cryptContext.m_cryptKey, m_cryptContext.m_cryptKeyArr[(int)CryptAlgorithm.DES] as DES_key_schedule);
        }

        public void checkDES()
        {
            if (m_cryptContext.m_cryptKey != null && m_cryptContext.m_cryptAlgorithm != CryptAlgorithm.DES)
            {
                m_cryptContext.m_cryptAlgorithm = CryptAlgorithm.DES;
            }
        }

        public MsgBuffer rawBuffer
        {
            get
            {
                return m_rawBuffer;
            }
        }

        public void SetRevBufferSize(int size)
        {
            m_dynBuff = new DynBuffer<byte>((uint)size);
        }

        public void moveDyn2Raw()
        {
            Ctx.mInstance.mLogSys.log(string.Format("移动动态数据消息数据到原始数据队列，消息长度　{0}", m_dynBuff.size));
            UtilMsg.formatBytes2Array(m_dynBuff.buff, m_dynBuff.size);

            if (MacroDef.MSG_ENCRIPT)
            {
                checkDES();
            }
            // 接收到一个socket数据，就被认为是一个数据包，这个地方可能会有问题，服务器是这么发送的，只能这么处理，自己写入包的长度
            m_tmp1fData.clear();
            m_tmp1fData.writeUnsignedInt32(m_dynBuff.size);      // 填充长度
            m_rawBuffer.circularBuffer.pushBackBA(m_tmp1fData);
            // 写入包的数据
            m_rawBuffer.circularBuffer.pushBackArr(m_dynBuff.buff, 0, m_dynBuff.size);
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
            m_tmpData.clear();
            m_tmpData.writeUnsignedInt32(m_sendData.length);      // 填充长度

            if (bnet)       // 从 socket 发送出去
            {
                using (MLock mlock = new MLock(m_writeMutex))
                {
                    //m_sendTmpBA.writeUnsignedInt(m_sendData.length);                            // 写入头部长度
                    //m_sendTmpBA.writeBytes(m_sendData.dynBuff.buff, 0, m_sendData.length);      // 写入内容

                    m_sendTmpBuffer.circularBuffer.pushBackBA(m_tmpData);
                    m_sendTmpBuffer.circularBuffer.pushBackBA(m_sendData);
                }
            }
            else        // 直接放入接收消息缓冲区
            {
                //m_tmpData.clear();
                //m_tmpData.writeUnsignedInt(m_sendData.length);      // 填充长度

                m_msgBuffer.circularBuffer.pushBackBA(m_tmpData);              // 保存消息大小字段
                m_msgBuffer.circularBuffer.pushBackBA(m_sendData);             // 保存消息大小字段
            }
        }

        public ByteBuffer getMsg()
        {
            using (MLock mlock = new MLock(m_readMutex))
            {
                if (m_msgBuffer.popFront())
                {
                    return m_msgBuffer.msgBodyBA;
                }
            }

            return null;
        }

        // 获取数据，然后压缩加密
        public void getSocketSendData()
        {
            m_socketSendBA.clear();

            // 获取完数据，就解锁
            using (MLock mlock = new MLock(m_writeMutex))
            {
                //m_socketSendBA.writeBytes(m_sendTmpBA.dynBuff.buff, 0, (uint)m_sendTmpBA.length);
                //m_sendTmpBA.clear();
                // 一次全部取出来发送出去
                //m_socketSendBA.writeBytes(m_sendTmpBuffer.circularBuffer.buff, 0, (uint)m_sendTmpBuffer.circuleBuffer.size);
                //m_sendTmpBuffer.circularBuffer.clear();
                // 一次仅仅获取一个消息发送出去，因为每一个消息的长度要填写加密补位后的长度
                if (m_sendTmpBuffer.popFront())     // 弹出一个消息，如果只有一个消息，内部会重置变量
                {
                    m_socketSendBA.writeBytes(m_sendTmpBuffer.headerBA.dynBuff.buff, 0, m_sendTmpBuffer.headerBA.length);       // 写入头
                    m_socketSendBA.writeBytes(m_sendTmpBuffer.msgBodyBA.dynBuff.buff, 0, m_sendTmpBuffer.msgBodyBA.length);             // 写入消息体
                }
            }

            if (MacroDef.MSG_COMPRESS || MacroDef.MSG_ENCRIPT)
            {
                m_socketSendBA.setPos(0);
                CompressAndEncryptEveryOne();
                // CompressAndEncryptAllInOne();
            }
            m_socketSendBA.position = 0;        // 设置指针 pos
        }

        // 压缩加密每一个包
        protected void CompressAndEncryptEveryOne()
        {
            uint origMsgLen = 0;    // 原始的消息长度，后面判断头部是否添加压缩标志
            uint compressMsgLen = 0;
            uint cryptLen = 0;
            bool bHeaderChange = false; // 消息内容最前面的四个字节中消息的长度是否需要最后修正

            while (m_socketSendBA.bytesAvailable > 0)
            {
                if (MacroDef.MSG_COMPRESS && !MacroDef.MSG_ENCRIPT)
                {
                    bHeaderChange = false;
                }

                m_socketSendBA.readUnsignedInt32(ref origMsgLen);    // 读取一个消息包头

                if (MacroDef.MSG_COMPRESS)
                {
                    if (origMsgLen > MsgCV.PACKET_ZIP_MIN)
                    {
                        compressMsgLen = m_socketSendBA.compress(origMsgLen);
                    }
                    else
                    {
                        m_socketSendBA.incPosDelta((int)origMsgLen);
                        compressMsgLen = origMsgLen;
                    }
                }
                // 只加密消息 body
                //#if MSG_ENCRIPT
                //                m_socketSendBA.position -= compressMsgLen;      // 移动加密指针位置
                //                cryptLen = m_socketSendBA.encrypt(m_cryptKeyArr[(int)m_cryptAlgorithm], compressMsgLen, m_cryptAlgorithm);
                //                if (compressMsgLen != cryptLen)
                //                {
                //                    bHeaderChange = true;
                //                }
                //                compressMsgLen = cryptLen;
                //#endif

                // 加密如果系统补齐字节，长度可能会变成 8 字节的证书倍，因此需要等加密完成后再写入长度
                if (MacroDef.MSG_COMPRESS && !MacroDef.MSG_ENCRIPT)
                {
                    if (origMsgLen > MsgCV.PACKET_ZIP_MIN)    // 如果原始长度需要压缩
                    {
                        bHeaderChange = true;
                        origMsgLen = compressMsgLen;                // 压缩后的长度
                        origMsgLen |= MsgCV.PACKET_ZIP;            // 添加
                    }

                    if (bHeaderChange)
                    {
                        m_socketSendBA.decPosDelta((int)compressMsgLen + 4);        // 移动到头部位置
                        m_socketSendBA.writeUnsignedInt32(origMsgLen, false);     // 写入压缩或者加密后的消息长度
                        m_socketSendBA.incPosDelta((int)compressMsgLen);              // 移动到下一个位置
                    }
                }

                // 整个消息压缩后，包括 4 个字节头的长度，然后整个加密
                if (MacroDef.MSG_ENCRIPT)
                {
                    cryptLen = ((compressMsgLen + 4 + 7) / 8) * 8 - 4;      // 计算加密后，不包括 4 个头长度的 body 长度
                    if (origMsgLen > MsgCV.PACKET_ZIP_MIN)    // 如果原始长度需要压缩
                    {
                        origMsgLen = cryptLen;                // 压缩后的长度
                        origMsgLen |= MsgCV.PACKET_ZIP;            // 添加
                    }
                    else
                    {
                        origMsgLen = cryptLen;                // 压缩后的长度
                    }

                    m_socketSendBA.decPosDelta((int)(compressMsgLen + 4));        // 移动到头部位置
                    m_socketSendBA.writeUnsignedInt32(origMsgLen, false);     // 写入压缩或者加密后的消息长度

                    m_socketSendBA.decPosDelta(4);      // 移动到头部
                    m_socketSendBA.encrypt(m_cryptContext, 0);  // 加密
                }
            }

            // 整个消息压缩后，包括 4 个字节头的长度，然后整个加密
//#if MSG_ENCRIPT
            //m_socketSendBA.position = 0;      // 移动到头部
            //m_socketSendBA.encrypt(m_cryptKeyArr[(int)m_cryptAlgorithm], 0, m_cryptAlgorithm);
//#endif
        }

        // 压缩解密作为一个包
        protected void CompressAndEncryptAllInOne()
        {
            uint origMsgLen = m_socketSendBA.length;       // 原始的消息长度，后面判断头部是否添加压缩标志
            uint compressMsgLen = 0;

            if (origMsgLen > MsgCV.PACKET_ZIP_MIN && MacroDef.MSG_COMPRESS)
            {
                compressMsgLen = m_socketSendBA.compress();
            }
            else if (MacroDef.MSG_ENCRIPT)
            {
                compressMsgLen = origMsgLen;
                m_socketSendBA.incPosDelta((int)origMsgLen);
            }

            if (MacroDef.MSG_ENCRIPT)
            {
                m_socketSendBA.decPosDelta((int)compressMsgLen);
                compressMsgLen = m_socketSendBA.encrypt(m_cryptContext, 0);
            }

            if (MacroDef.MSG_COMPRESS || MacroDef.MSG_ENCRIPT)             // 如果压缩或者加密，需要再次添加压缩或者加密后的头长度
            {
                if (origMsgLen > MsgCV.PACKET_ZIP_MIN)    // 如果原始长度需要压缩
                {
                    origMsgLen = compressMsgLen;
                    origMsgLen |= MsgCV.PACKET_ZIP;            // 添加
                }
                else
                {
                    origMsgLen = compressMsgLen;
                }

                m_socketSendBA.position = 0;
                m_socketSendBA.insertUnsignedInt32(origMsgLen);            // 写入压缩或者加密后的消息长度
            }
        }

        // 消息格式
        // |------------- 加密的整个消息  -------------------------------------|
        // |----4 Header----|-压缩的 body----|----4 Header----|-压缩的 body----|
        // |                |                |                |                |
        protected void UnCompressAndDecryptEveryOne()
        {
            if (MacroDef.MSG_ENCRIPT)
            {
                m_rawBuffer.msgBodyBA.decrypt(m_cryptContext, 0);
            }
//#if MSG_COMPRESS
            //m_rawBuffer.headerBA.setPos(0); // 这个头目前没有用，是客户端自己添加的，服务器发送一个包，就认为是一个完整的包
            //m_rawBuffer.msgBodyBA.setPos(0);
            //uint msglen = m_rawBuffer.headerBA.readUnsignedInt();
            //if ((msglen & DataCV.PACKET_ZIP) > 0)
            //{
            //    m_rawBuffer.msgBodyBA.uncompress();
            //}
//#endif

            m_rawBuffer.msgBodyBA.setPos(0);

            uint msglen = 0;
            while (m_rawBuffer.msgBodyBA.bytesAvailable >= 4)
            {
                m_rawBuffer.msgBodyBA.readUnsignedInt32(ref msglen);    // 读取一个消息包头
                if (msglen == 0)     // 如果是 0 ，就说明最后是由于加密补齐的数据
                {
                    break;
                }
                
                if ((msglen & MsgCV.PACKET_ZIP) > 0 && MacroDef.MSG_COMPRESS)
                {
                    msglen &= (~MsgCV.PACKET_ZIP);         // 去掉压缩标志位
                    Ctx.mInstance.mLogSys.log(string.Format("消息需要解压缩，消息未解压长度　{0}", msglen));
                    msglen = m_rawBuffer.msgBodyBA.uncompress(msglen);
                    Ctx.mInstance.mLogSys.log(string.Format("消息需要解压缩，消息解压后长度　{0}", msglen));
                }
                else
                {
                    Ctx.mInstance.mLogSys.log(string.Format("消息不需要解压缩，消息原始长度　{0}", msglen));
                    m_rawBuffer.msgBodyBA.position += msglen;
                }

                m_unCompressHeaderBA.clear();
                m_unCompressHeaderBA.writeUnsignedInt32(msglen);        // 写入解压后的消息的长度，不要写入 msglen ，如果压缩，再加密，解密后，再解压后的长度才是真正的长度
                m_unCompressHeaderBA.position = 0;

                using (MLock mlock = new MLock(m_readMutex))
                {
                    m_msgBuffer.circularBuffer.pushBackBA(m_unCompressHeaderBA);             // 保存消息大小字段
                    m_msgBuffer.circularBuffer.pushBackArr(m_rawBuffer.msgBodyBA.dynBuff.buff, m_rawBuffer.msgBodyBA.position - msglen, msglen);      // 保存消息大小字段
                }

                Ctx.mInstance.mLogSys.log(string.Format("解压解密后消息起始索引 {0}, 消息长度　{1}, 消息 position 位置 {2}, 消息 size {3}", m_rawBuffer.msgBodyBA.position - msglen, msglen, m_rawBuffer.msgBodyBA.position, m_rawBuffer.msgBodyBA.length));
                Ctx.mInstance.mNetCmdNotify.addOneRevMsg();

                // Test 读取消息头
                // ByteBuffer buff = getMsg();
                // stNullUserCmd cmd = new stNullUserCmd();
                // cmd.derialize(buff);
                // Ctx.mInstance.mLogSys.log(string.Format("测试打印消息: byCmd = {0}, byParam = {1}", cmd.byCmd, cmd.byParam));
            }
        }

        protected void UnCompressAndDecryptAllInOne()
        {
            if (MacroDef.MSG_ENCRIPT)
            {
                m_rawBuffer.msgBodyBA.decrypt(m_cryptContext, 0);
            }

            uint msglen = 0;
            if (MacroDef.MSG_COMPRESS)
            {
                m_rawBuffer.headerBA.setPos(0);

                m_rawBuffer.headerBA.readUnsignedInt32(ref msglen);
                if ((msglen & MsgCV.PACKET_ZIP) > 0)
                {
                    m_rawBuffer.msgBodyBA.uncompress();
                }
            }

            if (!MacroDef.MSG_COMPRESS && !MacroDef.MSG_ENCRIPT)
            {
                m_unCompressHeaderBA.clear();
                m_unCompressHeaderBA.writeUnsignedInt32(m_rawBuffer.msgBodyBA.length);
                m_unCompressHeaderBA.position = 0;
            }

            using (MLock mlock = new MLock(m_readMutex))
            {
                if (!MacroDef.MSG_COMPRESS && !MacroDef.MSG_ENCRIPT)
                {
                    m_msgBuffer.circularBuffer.pushBackBA(m_unCompressHeaderBA);             // 保存消息大小字段
                }
                m_msgBuffer.circularBuffer.pushBackBA(m_rawBuffer.msgBodyBA);      // 保存消息大小字段
            }
        }
    }
}