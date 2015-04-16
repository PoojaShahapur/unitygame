using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    public class MsgRouteDispHandle
    {
        public Dictionary<int, MsgRouteHandleBase> m_id2DispDic = new Dictionary<int, MsgRouteHandleBase>();

        public virtual void handleMsg(MsgRouteBase msg)
        {
            if (m_id2DispDic.ContainsKey((int)msg.m_msgID))
            {
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eMsgRoute1, (int)LangItemID.eItem0);
                Ctx.m_instance.m_log.log(string.Format(Ctx.m_instance.m_shareData.m_retLangStr, (int)msg.m_msgID));
                m_id2DispDic[(int)msg.m_msgID].handleMsg(msg);
            }
            else
            {
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eMsgRoute1, (int)LangItemID.eItem1);
                Ctx.m_instance.m_log.log(string.Format(Ctx.m_instance.m_shareData.m_retLangStr, (int)msg.m_msgID));
            }
        }
    }
}