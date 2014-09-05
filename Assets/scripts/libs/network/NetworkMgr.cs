using System;
using System.Collections.Generic;
using San.Guo;

namespace San.Guo
{
    class NetworkMgr
    {
        // 此处使用 Dictionary ，不适用 Hashable
        protected Dictionary<string, NetworkSocket> m_id2SocketDic;
        protected ThreadWrap m_threadWrap;

        // 函数区域
        public NetworkMgr()
        {
            m_id2SocketDic = new Dictionary<string, NetworkSocket>();
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
        public bool openSocket(string ip, Int32 port)
        {
            string key = ip + "&" + port;
            if (!m_id2SocketDic.ContainsKey(key))
            {
                m_id2SocketDic.Add(key, new NetworkSocket(ip, port));
                m_id2SocketDic[key].connect();
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
            // 读取数据

            return true;
        }
    }
}