using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetCmdHandleBase
    {
        public Dictionary<int, Action<ByteBuffer>> m_id2HandleDic = new Dictionary<int, Action<ByteBuffer>>();

        public virtual void handleMsg(ByteBuffer ba, byte byCmd, byte byParam)
        {
            if(m_id2HandleDic.ContainsKey(byParam))
            {
                m_id2HandleDic[byParam](ba);
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("消息没有处理: byCmd = {0},  byParam = {1}", byCmd, byParam));
            }
        }
    }
}