namespace SDK.Lib
{
    /**
     * @brief 主循环
     */
    public class EngineLoop
    {
        public void MainLoop()
        {
            // 处理 input
            //Ctx.mInstance.mInputMgr.handleKeyBoard();

            // 处理客户端自己的消息机制
            MsgRouteBase routeMsg = null;
            while ((routeMsg = Ctx.mInstance.mSysMsgRoute.pop()) != null)
            {
                Ctx.mInstance.mMsgRouteNotify.handleMsg(routeMsg);
            }

            // 处理网络
            if (!Ctx.mInstance.mNetCmdNotify.bStopNetHandle)
            {
                ByteBuffer ret = null;
                while ((ret = Ctx.mInstance.mNetMgr.getMsg()) != null)
                {
                    if (null != Ctx.mInstance.mNetCmdNotify)
                    {
                        Ctx.mInstance.mNetCmdNotify.addOneHandleMsg();
                        Ctx.mInstance.mNetCmdNotify.handleMsg(ret);       // CS 中处理
                        Ctx.mInstance.mLuaSystem.receiveToLuaRpc(ret);    // Lua 中处理
                    }
                }
            }

            // 游戏逻辑处理
            Ctx.mInstance.mProcessSys.ProcessNextFrame();
            //Ctx.mInstance.mLogSys.updateLog();
        }
    }
}
