using System;
using System.Collections.Generic;
using SDK.Common;


namespace SDK.Lib
{
    public class NetworkMgr : INetworkMgr
    {
        // 此处使用 Dictionary ，不适用 Hashable
        public Dictionary<string, NetTCPClient> m_id2SocketDic;
        protected NetThread m_netThread;
        protected NetTCPClient m_curSocket;
        public MutexWrap m_visitMutex = new MutexWrap(false, "NetMutex");

        // 函数区域
        public NetworkMgr()
        {
            m_id2SocketDic = new Dictionary<string, NetTCPClient>();
            #if NETMULTHREAD
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
                m_visitMutex.WaitOne();
                m_id2SocketDic.Add(key, m_curSocket);
                m_visitMutex.ReleaseMutex();
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
                m_visitMutex.WaitOne();
                m_id2SocketDic[key].Disconnect(0);
                m_id2SocketDic.Remove(key);
                m_visitMutex.ReleaseMutex();
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
                    m_visitMutex.WaitOne();
                    m_id2SocketDic[key].Disconnect(0);
                    m_id2SocketDic.Remove(key);
                    m_visitMutex.ReleaseMutex();
                    m_curSocket = null;
                }
            }
        }

        public IByteArray getMsg()
        {
            if (m_curSocket != null)
            {
                return m_curSocket.dataBuffer.getMsg();
            }

            return null;
        }

        // 获取发送消息缓冲区
        public IByteArray getSendBA()
        {
            if (m_curSocket != null)
            {
                m_curSocket.dataBuffer.sendData.clear();
                return m_curSocket.dataBuffer.sendData;
            }

            return null;
        }

        // 注意这个仅仅是放入缓冲区冲，真正发送在子线程中发送
        public void send()
        {
            m_curSocket.dataBuffer.send();
            #if !NETMULTHREAD
            m_curSocket.Send();
            #endif
        }

        // 关闭 App ，需要等待子线程结束
        public void quipApp()
        {
            closeCurSocket();
            #if NETMULTHREAD
            m_netThread.ExitFlag = true;        // 设置退出标志
            m_netThread.join();                 // 等待线程结束
            #endif
        }
    }
}