using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetCmdDispHandle : ICalleeObject
    {
        protected MDictionary<int, AddOnceEventDispatch> mId2HandleDic;

        public NetCmdDispHandle()
        {
            mId2HandleDic = new MDictionary<int, AddOnceEventDispatch>();
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }

        public void addParamHandle(int paramId, MAction<IDispatchObject> handle)
        {
            if(!mId2HandleDic.ContainsKey(paramId))
            {
                mId2HandleDic[paramId] = new AddOnceEventDispatch();   
            }
            else
            {
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
                
            }
        }
    }
}