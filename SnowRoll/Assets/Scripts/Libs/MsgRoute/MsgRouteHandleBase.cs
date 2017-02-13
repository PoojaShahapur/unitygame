using System.Collections.Generic;

namespace SDK.Lib
{
    public class MsgRouteHandleBase : GObject, ICalleeObject
    {
        public MDictionary<int, AddOnceEventDispatch> mId2HandleDic;

        public MsgRouteHandleBase()
        {
            this.mTypeId = "MsgRouteHandleBase";

            this.mId2HandleDic = new MDictionary<int, AddOnceEventDispatch>();
        }

        public void addMsgRouteHandle(MsgRouteID msgRouteID, MAction<IDispatchObject> handle)
        {
            if(!this.mId2HandleDic.ContainsKey((int)msgRouteID))
            {
                this.mId2HandleDic[(int)msgRouteID] = new AddOnceEventDispatch();
            }

            this.mId2HandleDic[(int)msgRouteID].addEventHandle(null, handle);
        }

        public void removeMsgRouteHandle(MsgRouteID msgRouteID, MAction<IDispatchObject> handle)
        {
            if (this.mId2HandleDic.ContainsKey((int)msgRouteID))
            {
                this.mId2HandleDic[(int)msgRouteID].removeEventHandle(null, handle);
            }
        }

        public virtual void handleMsg(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;

            if (this.mId2HandleDic.ContainsKey((int)msg.mMsgID))
            {
                this.mId2HandleDic[(int)msg.mMsgID].dispatchEvent(msg);
            }
            else
            {
                
            }
        }

        public void call(IDispatchObject dispObj)
        {

        }
    }
}