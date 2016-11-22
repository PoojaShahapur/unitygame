using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetCmdDispHandle : ICalleeObject
    {
        protected Dictionary<int, AddOnceEventDispatch> m_id2HandleDic;

        public NetCmdDispHandle()
        {
            m_id2HandleDic = new Dictionary<int, AddOnceEventDispatch>();
        }

        public void addParamHandle(int paramId, MAction<IDispatchObject> handle)
        {
            if(!m_id2HandleDic.ContainsKey(paramId))
            {
                m_id2HandleDic[paramId] = new AddOnceEventDispatch();   
            }
            else
            {
                Ctx.mInstance.mLogSys.log("Msg Id Already Register");
            }

            m_id2HandleDic[paramId].addEventHandle(null, handle);
        }

        public void removeParamHandle(int paramId, MAction<IDispatchObject> handle)
        {
            if(m_id2HandleDic.ContainsKey(paramId))
            {
                m_id2HandleDic[paramId].removeEventHandle(null, handle);
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
            if(m_id2HandleDic.ContainsKey(cmd.byParam))
            {
                m_id2HandleDic[cmd.byParam].dispatchEvent(cmd.bu);
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("消息没有处理: byCmd = {0},  byParam = {1}", cmd.byCmd, cmd.byParam));
            }
        }
    }
}