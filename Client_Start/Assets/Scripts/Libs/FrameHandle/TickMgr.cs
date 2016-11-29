/**
 * @brief 心跳管理器
 */
namespace SDK.Lib
{
    public class TickMgr : DelayHandleMgrBase
    {
        protected MList<TickProcessObject> mTickLst;

        public TickMgr()
        {
            this.mTickLst = new MList<TickProcessObject>();
        }

        public void addTick(ITickedObject tickObj, float priority = 0.0f)
        {
            addObject(tickObj as IDelayHandleItem, priority);
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (bInDepth())
            {
                base.addObject(delayObject, priority);
            }
            else
            {
                int position = -1;
                int idx = 0;
                int elemLen = this.mTickLst.Count();
                while(idx < elemLen)
                {
                    if (this.mTickLst[idx] == null)
                        continue;

                    if (this.mTickLst[idx].mTickObject == delayObject)
                    {
                        return;
                    }

                    if (this.mTickLst[idx].mPriority < priority)
                    {
                        position = idx;
                        break;
                    }

                    idx += 1;
                }

                TickProcessObject processObject = new TickProcessObject();
                processObject.mTickObject = delayObject as ITickedObject;
                processObject.mPriority = priority;

                if (position < 0 || position >= this.mTickLst.Count())
                {
                    this.mTickLst.Add(processObject);
                }
                else
                {
                    this.mTickLst.Insert(position, processObject);
                }
            }
        }

        public void delTick(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            if (bInDepth())
            {
                base.removeObject(delayObject);
            }
            else
            {
                foreach (TickProcessObject item in this.mTickLst.list())
                {
                    if (UtilApi.isAddressEqual(item.mTickObject, delayObject))
                    {
                        this.mTickLst.Remove(item);
                        break;
                    }
                }
            }
        }

        public void Advance(float delta)
        {
            incDepth();

            foreach (TickProcessObject tk in this.mTickLst.list())
            {
                if (!(tk.mTickObject as IDelayHandleItem).isClientDispose())
                {
                    (tk.mTickObject as ITickedObject).onTick(delta);
                }
            }

            decDepth();
        }
    }
}