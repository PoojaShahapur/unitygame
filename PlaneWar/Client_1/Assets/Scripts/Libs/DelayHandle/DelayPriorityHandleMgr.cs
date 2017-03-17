namespace SDK.Lib
{
    /**
     * @brief 延迟优先级处理管理器
     */
    public class DelayPriorityHandleMgr : DelayHandleMgrBase
    {
        protected PriorityList mPriorityList;

        public DelayPriorityHandleMgr()
        {
            this.mPriorityList = new PriorityList();
            this.mPriorityList.setIsSpeedUpFind(true);
        }

        override public void init()
        {

        }

        override public void dispose()
        {
            this.mPriorityList.Clear();
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (this.isInDepth())
            {
                base.addObject(delayObject, priority);
            }
            else
            {
                this.mPriorityList.addPriorityObject(delayObject as IPriorityObject, priority, true);
            }
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            if (this.isInDepth())
            {
                base.removeObject(delayObject);
            }
            else
            {
                this.mPriorityList.removePriorityObject(delayObject as IPriorityObject);
            }
        }

        public void addPriorityObject(IPriorityObject priorityObject, float priority = 0.0f)
        {
            this.addObject(priorityObject as IDelayHandleItem, priority);
        }

        public void removePriorityObject(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
        }
    }
}