﻿using System.Collections.Generic;

namespace SDK.Lib
{
    public class MsgRouteHandleBase : GObject, ICalleeObject
    {
        public Dictionary<int, AddOnceEventDispatch> mId2HandleDic;

        public MsgRouteHandleBase()
        {
            this.mTypeId = "MsgRouteHandleBase";

            mId2HandleDic = new Dictionary<int, AddOnceEventDispatch>();
        }

        public void addMsgRouteHandle(MsgRouteID msgRouteID, MAction<IDispatchObject> handle)
        {
            if(!mId2HandleDic.ContainsKey((int)msgRouteID))
            {
                mId2HandleDic[(int)msgRouteID] = new AddOnceEventDispatch();
            }

            mId2HandleDic[(int)msgRouteID].addEventHandle(null, handle);
        }

        public void removeMsgRouteHandle(MsgRouteID msgRouteID, MAction<IDispatchObject> handle)
        {
            if (mId2HandleDic.ContainsKey((int)msgRouteID))
            {
                mId2HandleDic[(int)msgRouteID].removeEventHandle(null, handle);
            }
        }

        public virtual void handleMsg(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            if (mId2HandleDic.ContainsKey((int)msg.m_msgID))
            {
                mId2HandleDic[(int)msg.m_msgID].dispatchEvent(msg);
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