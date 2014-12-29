using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 主循环
     */
    public class EngineLoop : IEngineLoop
    {
        public void MainLoop()
        {
            // 处理网络
            ByteArray ret = Ctx.m_instance.m_netMgr.getMsg() as ByteArray;
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
