using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetworkMgr
    {
        // 此处使用 Dictionary ，不适用 Hashable
        public Dictionary<string, NetTCPClient> mId2ClientDic;
        protected NetTCPClient mCurClient;
        protected NetThread mNetThread;
        public MMutex mVisitMutex;

        // 函数区域
        public NetworkMgr()
        {
            mVisitMutex = new MMutex(false, "NetMutex");
            mId2ClientDic = new Dictionary<string, NetTCPClient>();
            if (MacroDef.NET_MULTHREAD)
            {
                startThread();
            }
        }

        /**
         *@brief 启动线程
         */
        public void startThread()
        {
            mNetThread = new NetThread(this);
            mNetThread.start();
        }

        /**
         *@brief 打开到 socket 的连接
         */
        public bool openSocket(string ip, int port)
        {
            string key = ip + "&" + port;
            if (!mId2ClientDic.ContainsKey(key))
            {
                mCurClient = new NetTCPClient(ip, port);
                mCurClient.Connect(ip, port);
                using (MLock mlock = new MLock(mVisitMutex))
                {
                    mId2ClientDic.Add(key, mCurClient);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /**
         * @brief 关闭 socket
         */
        public void closeSocket(string ip, int port)
        {
            string key = ip + "&" + port;
            if (mId2ClientDic.ContainsKey(key))
            {
                // 关闭 socket 之前要等待所有的数据都发送完成，如果发送一直超时，可能就卡在这很长时间
                if (MacroDef.NET_MULTHREAD)
                {
                    mId2ClientDic[key].msgSendEndEvent.Reset();        // 重置信号
                    mId2ClientDic[key].msgSendEndEvent.WaitOne();      // 阻塞等待数据全部发送完成
                }

                using (MLock mlock = new MLock(mVisitMutex))
                {
                    mId2ClientDic[key].Disconnect(0);
                    mId2ClientDic.Remove(key);
                }
                mCurClient = null;
            }
        }

        /**
         * @brief 关闭当前 socket
         */
        public void closeCurSocket()
        {
            if(mCurClient != null)
            {
                string ip;
                int port;

                ip = mCurClient.mIp;
                port = mCurClient.mPort;

                string key = ip + "&" + port;

                // 关闭 socket 之前要等待所有的数据都发送完成
                //m_id2SocketDic[key].msgSendEndEvent.Reset();        // 重置信号
                //m_id2SocketDic[key].msgSendEndEvent.WaitOne();      // 阻塞等待数据全部发送完成

                if (mId2ClientDic.ContainsKey(key))
                {
                    using (MLock mlock = new MLock(mVisitMutex))
                    {
                        mId2ClientDic[key].Disconnect(0);
                        mId2ClientDic.Remove(key);
                    }
                    mCurClient = null;
                }
            }
        }

        // 获取消息队列中的消息
        public ByteBuffer getMsg()
        {
            if (mCurClient != null)
            {
                return mCurClient.clientBuffer.getMsg();
            }

            return null;
        }

        // KBEngine 引擎弹出消息
        public ByteBuffer getMsg_KBE()
        {
            if (mCurClient != null)
            {
                return mCurClient.clientBuffer.getMsg_KBE();
            }

            return null;
        }

        // 获取发送消息缓冲区
        public ByteBuffer getSendBA()
        {
            if (mCurClient != null)
            {
                mCurClient.clientBuffer.sendData.clear();
                return mCurClient.clientBuffer.sendData;
            }

            return null;
        }

        // 注意这个仅仅是放入缓冲区冲，真正发送在子线程中发送
        public void send(bool isSendToNet = true)
        {
            if (mCurClient != null)
            {
                mCurClient.clientBuffer.send(isSendToNet);
                if (!MacroDef.NET_MULTHREAD)
                {
                    mCurClient.Send();
                }
            }
            else
            {
                Ctx.mInstance.mLogSys.log("current socket null");
            }
        }

        // 关闭 App ，需要等待子线程结束
        public void quipApp()
        {
            closeCurSocket();
            if (MacroDef.NET_MULTHREAD)
            {
                mNetThread.ExitFlag = true;        // 设置退出标志
                mNetThread.join();                 // 等待线程结束
            }
        }

        public void sendAndRecData()
        {
            using (MLock mlock = new MLock(mVisitMutex))
            {
                // 从原始缓冲区取数据，然后放到解压和解密后的消息缓冲区中
                foreach (NetTCPClient client in mId2ClientDic.Values)
                {
                    if (!client.brecvThreadStart && client.isConnected)
                    {
                        client.brecvThreadStart = true;
                        client.Receive();
                    }

                    // 处理接收到的数据
                    //socket.dataBuffer.moveRaw2Msg();
                    // 处理发送数据
                    if (client.canSendNewData())        // 只有上一次发送的数据全部发送出去后，才能继续发送新的数据
                    {
                        client.Send();
                    }
                }
            }
        }

        public void setCryptKey(byte[] encrypt)
        {
            mCurClient.clientBuffer.setCryptKey(encrypt);
        }

        public bool isNetThread(int threadID)
        {
            if (MacroDef.NET_MULTHREAD)
            {
                return mNetThread.isCurThread(threadID);
            }
            else
            {
                return true;
            }
        }
    }
}