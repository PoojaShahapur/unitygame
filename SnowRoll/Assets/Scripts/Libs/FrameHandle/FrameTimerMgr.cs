/**
 * @brief 定时器管理器
 */
namespace SDK.Lib
{
    public class FrameTimerMgr : DelayHandleMgrBase
    {
        protected MList<FrameTimerItem> mTimerList;     // 当前所有的定时器列表

        public FrameTimerMgr()
        {
            this.mTimerList = new MList<FrameTimerItem>();
        }

        override public void init()
        {

        }

        override public void dispose()
        {

        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            // 检查当前是否已经在队列中
            if (!this.mTimerList.Contains(delayObject as FrameTimerItem))
            {
                if (this.isInDepth())
                {
                    base.addObject(delayObject, priority);
                }
                else
                {
                    this.mTimerList.Add(delayObject as FrameTimerItem);
                }
            }
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            // 检查当前是否在队列中
            if (this.mTimerList.Contains(delayObject as FrameTimerItem))
            {
                (delayObject as FrameTimerItem).mDisposed = true;
                if (isInDepth())
                {
                    base.addObject(delayObject);
                }
                else
                {
                    foreach (FrameTimerItem item in this.mTimerList.list())
                    {
                        if (UtilApi.isAddressEqual(item, delayObject))
                        {
                            this.mTimerList.Remove(item);
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

            foreach (FrameTimerItem timerItem in this.mTimerList.list())
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