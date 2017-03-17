namespace SDK.Lib
{
    /**
     * @brief 延迟优先级处理管理器
     */
    public class DelayNoOrPriorityHandleMgr : DelayNoOrPriorityHandleMgrBase
    {
        protected INoOrPriorityList mNoOrPriorityList;

        public DelayNoOrPriorityHandleMgr()
        {

        }

        override public void init()
        {
            base.init();
        }

        override public void dispose()
        {
            this.mNoOrPriorityList.Clear();
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
                    this.mNoOrPriorityList.addNoOrPriorityObject(delayObject as INoOrPriorityObject, priority);
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
                    this.mNoOrPriorityList.removeNoOrPriorityObject(delayObject as INoOrPriorityObject);
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

        public void addNoOrPriorityObject(INoOrPriorityObject priorityObject, float priority = 0.0f)
        {
            this.addObject(priorityObject as IDelayHandleItem, priority);
        }

        public void removeNoOrPriorityObject(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
        }
    }
}