namespace SDK.Lib
{
    /**
     * @brief 延迟优先级处理管理器
     */
    public class DelayNoPriorityHandleMgr : DelayNoPriorityHandleMgrBase
    {
        protected NoPriorityList mNoPriorityList;

        public DelayNoPriorityHandleMgr()
        {
            this.mNoPriorityList = new NoPriorityList();
            this.mNoPriorityList.setIsSpeedUpFind(true);
        }

        override public void init()
        {

        }

        override public void dispose()
        {
            this.mNoPriorityList.Clear();
        }

        override protected void addObject(IDelayHandleItem delayObject)
        {
            if (null != delayObject)
            {
                if (this.isInDepth())
                {
                    base.addObject(delayObject);
                }
                else
                {
                    this.mNoPriorityList.addNoPriorityObject(delayObject as INoOrPriorityObject, true);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("DelayNoPriorityHandleMgr::addObject, failed", LogTypeId.eLogNoPriorityListCheck);
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
                    this.mNoPriorityList.removeNoPriorityObject(delayObject as INoOrPriorityObject);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("DelayNoPriorityHandleMgr::removeObject, failed", LogTypeId.eLogNoPriorityListCheck);
                }
            }
        }

        public void addPriorityObject(INoOrPriorityObject priorityObject)
        {
            this.addObject(priorityObject as IDelayHandleItem);
        }

        public void removeNoPriorityObject(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
        }
    }
}