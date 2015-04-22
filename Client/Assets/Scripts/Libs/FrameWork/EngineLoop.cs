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
            // 处理客户端自己的消息机制
            MsgRouteBase routeMsg = null;
            while ((routeMsg = Ctx.m_instance.m_sysMsgRoute.pop()) != null)
            {
                Ctx.m_instance.m_msgRouteList.handleMsg(routeMsg);
            }

            // 处理网络
            ByteBuffer ret = null;
            while((ret = Ctx.m_instance.m_netMgr.getMsg()) != null)
            {
                if (null != Ctx.m_instance.m_netDispList && false == Ctx.m_instance.m_bStopNetHandle)
                {
                    Ctx.m_instance.m_netDispList.handleMsg(ret);
                }
            }

            // 处理 input
            //Ctx.m_instance.m_inputMgr.handleKeyBoard();
            // 游戏循环处理
            Ctx.m_instance.m_processSys.ProcessNextFrame();
            Ctx.m_instance.m_logSys.updateLog();
        }
    }
}
