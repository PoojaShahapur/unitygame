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

        public TimerMgr()
        {
            
        }

        public override void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            // 检查当前是否已经在队列中
            if (m_timerLists.IndexOf(delayObject as TimerItemBase) == -1)
            {
                if (bInDepth())
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
            // 检查当前是否在队列中
            if (m_timerLists.IndexOf(delayObject as TimerItemBase) != -1)
            {
                (delayObject as TimerItemBase).m_disposed = true;
                if (bInDepth())
                {
                    base.delObject(delayObject);
                }
                else
                {
                    foreach (TimerItemBase item in m_timerLists)
                    {
                        if (UtilApi.isAddressEqual(item, delayObject))
                        {
                            m_timerLists.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        public void Advance(float delta)
        {
            incDepth();

            foreach (TimerItemBase timerItem in m_timerLists)
            {
                timerItem.OnTimer(delta);
                if (timerItem.m_disposed)        // 如果已经结束
                {
                    delObject(timerItem);
                }
            }

            decDepth();
        }
    }
}