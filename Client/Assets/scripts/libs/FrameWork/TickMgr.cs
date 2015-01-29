using SDK.Common;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * @brief 心跳管理器
 */
namespace SDK.Lib
{
    class TickMgr : DelayHandleMgrBase, ITickMgr
    {
        protected List<ProcessObject> m_tickLst = new List<ProcessObject>();

        public TickMgr()
        {
            
        }

        public override void addObject(IDelayHandleItem delayObject, float priority)
        {
            if (m_duringAdvance)
            {
                base.addObject(delayObject, priority);
            }
            else
            {
                int position = -1;
                for (int i = 0; i < m_tickLst.Count; i++)
                {
                    if (m_tickLst[i] == null)
                        continue;

                    if (m_tickLst[i].m_listener == delayObject)
                    {
                        return;
                    }

                    if (m_tickLst[i].m_priority < priority)
                    {
                        position = i;
                        break;
                    }
                }

                ProcessObject processObject = new ProcessObject();
                processObject.m_listener = delayObject;
                processObject.m_priority = priority;

                if (position < 0 || position >= m_tickLst.Count)
                    m_tickLst.Add(processObject);
                else
                    m_tickLst.Insert(position, processObject);
            }
        }

        public override void delObject(IDelayHandleItem delayObject)
        {
            if (m_duringAdvance)
            {
                base.addObject(delayObject);
            }
            else
            {
                foreach(ProcessObject item in m_tickLst)
                {
                    if(item.m_listener == delayObject)
                    {
                        m_tickLst.Remove(item);
                        break;
                    }
                }
            }
        }

        public override void Advance(float delta)
        {
            base.Advance(delta);
            foreach (ProcessObject tk in m_tickLst)
            {
                (tk.m_listener as ITickedObject).OnTick(delta);
            }
            m_duringAdvance = false;
        }
    }
}