using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetworkMgr
    {
        // 此处使用 Dictionary ，不适用 Hashable
        public Dictionary<string, NetTCPClient> m_id2ClientDic;
        protected NetTCPClient m_curClient;
#if NET_MULTHREAD
        protected NetThread m_netThread;
        public MMutex m_visitMutex;
#endif

        // 函数区域
        public NetworkMgr()
        {
            m_visitMutex = new MMutex(false, "NetMutex");
            m_id2ClientDic = new Dictionary<string, NetTCPClient>();
            #if NET_MULTHREAD
            startThread();
            #endif
        }

#if NET_MULTHREAD
        /**
         *@brief 启动线程
         */
        public void startThread()
        {
            m_netThread = new NetThread(this);
            m_netThread.start();
        }
#endif

        /**
         *@brief 打开到 socket 的连接
         */
        public bool openSocket(string ip, int port)
        {
            string key = ip + "&" + port;
            if (!m_id2ClientDic.ContainsKey(key))
            {
                m_curClient = new NetTCPClient(ip, port);
                m_curClient.Connect(ip, port);
                #if NET_MULTHREAD
                using (MLock mlock = new MLock(m_visitMutex))
                #endif
                {
                    m_id2ClientDic.Add(key, m_curClient);
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
            if (m_id2ClientDic.ContainsKey(key))
            {
                // 关闭 socket 之前要等待所有的数据都发送完成，如果发送一直超时，可能就卡在这很长时间
                #if NET_MULTHREAD
                m_id2ClientDic[key].msgSendEndEvent.Reset();        // 重置信号
                m_id2ClientDic[key].msgSendEndEvent.WaitOne();      // 阻塞等待数据全部发送完成
                #endif

                #if NET_MULTHREAD
                using (MLock mlock = new MLock(m_visitMutex))
                #endif
                {
                    m_id2ClientDic[key].Disconnect(0);
                    m_id2ClientDic.Remove(key);
                }
                m_curClient = null;
            }
        }

        /**
         * @brief 关闭当前 socket
         */
        public void closeCurSocket()
        {
            if(m_curClient != null)
            {
                string ip;
                int port;

                ip = m_curClient.m_ip;
                port = m_curClient.m_port;

                string key = ip + "&" + port;

                // 关闭 socket 之前要等待所有的数据都发送完成
                //m_id2SocketDic[key].msgSendEndEvent.Reset();        // 重置信号
                //m_id2SocketDic[key].msgSendEndEvent.WaitOne();      // 阻塞等待数据全部发送完成

                if (m_id2ClientDic.ContainsKey(key))
                {
                    #if NET_MULTHREAD
                    using (MLock mlock = new MLock(m_visitMutex))
                    #endif
                    {
                        m_id2ClientDic[key].Disconnect(0);
                        m_id2ClientDic.Remove(key);
                    }
                    m_curClient = null;
                }
            }
        }

        // 获取消息队列中的消息
        public ByteBuffer getMsg()
        {
            if (m_curClient != null)
            {
                return m_curClient.clientBuffer.getMsg();
            }

            return null;
        }

        // 获取发送消息缓冲区
        public ByteBuffer getSendBA()
        {
            if (m_curClient != null)
            {
                m_curClient.clientBuffer.sendData.clear();
                return m_curClient.clientBuffer.sendData;
            }

            return null;
        }

        // 注意这个仅仅是放入缓冲区冲，真正发送在子线程中发送
        public void send(bool bnet = true)
        {
            if (m_curClient != null)
            {
                m_curClient.clientBuffer.send(bnet);
                #if !NET_MULTHREAD
                m_curClient.Send();
                #endif
            }
            else
            {
                Ctx.m_instance.m_logSys.log("current socket null");
            }
        }

        // 关闭 App ，需要等待子线程结束
        public void quipApp()
        {
            closeCurSocket();
            #if NET_MULTHREAD
            m_netThread.ExitFlag = true;        // 设置退出标志
            m_netThread.join();                 // 等待线程结束
            #endif
        }

        public void sendAndRecData()
        {
            #if NET_MULTHREAD
            using (MLock mlock = new MLock(m_visitMutex))
            #endif
            {
                // 从原始缓冲区取数据，然后放到解压和解密后的消息缓冲区中
                foreach (NetTCPClient client in m_id2ClientDic.Values)
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

#if MSG_ENCRIPT
        public void setCryptKey(byte[] encrypt)
        {
            m_curClient.clientBuffer.setCryptKey(encrypt);
        }
#endif

        public bool isNetThread(int threadID)
        {
#if NET_MULTHREAD
            return m_netThread.isCurThread(threadID);
#else
            return true;
#endif
        }
    }
}