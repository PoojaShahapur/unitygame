using SDK.Common;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * @brief 心跳管理器
 */
namespace SDK.Lib
{
    public class TickMgr : DelayHandleMgrBase
    {
        protected List<ProcessObject> m_tickLst = new List<ProcessObject>();

        public TickMgr()
        {
            
        }

        public void addTick(ITickedObject tickObj, float priority = 0.0f)
        {
            addObject(tickObj as IDelayHandleItem, priority);
        }

        public override void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (bInDepth())
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

                    if (m_tickLst[i].m_tickObject == delayObject)
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
                processObject.m_tickObject = delayObject as ITickedObject;
                processObject.m_priority = priority;

                if (position < 0 || position >= m_tickLst.Count)
                {
                    m_tickLst.Add(processObject);
                }
                else
                {
                    m_tickLst.Insert(position, processObject);
                }
            }
        }

        public override void delObject(IDelayHandleItem delayObject)
        {
            if (bInDepth())
            {
                base.delObject(delayObject);
            }
            else
            {
                foreach(ProcessObject item in m_tickLst)
                {
                    if (item.m_tickObject == delayObject)
                    {
                        m_tickLst.Remove(item);
                        break;
                    }
                }
            }
        }

        public void Advance(float delta)
        {
            incDepth();

            foreach (ProcessObject tk in m_tickLst)
            {
                if (!(tk.m_tickObject as IDelayHandleItem).getClientDispose())
                {
                    (tk.m_tickObject as ITickedObject).OnTick(delta);
                }
            }

            decDepth();
        }
    }
}