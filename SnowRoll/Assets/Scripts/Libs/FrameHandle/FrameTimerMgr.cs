using System.Collections.Generic;

/**
 * @brief 定时器管理器
 */
namespace SDK.Lib
{
    public class FrameTimerMgr : DelayHandleMgrBase
    {
        protected List<FrameTimerItem> mTimerLists;     // 当前所有的定时器列表

        public FrameTimerMgr()
        {
            this.mTimerLists = new List<FrameTimerItem>();
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            // 检查当前是否已经在队列中
            if (this.mTimerLists.IndexOf(delayObject as FrameTimerItem) == -1)
            {
                if (isInDepth())
                {
                    base.addObject(delayObject, priority);
                }
                else
                {
                    this.mTimerLists.Add(delayObject as FrameTimerItem);
                }
            }
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            // 检查当前是否在队列中
            if (this.mTimerLists.IndexOf(delayObject as FrameTimerItem) != -1)
            {
                (delayObject as FrameTimerItem).mDisposed = true;
                if (isInDepth())
                {
                    base.addObject(delayObject);
                }
                else
                {
                    foreach (FrameTimerItem item in this.mTimerLists)
                    {
                        if (UtilApi.isAddressEqual(item, delayObject))
                        {
                            this.mTimerLists.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        public void addFrameTimer(FrameTimerItem timer, float priority = 0.0f)
        {
            this.addObject(timer, priority);
        }

        public void removeFrameTimer(FrameTimerItem timer)
        {
            this.removeObject(timer);
        }

        public void Advance(float delta)
        {
            incDepth();

            foreach (FrameTimerItem timerItem in this.mTimerLists)
            {
                if (!timerItem.isClientDispose())
                {
                    timerItem.OnFrameTimer();
                }
                if (timerItem.mDisposed)
                {
                    removeObject(timerItem);
                }
            }

            decDepth();
        }
    }
}