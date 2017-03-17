namespace SDK.Lib
{
    // 每一帧执行的对象管理器
    public class TickObjectPriorityMgr : DelayPriorityHandleMgr, ITickedObject, IDelayHandleItem, INoOrPriorityObject
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

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        public void onTick(float delta, TickMode tickMode)
        {
            this.Advance(delta, tickMode);
        }

        public void Advance(float delta, TickMode tickMode)
        {
            this.incDepth();

            this.onPreAdvance(delta, tickMode);
            this.onExecAdvance(delta, tickMode);
            this.onPostAdvance(delta, tickMode);

            this.decDepth();
        }

        virtual protected void onPreAdvance(float delta, TickMode tickMode)
        {

        }

        virtual protected void onExecAdvance(float delta, TickMode tickMode)
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
                        tickObject.onTick(delta, tickMode);
                    }
                }
                else
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log("TickObjectPriorityMgr::onExecAdvance, failed", LogTypeId.eLogCommon);
                    }
                }

                ++idx;
            }
        }

        virtual protected void onPostAdvance(float delta, TickMode tickMode)
        {

        }
    }
}