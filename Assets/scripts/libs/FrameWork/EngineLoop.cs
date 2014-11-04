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
            //ByteArray ret = Ctx.m_instance.m_netMgr.getMsg() as ByteArray;
            // 处理 input
            //Ctx.m_instance.m_inputMgr.handleKeyBoard();
            // 游戏循环处理
            Ctx.m_instance.m_ProcessSys.ProcessNextFrame();
        }
    }
}
