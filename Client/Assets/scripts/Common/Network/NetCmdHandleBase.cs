using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public class NetCmdHandleBase
    {
        public Dictionary<int, Action<IByteArray>> m_id2HandleDic = new Dictionary<int, Action<IByteArray>>();

        public virtual void handleMsg(IByteArray ba, byte byCmd, byte byParam)
        {
            if(m_id2HandleDic.ContainsKey(byParam))
            {
                m_id2HandleDic[byParam](ba);
            }
            else
            {
                Ctx.m_instance.m_log.log(string.Format("消息没有处理: byCmd = {0},  byParam = {1}", byCmd, byParam));
            }
        }
    }
}