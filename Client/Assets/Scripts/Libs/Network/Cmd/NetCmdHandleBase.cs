using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetCmdHandleBase : ICalleeObject
    {
        protected Dictionary<int, AddOnceEventDispatch> m_id2HandleDic;

        public NetCmdHandleBase()
        {
            m_id2HandleDic = new Dictionary<int, AddOnceEventDispatch>();
        }

        public void addParamHandle(int paramId, Action<IDispatchObject> handle)
        {
            if(!m_id2HandleDic.ContainsKey(paramId))
            {
                m_id2HandleDic[paramId] = new AddOnceEventDispatch();   
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Msg Id Already Register");
            }

            m_id2HandleDic[paramId].addEventHandle(null, handle);
        }

        public void removeParamHandle(int paramId, Action<IDispatchObject> handle)
        {
            if(m_id2HandleDic.ContainsKey(paramId))
            {
                m_id2HandleDic[paramId].removeEventHandle(null, handle);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("ParamId not Register");
            }
        }

        public void call(IDispatchObject dispObj)
        {

        }

        public virtual void handleMsg(ByteBuffer bu, byte byCmd, byte byParam)
        {
            if(m_id2HandleDic.ContainsKey(byParam))
            {
                m_id2HandleDic[byParam].dispatchEvent(bu);
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("消息没有处理: byCmd = {0},  byParam = {1}", byCmd, byParam));
            }
        }
    }
}