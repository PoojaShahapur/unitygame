using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetDispHandle
    {
        public Dictionary<int, NetCmdHandleBase> m_id2DispDic = new Dictionary<int, NetCmdHandleBase>();
        public LuaCSBridgeNetDispHandle m_luaCSBridgeNetDispHandle;     // Lua 网络事件处理器

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
                m_id2DispDic[byCmd].handleMsg(msg, byCmd, byParam);
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