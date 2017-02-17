namespace SDK.Lib
{
    // 每一帧执行的对象管理器
    public class TickObjectMgrBase : DelayHandleMgrBase, ITickedObject, IDelayHandleItem
    {
        protected MList<ITickedObject> mTickObjectList;

        public TickObjectMgrBase()
        {
            this.mTickObjectList = new MList<ITickedObject>();
        }

        override public void init()
        {

        }

        override public void dispose()
        {

        }

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        virtual public void onTick(float delta)
        {
            this.mLoopDepth.incDepth();

            this.onExecTick(delta);

            this.mLoopDepth.decDepth();
        }

        virtual protected void onExecTick(float delta)
        {
            int idx = 0;
            int count = this.mTickObjectList.Count();
            ITickedObject tickObject = null;

            while (idx < count)
            {
                tickObject = this.mTickObjectList[idx];

                if (!(tickObject as IDelayHandleItem).isClientDispose())
                {
                    tickObject.onTick(delta);
                }

                ++idx;
            }
        }

        override protected void addObject(IDelayHandleItem tickObject, float priority = 0.0f)
        {
            if (this.mLoopDepth.isInDepth())
            {
                base.addObject(tickObject);
            }
            else
            {
                if (this.mTickObjectList.IndexOf(tickObject as ITickedObject) == -1)
                {
                    this.mTickObjectList.Add(tickObject as ITickedObject);
                }
            }
        }

        override protected void removeObject(IDelayHandleItem tickObject)
        {
            if (this.mLoopDepth.isInDepth())
            {
                base.removeObject(tickObject);
            }
            else
            {
                if (this.mTickObjectList.IndexOf(tickObject as ITickedObject) != -1)
                {
                    this.mTickObjectList.Remove(tickObject as ITickedObject);
                }
            }
        }
    }
}