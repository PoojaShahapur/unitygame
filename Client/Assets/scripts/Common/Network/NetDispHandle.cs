using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public class NetDispHandle
    {
        public Dictionary<int, NetCmdHandleBase> m_id2DispDic = new Dictionary<int, NetCmdHandleBase>();

        public virtual void handleMsg(IByteArray msg)
        {
            byte byCmd = msg.readByte();
            byte byParam = msg.readByte();
            msg.setPos(0);

            if(m_id2DispDic.ContainsKey(byCmd))
            {
                Ctx.m_instance.m_log.log(string.Format("处理消息: byCmd = {0},  byParam = {1}", byCmd, byParam));
                m_id2DispDic[byCmd].handleMsg(msg, byCmd, byParam);
            }
            else
            {
                Ctx.m_instance.m_log.log(string.Format("消息没有处理: byCmd = {0},  byParam = {1}", byCmd, byParam));
            }
        }
    }
}