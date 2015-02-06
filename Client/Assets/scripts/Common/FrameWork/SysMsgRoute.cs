using System;

namespace SDK.Common
{
    /**
     * @brief 系统消息流程，整个系统的消息分发都走这里，仅限单线程
     */
    public class SysMsgRoute
    {
        // 主线程中调用这个函数
        public bool m_bSocketOpened;
        // socket 连接成功，暂时无参数
        public Action m_socketOpenedCB;
        // socket 断开连接
        public Action m_socketClosedCB;
    }
}