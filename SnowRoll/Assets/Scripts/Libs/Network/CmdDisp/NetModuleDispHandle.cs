using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetModuleDispHandle
    {
        protected MDictionary<int, AddOnceEventDispatch> mId2DispDic;
        protected LuaCSBridgeNetDispHandle m_luaCSBridgeNetDispHandle;     // Lua 网络事件处理器

        public NetModuleDispHandle()
        {
            mId2DispDic = new MDictionary<int, AddOnceEventDispatch>();
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
            }

            mId2DispDic[cmdId].removeEventHandle(calleeObj, null);
        }

        public virtual void handleMsg(CmdDispInfo cmdDispInfo)
        {
            if(mId2DispDic.ContainsKey(cmdDispInfo.byCmd))
            {                
                mId2DispDic[cmdDispInfo.byCmd].dispatchEvent(cmdDispInfo);
            }
            else
            {
                
            }

            if(m_luaCSBridgeNetDispHandle != null)
            {
                m_luaCSBridgeNetDispHandle.handleMsg(cmdDispInfo.bu, cmdDispInfo.byCmd, cmdDispInfo.byParam);
            }
        }
    }
}