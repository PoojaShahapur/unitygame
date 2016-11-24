using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetCmdDispHandle : ICalleeObject
    {
        protected Dictionary<int, AddOnceEventDispatch> mId2HandleDic;

        public NetCmdDispHandle()
        {
            mId2HandleDic = new Dictionary<int, AddOnceEventDispatch>();
        }

        public void addParamHandle(int paramId, MAction<IDispatchObject> handle)
        {
            if(!mId2HandleDic.ContainsKey(paramId))
            {
                mId2HandleDic[paramId] = new AddOnceEventDispatch();   
            }
            else
            {
                Ctx.mInstance.mLogSys.log("Msg Id Already Register");
            }

            mId2HandleDic[paramId].addEventHandle(null, handle);
        }

        public void removeParamHandle(int paramId, MAction<IDispatchObject> handle)
        {
            if(mId2HandleDic.ContainsKey(paramId))
            {
                mId2HandleDic[paramId].removeEventHandle(null, handle);
            }
            else
            {
                Ctx.mInstance.mLogSys.log("ParamId not Register");
            }
        }

        public void call(IDispatchObject dispObj)
        {

        }

        public virtual void handleMsg(CmdDispInfo cmd)
        {
            if(mId2HandleDic.ContainsKey(cmd.byParam))
            {
                mId2HandleDic[cmd.byParam].dispatchEvent(cmd.bu);
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("Msg not handle: byCmd = {0},  byParam = {1}", cmd.byCmd, cmd.byParam));
            }
        }
    }
}