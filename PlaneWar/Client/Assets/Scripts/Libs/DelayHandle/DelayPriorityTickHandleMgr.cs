namespace SDK.Lib
{
    /**
     * @brief 有心跳的延迟优先级处理管理器
     */
    public class DelayPriorityTickHandleMgr : DelayPriorityHandleMgr, ITickedObject
    {
        public void onTick(float delta)
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
            IPriorityObject priorityObject = null;

            while (idx < count)
            {
                priorityObject = this.mPriorityList.get(idx);

                if(null != (priorityObject as IDelayHandleItem))
                {
                    if (!(priorityObject as IDelayHandleItem).isClientDispose())
                    {
                        if(null != (priorityObject as ITickedObject))
                        {
                            (priorityObject as ITickedObject).onTick(delta);
                        }
                        else
                        {
                            if(MacroDef.ENABLE_LOG)
                            {
                                Ctx.mInstance.mLogSys.log("DelayPriorityTickHandleMgr::onExecAdvance, failed", LogTypeId.eLogCommon);
                            }
                        }
                    }
                }
                else
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log("DelayPriorityTickHandleMgr::onExecAdvance, failed", LogTypeId.eLogCommon);
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