namespace SDK.Lib
{
    // 每一帧执行的对象管理器
    public class TickObjectPriorityMgr : DelayPriorityHandleMgr
    {
        public TickObjectPriorityMgr()
        {

        }

        override public void init()
        {
            base.init();
        }

        override public void dispose()
        {
            base.dispose();
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
                    this.mPriorityList.addPriorityObject(delayObject as IPriorityObject, priority);
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("TickMgr::addObject, failed", LogTypeId.eLogCommon);
                }
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

        public void Advance(float delta)
        {
            this.incDepth();

            this.onPreAdvance(delta);
            this.onExecAdvance(delta);
            this.onPostAdvance(delta);

            this.decDepth();
        }

        virtual protected void onPreAdvance(float delta)
        {

        }

        virtual protected void onExecAdvance(float delta)
        {
            int idx = 0;
            int count = this.mPriorityList.Count();
            ITickedObject tickObject = null;

            while (idx < count)
            {
                tickObject = this.mPriorityList.get(idx) as ITickedObject;

                if (null != (tickObject as IDelayHandleItem))
                {
                    if (!(tickObject as IDelayHandleItem).isClientDispose())
                    {
                        tickObject.onTick(delta);
                    }
                }
                else
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log("TickObjectMgrBase::onExecAdvance, failed", LogTypeId.eLogCommon);
                    }
                }

                ++idx;
            }
        }

        virtual protected void onPostAdvance(float delta)
        {

        }
    }
}