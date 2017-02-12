/**
 * @brief 心跳管理器
 */
namespace SDK.Lib
{
    public class TickMgr : DelayHandleMgrBase
    {
        protected MList<TickProcessObject> mTickList;

        public TickMgr()
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

        public void addTick(ITickedObject tickObj, float priority = 0.0f)
        {
            this.addObject(tickObj as IDelayHandleItem, priority);
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
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

                while(idx < elemLen)
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

        public void removeTick(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
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

        public void Advance(float delta)
        {
            this.incDepth();

            //foreach (TickProcessObject tk in this.mTickList.list())
            //{
            //    if (!(tk.mTickObject as IDelayHandleItem).isClientDispose())
            //    {
            //        (tk.mTickObject as ITickedObject).onTick(delta);
            //    }
            //}
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

                if (!(tickObject as IDelayHandleItem).isClientDispose())
                {
                    tickObject.onTick(delta);
                }

                ++idx;
            }
        }

        virtual protected void onPostAdvance(float delta)
        {

        }
    }
}