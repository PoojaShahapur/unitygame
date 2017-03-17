namespace SDK.Lib
{
    /**
     * @brief 延迟优先级处理管理器
     */
    public class DelayPriorityHandleMgr : DelayPriorityHandleMgrBase
    {
        protected PriorityList mPriorityList;

        public DelayPriorityHandleMgr()
        {
            this.mPriorityList = new PriorityList();
            this.mPriorityList.setIsSpeedUpFind(true);
            this.mPriorityList.setIsOpKeepSort(true);
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
            if (null != delayObject)
            {
                if (this.isInDepth())
                {
                    base.addObject(delayObject, priority);
                }
                else
                {
                    this.mPriorityList.addPriorityObject(delayObject as INoOrPriorityObject, priority);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("DelayPriorityHandleMgr::addObject, failed", LogTypeId.eLogCommon);
                }
            }
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            if (null != delayObject)
            {
                if (this.isInDepth())
                {
                    base.removeObject(delayObject);
                }
                else
                {
                    this.mPriorityList.removePriorityObject(delayObject as INoOrPriorityObject);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("DelayPriorityHandleMgr::removeObject, failed", LogTypeId.eLogCommon);
                }
            }
        }

        public void addPriorityObject(INoOrPriorityObject priorityObject, float priority = 0.0f)
        {
            this.addObject(priorityObject as IDelayHandleItem, priority);
        }

        public void removePriorityObject(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
        }
    }
}