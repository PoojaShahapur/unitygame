using System;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    public class NetworkMgr : INetworkMgr
    {
        // 此处使用 Dictionary ，不适用 Hashable
        protected Dictionary<string, NetTCPClient> m_id2SocketDic;
        protected ThreadWrap m_threadWrap;
        protected bool m_quit;
        protected NetTCPClient m_curSocket;

        // 函数区域
        public NetworkMgr()
        {
            m_id2SocketDic = new Dictionary<string, NetTCPClient>();
        }

        /**
         *@brief 启动线程
         */
        public void startThread()
        {
            m_threadWrap = new ThreadWrap(threadIO, this);
        }

        /**
         *@brief 打开到 socket 的连接
         */
        public bool openSocket(string ip, int port)
        {
            string key = ip + "&" + port;
            if (!m_id2SocketDic.ContainsKey(key))
            {
                m_id2SocketDic.Add(key, new NetTCPClient(ip, port));
                m_id2SocketDic[key].Connect(ip, port);
                m_curSocket = m_id2SocketDic[key];
            }
            else
            {
                return false;
            }

            return true;
        }

        /**
         *brief 线程回调函数
         */
        public bool threadIO(Object param)
        {
            while (!m_quit)
            {
                // 从原始缓冲区取数据，然后放到解压和解密后的消息缓冲区中
                foreach (NetTCPClient socket in m_id2SocketDic.Values)
                {
                    socket.dataBuffer.moveRaw2Msg();
                    socket.Send();
                }
            }

            return true;
        }

        public IByteArray getMsg()
        {
            if (m_curSocket != null)
            {
                return m_curSocket.dataBuffer.getMsg();
            }

            return null;
        }
    }
}