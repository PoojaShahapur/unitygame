using System.Collections.Generic;

namespace SDK.Lib
{
    public class MsgRouteHandleBase : GObject, ICalleeObject
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

        public virtual void handleMsg(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            if (m_id2HandleDic.ContainsKey((int)msg.m_msgID))
            {
                m_id2HandleDic[(int)msg.m_msgID].dispatchEvent(msg);
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format(Ctx.mInstance.mLangMgr.getText(LangTypeId.eMsgRoute1, LangItemID.eItem1), (int)msg.m_msgID));
            }
        }

        public void call(IDispatchObject dispObj)
        {

        }
    }
}