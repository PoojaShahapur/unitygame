namespace SDK.Lib
{
    /**
     * @brief 当需要管理的对象可能在遍历中间添加的时候，需要这个管理器
     */
    public class DelayNoPriorityHandleMgrBase : DelayNoOrPriorityHandleMgrBase
    {
        public DelayNoPriorityHandleMgrBase()
        {
            this.mDeferredAddQueue = new NoPriorityList();
            this.mDeferredAddQueue.setIsSpeedUpFind(true);
            this.mDeferredDelQueue = new NoPriorityList();
            this.mDeferredDelQueue.setIsSpeedUpFind(true);
        }

        override public void init()
        {
            base.init();
        }

        override public void dispose()
        {
            base.dispose();
        }
    }
}