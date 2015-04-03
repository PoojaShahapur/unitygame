using SDK.Lib;
using System;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 系统消息流程，整个系统的消息分发都走这里，仅限单线程
     */
    public class SysMsgRoute
    {
        // 主线程中调用这个函数
        //public bool m_bSocketOpened;
        // socket 连接成功，暂时无参数
        //public Action m_socketOpenedCB;
        // socket 断开连接
        //public Action m_socketClosedCB;

        public MMutex m_visitMutex = new MMutex(false, "SysMsgRouteMutex");
        protected List<MsgRouteBase> m_msgList = new List<MsgRouteBase>();
        protected MsgRouteBase m_retMsg;

        public void pushMsg(MsgRouteBase msg)
        {
            using (MLock mlock = new MLock(m_visitMutex))
            {
                m_msgList.Add(msg);
            }
        }

        public MsgRouteBase popMsg()
        {
            m_retMsg = null;

            if(m_msgList.Count > 0)
            {
                using (MLock mlock = new MLock(m_visitMutex))
                {
                    m_retMsg = m_msgList[0];
                    m_msgList.RemoveAt(0);
                }
            }

            return m_retMsg;
        }
    }
}