using SDK.Common;
using System.Collections.Generic;

/**
 * @brief 定时器管理器
 */
namespace SDK.Lib
{
    public class TimerMgr : DelayHandleMgrBase
    {
        protected List<TimerItemBase> m_timerLists = new List<TimerItemBase>();     // 当前所有的定时器列表
        protected List<TimerItemBase> m_delLists = new List<TimerItemBase>();       // 当前需要删除的定时器

        public TimerMgr()
        {
            
        }

        public override void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            // 检查当前是否已经在队列中
            if (m_timerLists.IndexOf(delayObject as TimerItemBase) == -1)
            {
                if (m_duringAdvance)
                {
                    base.addObject(delayObject, priority);
                }
                else
                {
                    m_timerLists.Add(delayObject as TimerItemBase);
                }
            }
        }

        public override void delObject(IDelayHandleItem delayObject)
        {
            (delayObject as TimerItemBase).m_disposed = true;
            if (m_duringAdvance)
            {
                base.addObject(delayObject);
            }
            else
            {
                foreach (TimerItemBase item in m_timerLists)
                {
                    if (item == delayObject)
                    {
                        m_timerLists.Remove(item);
                        break;
                    }
                }
            }
        }

        public override void Advance(float delta)
        {
            base.Advance(delta);

            foreach (TimerItemBase timerItem in m_timerLists)
            {
                if(timerItem.OnTimer(delta))        // 如果已经结束
                {
                    m_delLists.Add(timerItem);
                }
            }

            foreach (TimerItemBase timerItem in m_delLists)
            {
                m_timerLists.Remove(timerItem);
            }

            m_delLists.Clear();

            m_duringAdvance = false;
        }
    }
}