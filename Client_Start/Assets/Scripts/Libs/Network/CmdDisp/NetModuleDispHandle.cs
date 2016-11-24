using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetModuleDispHandle
    {
        protected Dictionary<int, AddOnceEventDispatch> mId2DispDic;
        protected LuaCSBridgeNetDispHandle m_luaCSBridgeNetDispHandle;     // Lua 网络事件处理器

        public NetModuleDispHandle()
        {
            mId2DispDic = new Dictionary<int, AddOnceEventDispatch>();
        }

        public void addCmdHandle(int cmdId, NetCmdDispHandle callee, MAction<IDispatchObject> handle)
        {
            if (!mId2DispDic.ContainsKey(cmdId))
            {
                mId2DispDic[cmdId] = new AddOnceEventDispatch();
            }

            mId2DispDic[cmdId].addEventHandle(callee, handle);
        }

        public void removeCmdHandle(int cmdId, NetCmdDispHandle calleeObj = null)
        {
            if(!mId2DispDic.ContainsKey(cmdId))
            {
                Ctx.mInstance.mLogSys.log("Cmd Handle Not Register");
            }

            mId2DispDic[cmdId].removeEventHandle(calleeObj, null);
        }

        public virtual void handleMsg(CmdDispInfo cmdDispInfo)
        {
            if(mId2DispDic.ContainsKey(cmdDispInfo.byCmd))
            {
                Ctx.mInstance.mLogSys.log(string.Format("Handle msg: byCmd = {0},  byParam = {1}", cmdDispInfo.byCmd, cmdDispInfo.byParam));
                mId2DispDic[cmdDispInfo.byCmd].dispatchEvent(cmdDispInfo);
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("Msg not handle: byCmd = {0},  byParam = {1}", cmdDispInfo.byCmd, cmdDispInfo.byParam));
            }

            if(m_luaCSBridgeNetDispHandle != null)
            {
                m_luaCSBridgeNetDispHandle.handleMsg(cmdDispInfo.bu, cmdDispInfo.byCmd, cmdDispInfo.byParam);
            }
        }
    }
}