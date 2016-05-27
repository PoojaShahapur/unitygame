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
                Ctx.m_instance.m_msgRouteNotify.handleMsg(routeMsg);
            }

            // 处理网络
            if (!Ctx.m_instance.m_netCmdNotify.bStopNetHandle)
            {
                ByteBuffer ret = null;
                while ((ret = Ctx.m_instance.m_netMgr.getMsg()) != null)
                {
                    if (null != Ctx.m_instance.m_netCmdNotify)
                    {
                        Ctx.m_instance.m_netCmdNotify.addOneHandleMsg();
                        Ctx.m_instance.m_netCmdNotify.handleMsg(ret);
                    }
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
