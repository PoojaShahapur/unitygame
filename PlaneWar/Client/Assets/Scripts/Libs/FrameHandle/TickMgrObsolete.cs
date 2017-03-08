namespace SDK.Lib
{
    /**
     * @brief 心跳管理器
     */
    public class TickMgrObsolete : DelayHandleMgrBase
    {
        protected MList<TickProcessObject> mTickList;

        public TickMgrObsolete()
        {
            this.mTickList = new MList<TickProcessObject>();
        }

        override public void init()
        {

        }

        override public void dispose()
        {
            this.mTickList.Clear();
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
                    int position = -1;
                    int idx = 0;
                    int elemLen = this.mTickList.Count();

                    while (idx < elemLen)
                    {
                        if (this.mTickList[idx] == null)
                        {
                            continue;
                        }

                        if (this.mTickList[idx].mTickObject == delayObject)
                        {
                            return;
                        }

                        if (this.mTickList[idx].mPriority < priority)
                        {
                            position = idx;
                            break;
                        }

                        idx += 1;
                    }

                    TickProcessObject processObject = new TickProcessObject();
                    processObject.mTickObject = delayObject as ITickedObject;
                    processObject.mPriority = priority;

                    if (position < 0 || position >= this.mTickList.Count())
                    {
                        this.mTickList.Add(processObject);
                    }
                    else
                    {
                        this.mTickList.Insert(position, processObject);
                    }
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
                foreach (TickProcessObject item in this.mTickList.list())
                {
                    if (UtilApi.isAddressEqual(item.mTickObject, delayObject))
                    {
                        this.mTickList.Remove(item);
                        break;
                    }
                }
            }
        }

        public void addTick(ITickedObject tickObj, float priority = 0.0f)
        {
            this.addObject(tickObj as IDelayHandleItem, priority);
        }

        public void removeTick(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
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
            int count = this.mTickList.Count();
            ITickedObject tickObject = null;

            while (idx < count)
            {
                tickObject = this.mTickList[idx].mTickObject;

                if(null != (tickObject as IDelayHandleItem))
                {
                    if (!(tickObject as IDelayHandleItem).isClientDispose())
                    {
                        tickObject.onTick(delta);
                    }
                }
                else
                {
                    if(MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log("TickMgr::onExecAdvance, failed", LogTypeId.eLogCommon);
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