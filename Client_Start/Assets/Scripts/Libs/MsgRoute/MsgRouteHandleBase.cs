using System.Collections.Generic;

namespace SDK.Lib
{
    public class MsgRouteHandleBase : GObject
    {
        public Dictionary<int, AddOnceEventDispatch> m_id2HandleDic;

        public MsgRouteHandleBase()
        {
            this.mTypeId = "MsgRouteHandleBase";

            m_id2HandleDic = new Dictionary<int, AddOnceEventDispatch>();
        }

        public void addMsgRouteHandle(MsgRouteID msgRouteID, MAction<IDispatchObject> handle)
        {
            if(!m_id2HandleDic.ContainsKey((int)msgRouteID))
            {
                m_id2HandleDic[(int)msgRouteID] = new AddOnceEventDispatch();
            }

            m_id2HandleDic[(int)msgRouteID].addEventHandle(null, handle);
        }

        public void removeMsgRouteHandle(MsgRouteID msgRouteID, MAction<IDispatchObject> handle)
        {
            if (m_id2HandleDic.ContainsKey((int)msgRouteID))
            {
                m_id2HandleDic[(int)msgRouteID].removeEventHandle(null, handle);
            }
        }

        public virtual void handleMsg(MsgRouteBase msg)
        {
            if (m_id2HandleDic.ContainsKey((int)msg.m_msgID))
            {
                m_id2HandleDic[(int)msg.m_msgID].dispatchEvent(msg);
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format(Ctx.m_instance.m_langMgr.getText(LangTypeId.eMsgRoute1, LangItemID.eItem1), (int)msg.m_msgID));
            }
        }
    }
}