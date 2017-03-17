﻿namespace SDK.Lib
{
    /**
     * @brief 当需要管理的对象可能在遍历中间添加的时候，需要这个管理器
     */
    public class DelayNoPriorityHandleMgrBase : GObject
    {
        protected NoPriorityList mDeferredAddQueue;
        protected NoPriorityList mDeferredDelQueue;

        protected LoopDepth mLoopDepth;           // 是否在循环中，支持多层嵌套，就是循环中再次调用循环

        public DelayNoPriorityHandleMgrBase()
        {
            this.mDeferredAddQueue = new NoPriorityList();
            this.mDeferredAddQueue.setIsSpeedUpFind(true);
            this.mDeferredDelQueue = new NoPriorityList();
            this.mDeferredDelQueue.setIsSpeedUpFind(true);

            this.mLoopDepth = new LoopDepth();
            this.mLoopDepth.setZeroHandle(this.processDelayObjects);
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }

        virtual protected void addObject(IDelayHandleItem delayObject)
        {
            if(this.mLoopDepth.isInDepth())
            {
                if (!this.mDeferredAddQueue.Contains(delayObject as INoOrPriorityObject))        // 如果添加列表中没有
                {
                    if (this.mDeferredDelQueue.Contains(delayObject as INoOrPriorityObject))     // 如果已经添加到删除列表中
                    {
                        this.mDeferredDelQueue.removeNoPriorityObject(delayObject as INoOrPriorityObject);
                    }
                    
                    this.mDeferredAddQueue.addNoPriorityObject(delayObject as INoOrPriorityObject);
                }
            }
        }

        virtual protected void removeObject(IDelayHandleItem delayObject)
        {
            if (this.mLoopDepth.isInDepth())
            {
                if (!this.mDeferredDelQueue.Contains(delayObject as INoOrPriorityObject))
                {
                    if (this.mDeferredAddQueue.Contains(delayObject as INoOrPriorityObject))    // 如果已经添加到删除列表中
                    {
                        this.mDeferredAddQueue.removeNoPriorityObject(delayObject as INoOrPriorityObject);
                    }

                    delayObject.setClientDispose(true);
                    
                    this.mDeferredDelQueue.removeNoPriorityObject(delayObject as INoOrPriorityObject);
                }
            }
        }

        private void processDelayObjects()
        {
            int idx = 0;
            // len 是 Python 的关键字
            int elemLen = 0;

            if (!this.mLoopDepth.isInDepth())       // 只有全部退出循环后，才能处理添加删除
            {
                if (this.mDeferredAddQueue.Count() > 0)
                {
                    idx = 0;
                    elemLen = this.mDeferredAddQueue.Count();

                    while(idx < elemLen)
                    {
                        this.addObject(this.mDeferredAddQueue.get(idx) as IDelayHandleItem);

                        idx += 1;
                    }

                    this.mDeferredAddQueue.Clear();
                }

                if (this.mDeferredDelQueue.Count() > 0)
                {
                    idx = 0;
                    elemLen = this.mDeferredDelQueue.Count();

                    while(idx < elemLen)
                    {
                        this.removeObject(this.mDeferredDelQueue.get(idx) as IDelayHandleItem);

                        idx += 1;
                    }

                    this.mDeferredDelQueue.Clear();
                }
            }
        }

        protected void incDepth()
        {
            this.mLoopDepth.incDepth();
        }

        protected void decDepth()
        {
            this.mLoopDepth.decDepth();
        }

        protected bool isInDepth()
        {
            return this.mLoopDepth.isInDepth();
        }
    }
}