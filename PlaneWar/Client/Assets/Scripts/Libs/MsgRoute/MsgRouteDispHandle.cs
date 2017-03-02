namespace SDK.Lib
{
    public class MsgRouteDispHandle
    {
        protected EventDispatchGroup mEventDispatchGroup;

        public MsgRouteDispHandle()
        {
            this.mEventDispatchGroup = new EventDispatchGroup();
        }

        public void addRouteHandle(int evtId, MsgRouteHandleBase pThis, MAction<IDispatchObject> handle)
        {
            this.mEventDispatchGroup.addEventHandle(evtId, pThis, handle);
        }

        public void removeRouteHandle(int evtId, MsgRouteHandleBase pThis, MAction<IDispatchObject> handle)
        {
            this.mEventDispatchGroup.removeEventHandle(evtId, pThis, handle);
        }

        public virtual void handleMsg(MsgRouteBase msg)
        {
            string textStr = "";

            if(this.mEventDispatchGroup.hasEventHandle((int)msg.mMsgType))
            {
                textStr = Ctx.mInstance.mLangMgr.getText(LangTypeId.eMsgRoute1, LangItemID.eItem2);
                this.mEventDispatchGroup.dispatchEvent((int)msg.mMsgType, msg);
            }
            else
            {
                textStr = Ctx.mInstance.mLangMgr.getText(LangTypeId.eMsgRoute1, LangItemID.eItem3);
            }
        }
    }
}