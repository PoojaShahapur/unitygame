using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class EventDispatchGroup
    {
        protected Dictionary<int, EventDispatch> m_groupID2DispatchDic = new Dictionary<int,EventDispatch>();

        public void addEventHandle(int groupID, Action<IDispatchObject> handle)
        {
            if (!m_groupID2DispatchDic.ContainsKey(groupID))
            {
                m_groupID2DispatchDic[groupID] = new EventDispatch();
            }

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
            if (m_groupID2DispatchDic.ContainsKey(groupID))
            {
                m_groupID2DispatchDic[groupID].dispatchEvent(dispatchObject);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Event Dispatch Group not exist");
            }
        }

        public void clearAllEventHandle()
        {
            foreach(EventDispatch dispatch in m_groupID2DispatchDic.Values)
            {
                dispatch.clearEventHandle();
            }

            m_groupID2DispatchDic.Clear();
        }

        public void clearGroupEventHandle(int groupID)
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
    }
}