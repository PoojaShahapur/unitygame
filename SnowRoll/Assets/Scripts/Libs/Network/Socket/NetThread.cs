using System.Threading;

namespace SDK.Lib
{
    /**
     * @brief 网络线程
     */
    public class NetThread : MThread
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
            base.threadHandle();

            while (!mIsExitFlag)
            {
                m_networkMgr.sendAndRecData();
                Thread.Sleep(1000);       // 本来是想24帧每秒，但是还是改成 1 帧每秒，因为在 C++ 中测试24帧每秒仍然会阻塞主线程， 1帧每秒在 C++ 中也是正常的
            }
        }
    }
}