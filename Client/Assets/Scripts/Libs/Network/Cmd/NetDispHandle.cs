using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetDispHandle
    {
        protected Dictionary<int, AddOnceEventDispatch> m_id2DispDic;
        protected CmdDispInfo mCmdDispInfo;
        protected LuaCSBridgeNetDispHandle m_luaCSBridgeNetDispHandle;     // Lua 网络事件处理器

        public NetDispHandle()
        {
            m_id2DispDic = new Dictionary<int, AddOnceEventDispatch>();
            mCmdDispInfo = new CmdDispInfo();
        }

        public void addCmdHandle(int cmdId, NetCmdHandleBase handle)
        {
            if (!m_id2DispDic.ContainsKey(cmdId))
            {
                m_id2DispDic[cmdId] = new AddOnceEventDispatch();
            }

            m_id2DispDic[cmdId].addEventHandle(handle, null);
        }

        public void removeCmdHandle(int cmdId, NetCmdHandleBase calleeObj = null)
        {
            if(!m_id2DispDic.ContainsKey(cmdId))
            {
                Ctx.m_instance.m_logSys.log("Cmd Handle Not Register");
            }

            m_id2DispDic[cmdId].removeEventHandle(calleeObj, null);
        }

        public virtual void handleMsg(ByteBuffer msg)
        {
            byte byCmd = 0;
            msg.readUnsignedInt8(ref byCmd);
            byte byParam = 0;
            msg.readUnsignedInt8(ref byParam);
            msg.setPos(0);

            if(m_id2DispDic.ContainsKey(byCmd))
            {
                Ctx.m_instance.m_logSys.log(string.Format("处理消息: byCmd = {0},  byParam = {1}", byCmd, byParam));
                mCmdDispInfo.bu = msg;
                mCmdDispInfo.byCmd = byCmd;
                mCmdDispInfo.byParam = byParam;
                m_id2DispDic[byCmd].dispatchEvent(mCmdDispInfo);
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("消息没有处理: byCmd = {0},  byParam = {1}", byCmd, byParam));
            }

            if(m_luaCSBridgeNetDispHandle != null)
            {
                m_luaCSBridgeNetDispHandle.handleMsg(msg, byCmd, byParam);
            }
        }
    }
}