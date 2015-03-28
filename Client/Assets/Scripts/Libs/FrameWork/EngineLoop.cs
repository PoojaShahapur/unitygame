using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 主循环
     */
    public class EngineLoop
    {
        public void MainLoop()
        {
            // 处理一些回调
            if(Ctx.m_instance.m_sysMsgRoute.m_bSocketOpened)
            {
                if (Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB != null)
                {
                    Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB();
                }

                Ctx.m_instance.m_sysMsgRoute.m_bSocketOpened = false;
            }

            // 处理网络
            ByteBuffer ret = Ctx.m_instance.m_netMgr.getMsg() as ByteBuffer;
            if (null != ret && null != Ctx.m_instance.m_netHandle && false == Ctx.m_instance.m_bStopNetHandle)
            {
                Ctx.m_instance.m_netHandle.handleMsg(ret);
            }
            // 处理 input
            //Ctx.m_instance.m_inputMgr.handleKeyBoard();
            // 游戏循环处理
            Ctx.m_instance.m_processSys.ProcessNextFrame();
            Ctx.m_instance.m_log.updateLog();
        }
    }
}
