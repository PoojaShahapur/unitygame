using System.Collections.Generic;

namespace SDK.Lib
{
    public class MsgRouteDispHandle
    {
        protected EventDispatchGroup mEventDispatchGroup;

        public MsgRouteDispHandle()
        {
            mEventDispatchGroup = new EventDispatchGroup();
        }

        public void addRouteHandle(int evtId, MsgRouteHandleBase pThis, MAction<IDispatchObject> handle)
        {
            mEventDispatchGroup.addEventHandle(evtId, pThis, handle);
        }

        public void removeRouteHandle(int evtId, MsgRouteHandleBase pThis, MAction<IDispatchObject> handle)
        {
            mEventDispatchGroup.removeEventHandle(evtId, pThis, handle);
        }

        public virtual void handleMsg(MsgRouteBase msg)
        {
            string textStr = "";
            if(mEventDispatchGroup.hasEventHandle((int)msg.m_msgType))
            {
                textStr = Ctx.m_instance.m_langMgr.getText(LangTypeId.eMsgRoute1, LangItemID.eItem2);
                Ctx.m_instance.m_logSys.log(string.Format(textStr, (int)msg.m_msgType));
                mEventDispatchGroup.dispatchEvent((int)msg.m_msgType, msg);
            }
            else
            {
                textStr = Ctx.m_instance.m_langMgr.getText(LangTypeId.eMsgRoute1, LangItemID.eItem3);
                Ctx.m_instance.m_logSys.log(string.Format(textStr, (int)msg.m_msgID));
            }
        }
    }
}