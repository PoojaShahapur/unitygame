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

        public void onTick(float delta)
        {
            this.Advance(delta);
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
                        Ctx.mInstance.mLogSys.log("TickObjectPriorityMgr::onExecAdvance, failed", LogTypeId.eLogCommon);
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