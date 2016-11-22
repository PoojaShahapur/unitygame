using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetModuleDispHandle
    {
        protected Dictionary<int, AddOnceEventDispatch> m_id2DispDic;
        protected LuaCSBridgeNetDispHandle m_luaCSBridgeNetDispHandle;     // Lua 网络事件处理器

        public NetModuleDispHandle()
        {
            m_id2DispDic = new Dictionary<int, AddOnceEventDispatch>();
        }

        public void addCmdHandle(int cmdId, NetCmdDispHandle callee, MAction<IDispatchObject> handle)
        {
            if (!m_id2DispDic.ContainsKey(cmdId))
            {
                m_id2DispDic[cmdId] = new AddOnceEventDispatch();
            }

            m_id2DispDic[cmdId].addEventHandle(callee, handle);
        }

        public void removeCmdHandle(int cmdId, NetCmdDispHandle calleeObj = null)
        {
            if(!m_id2DispDic.ContainsKey(cmdId))
            {
                Ctx.mInstance.mLogSys.log("Cmd Handle Not Register");
            }

            m_id2DispDic[cmdId].removeEventHandle(calleeObj, null);
        }

        public virtual void handleMsg(CmdDispInfo cmdDispInfo)
        {
            if(m_id2DispDic.ContainsKey(cmdDispInfo.byCmd))
            {
                Ctx.mInstance.mLogSys.log(string.Format("处理消息: byCmd = {0},  byParam = {1}", cmdDispInfo.byCmd, cmdDispInfo.byParam));
                m_id2DispDic[cmdDispInfo.byCmd].dispatchEvent(cmdDispInfo);
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("消息没有处理: byCmd = {0},  byParam = {1}", cmdDispInfo.byCmd, cmdDispInfo.byParam));
            }

            if(m_luaCSBridgeNetDispHandle != null)
            {
                m_luaCSBridgeNetDispHandle.handleMsg(cmdDispInfo.bu, cmdDispInfo.byCmd, cmdDispInfo.byParam);
            }
        }
    }
}