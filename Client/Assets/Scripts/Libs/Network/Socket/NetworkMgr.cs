using System;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    public class NetworkMgr
    {
        // 此处使用 Dictionary ，不适用 Hashable
        public Dictionary<string, NetTCPClient> m_id2SocketDic;
        protected NetThread m_netThread;
        protected NetTCPClient m_curSocket;
        public MMutex m_visitMutex = new MMutex(false, "NetMutex");

        // 函数区域
        public NetworkMgr()
        {
            m_id2SocketDic = new Dictionary<string, NetTCPClient>();
            #if NET_MULTHREAD
            startThread();
            #endif
        }

        /**
         *@brief 启动线程
         */
        public void startThread()
        {
            m_netThread = new NetThread(this);
            m_netThread.start();
        }

        /**
         *@brief 打开到 socket 的连接
         */
        public bool openSocket(string ip, int port)
        {
            string key = ip + "&" + port;
            if (!m_id2SocketDic.ContainsKey(key))
            {
                m_curSocket = new NetTCPClient(ip, port);
                m_curSocket.Connect(ip, port);
                using (MLock mlock = new MLock(m_visitMutex))
                {
                    m_id2SocketDic.Add(key, m_curSocket);
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
            if (m_id2SocketDic.ContainsKey(key))
            {
                // 关闭 socket 之前要等待所有的数据都发送完成
                m_id2SocketDic[key].msgSendEndEvent.Reset();        // 重置信号
                m_id2SocketDic[key].msgSendEndEvent.WaitOne();      // 阻塞等待数据全部发送完成

                using (MLock mlock = new MLock(m_visitMutex))
                {
                    m_id2SocketDic[key].Disconnect(0);
                    m_id2SocketDic.Remove(key);
                }
                m_curSocket = null;
            }
        }

        /**
         * @brief 关闭当前 socket
         */
        public void closeCurSocket()
        {
            if(m_curSocket != null)
            {
                string ip;
                int port;

                ip = m_curSocket.m_host;
                port = m_curSocket.m_port;

                string key = ip + "&" + port;
                if (m_id2SocketDic.ContainsKey(key))
                {
                    using (MLock mlock = new MLock(m_visitMutex))
                    {
                        m_id2SocketDic[key].Disconnect(0);
                        m_id2SocketDic.Remove(key);
                    }
                    m_curSocket = null;
                }
            }
        }

        public ByteBuffer getMsg()
        {
            if (m_curSocket != null)
            {
                return m_curSocket.dataBuffer.getMsg();
            }

            return null;
        }

        // 获取发送消息缓冲区
        public ByteBuffer getSendBA()
        {
            if (m_curSocket != null)
            {
                m_curSocket.dataBuffer.sendData.clear();
                return m_curSocket.dataBuffer.sendData;
            }

            return null;
        }

        // 注意这个仅仅是放入缓冲区冲，真正发送在子线程中发送
        public void send(bool bnet = true)
        {
            if (m_curSocket != null)
            {
                m_curSocket.dataBuffer.send(bnet);
                #if !NET_MULTHREAD
                m_curSocket.Send();
                #endif
            }
            else
            {
                Ctx.m_instance.m_log.log("current socket null");
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
            using (MLock mlock = new MLock(m_visitMutex))
            {
                // 从原始缓冲区取数据，然后放到解压和解密后的消息缓冲区中
                foreach (NetTCPClient socket in m_id2SocketDic.Values)
                {
                    if (!socket.brecvThreadStart && socket.isConnected)
                    {
                        socket.brecvThreadStart = true;
                        socket.Receive();
                    }

                    // 处理接收到的数据
                    //socket.dataBuffer.moveRaw2Msg();
                    // 处理发送数据
                    socket.Send();
                }
            }
        }

#if MSG_ENCRIPT
        public void setCryptKey(byte[] encrypt)
        {
            m_curSocket.dataBuffer.setCryptKey(encrypt);
        }
#endif
    }
}