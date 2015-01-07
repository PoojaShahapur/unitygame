using SDK.Common;
using System.Threading;
namespace SDK.Lib
{
    /**
     * @brief 网络线程
     */
    public class NetThread : ThreadWrap
    {
        protected NetworkMgr m_networkMgr;

        public NetThread(NetworkMgr netMgr)
            : base(null, null)
        {
            m_networkMgr = netMgr;
        }

        /**
         *brief 线程回调函数
         */
        override public void threadHandle()
        {
            while (!m_ExitFlag)
            {
                m_networkMgr.m_visitMutex.WaitOne();
                // 从原始缓冲区取数据，然后放到解压和解密后的消息缓冲区中
                foreach (NetTCPClient socket in m_networkMgr.m_id2SocketDic.Values)
                {
                    if (!socket.brecvThreadStart && socket.isConnected)
                    {
                        socket.brecvThreadStart = true;
                        socket.Receive();
                    }

                    // 处理接收到的数据
                    socket.dataBuffer.moveRaw2Msg();
                    // 处理发送数据
                    socket.Send();
                }
                m_networkMgr.m_visitMutex.ReleaseMutex();

                Thread.Sleep(40);       // 24帧每秒
            }
        }
    }
}