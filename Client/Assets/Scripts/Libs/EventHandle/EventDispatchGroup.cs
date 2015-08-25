using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class EventDispatchGroup
    {
        protected Dictionary<int, EventDispatch> m_groupID2DispatchDic = new Dictionary<int, EventDispatch>();
        protected bool m_bInLoop;       // 是否是在循环遍历中

        public EventDispatchGroup()
        {
            m_bInLoop = false;
        }

        // 添加分发器
        public void addEventDispatch(int groupID, EventDispatch disp)
        {
            if (!m_groupID2DispatchDic.ContainsKey(groupID))
            {
                m_groupID2DispatchDic[groupID] = disp;
            }
        }

        public void addEventHandle(int groupID, Action<IDispatchObject> handle)
        {
            m_groupID2DispatchDic[groupID].addEventHandle(handle);
        }

        public void removeEventHandle(int groupID, Action<IDispatchObject> handle)
        {
            if (m_groupID2DispatchDic.ContainsKey(groupID))
            {
                m_groupID2DispatchDic[groupID].removeEventHandle(handle);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Event Dispatch Group not exist");
            }
        }

        public void dispatchEvent(int groupID,  IDispatchObject dispatchObject)
        {
            m_bInLoop = true;
            if (m_groupID2DispatchDic.ContainsKey(groupID))
            {
                m_groupID2DispatchDic[groupID].dispatchEvent(dispatchObject);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Event Dispatch Group not exist");
            }
            m_bInLoop = false;
        }

        public void clearAllEventHandle()
        {
            if (!m_bInLoop)
            {
                foreach (EventDispatch dispatch in m_groupID2DispatchDic.Values)
                {
                    dispatch.clearEventHandle();
                }

                m_groupID2DispatchDic.Clear();
            }
            else
            {
                Ctx.m_instance.m_logSys.log("looping cannot delete element");
            }
        }

        public void clearGroupEventHandle(int groupID)
        {
            if (!m_bInLoop)
            {
                if (m_groupID2DispatchDic.ContainsKey(groupID))
                {
                    m_groupID2DispatchDic[groupID].clearEventHandle();
                    m_groupID2DispatchDic.Remove(groupID);
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("Event Dispatch Group not exist");
                }
            }
            else
            {
                Ctx.m_instance.m_logSys.log("looping cannot delete element");
            }
        }
    }
}