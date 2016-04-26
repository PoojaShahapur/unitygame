using System.Collections.Generic;

/**
 * @brief 心跳管理器
 */
namespace SDK.Lib
{
    public class TickMgr : DelayHandleMgrBase
    {
        protected List<TickProcessObject> m_tickLst;

        public TickMgr()
        {
            m_tickLst = new List<TickProcessObject>();
        }

        public void addTick(ITickedObject tickObj, float priority = 0.0f)
        {
            addObject(tickObj as IDelayHandleItem, priority);
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
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

                TickProcessObject processObject = new TickProcessObject();
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

        public void delTick(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            if (bInDepth())
            {
                base.removeObject(delayObject);
            }
            else
            {
                foreach (TickProcessObject item in m_tickLst)
                {
                    if (UtilApi.isAddressEqual(item.m_tickObject, delayObject))
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

            foreach (TickProcessObject tk in m_tickLst)
            {
                if (!(tk.m_tickObject as IDelayHandleItem).getClientDispose())
                {
                    (tk.m_tickObject as ITickedObject).onTick(delta);
                }
            }

            decDepth();
        }
    }
}