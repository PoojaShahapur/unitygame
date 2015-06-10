using SDK.Common;
using System.Collections.Generic;

/**
 * @brief 定时器管理器
 */
namespace SDK.Lib
{
    public class FrameTimerMgr : DelayHandleMgrBase
    {
        protected List<FrameTimerItem> m_timerLists = new List<FrameTimerItem>();     // 当前所有的定时器列表
        protected List<FrameTimerItem> m_delLists = new List<FrameTimerItem>();       // 当前需要删除的定时器

        public FrameTimerMgr()
        {
            
        }

        public override void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            // 检查当前是否已经在队列中
            if (m_timerLists.IndexOf(delayObject as FrameTimerItem) == -1)
            {
                if (m_duringAdvance)
                {
                    base.addObject(delayObject, priority);
                }
                else
                {
                    m_timerLists.Add(delayObject as FrameTimerItem);
                }
            }
        }

        public override void delObject(IDelayHandleItem delayObject)
        {
            // 检查当前是否在队列中
            if (m_timerLists.IndexOf(delayObject as FrameTimerItem) != -1)
            {
                (delayObject as TimerItemBase).m_disposed = true;
                if (m_duringAdvance)
                {
                    base.addObject(delayObject);
                }
                else
                {
                    foreach (FrameTimerItem item in m_timerLists)
                    {
                        if (item == delayObject)
                        {
                            m_timerLists.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        public override void Advance(float delta)
        {
            base.Advance(delta);

            foreach (FrameTimerItem timerItem in m_timerLists)
            {
                timerItem.OnFrameTimer();
                if (timerItem.m_disposed)
                {
                    m_delLists.Add(timerItem);
                }
            }

            foreach (FrameTimerItem timerItem in m_delLists)
            {
                m_timerLists.Remove(timerItem);
            }

            m_delLists.Clear();

            m_duringAdvance = false;
        }
    }
}