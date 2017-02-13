namespace SDK.Lib
{
    /**
     * @brief 主循环
     */
    public class EngineLoop
    {
        public void MainLoop()
        {
            // 每一帧处理
            // 处理 input
            //Ctx.mInstance.mInputMgr.handleKeyBoard();

            // 处理客户端的各类消息
            // 处理客户端自己的消息机制
            MsgRouteBase routeMsg = null;
            while ((routeMsg = Ctx.mInstance.mSysMsgRoute.pop()) != null)
            {
                Ctx.mInstance.mMsgRouteNotify.handleMsg(routeMsg);
            }

            // 处理网络
            //if (!Ctx.mInstance.mNetCmdNotify.isStopNetHandle)
            //{
            //    ByteBuffer ret = null;
            //    while ((ret = Ctx.mInstance.mNetMgr.getMsg()) != null)
            //    {
            //        if (null != Ctx.mInstance.mNetCmdNotify)
            //        {
            //            Ctx.mInstance.mNetCmdNotify.addOneHandleMsg();
            //            Ctx.mInstance.mNetCmdNotify.handleMsg(ret);       // CS 中处理
            //            Ctx.mInstance.mLuaSystem.receiveToLuaRpc(ret);    // Lua 中处理
            //        }
            //    }
            //}

            // 填充数据到 KBEngine ，使用 KBEngine 引擎的逻辑解析
            //if (!Ctx.mInstance.mNetCmdNotify.isStopNetHandle)
            //{
            //    ByteBuffer ret = null;
            //    while ((ret = Ctx.mInstance.mNetMgr.getMsg_KBE()) != null)
            //    {
            //        Ctx.mInstance.mMKBEMainEntry.gameapp.pushBuffer(ret.dynBuffer.buffer, ret.dynBuffer.size);
            //    }
            //}

            // KBEngine 引擎逻辑处理
            //Ctx.mInstance.mMKBEMainEntry.FixedUpdate();

            // 每一帧的游戏逻辑处理
            Ctx.mInstance.mProcessSys.ProcessNextFrame();
            // 日志处理
            Ctx.mInstance.mLogSys.updateLog();
        }

        public void fixedUpdate()
        {
            Ctx.mInstance.mProcessSys.ProcessNextFixedFrame();
        }

        // 循环执行完成后，再次
        public void postUpdate()
        {
            Ctx.mInstance.mPlayerMgr.postUpdate();
        }
    }
}
