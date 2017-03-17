namespace SDK.Lib
{
    public class ResizeMgr : DelayPriorityHandleMgrBase, ITickedObject, IDelayHandleItem, INoOrPriorityObject
    {
        protected int mPreWidth;       // 之前宽度
        protected int mPreHeight;
        protected int mCurWidth;       // 现在宽度
        protected int mCurHeight;

        protected int mCurHalfWidth;       // 当前一半宽度
        protected int mCurHalfHeight;

        protected MList<IResizeObject> mResizeList;

        public ResizeMgr()
        {
            this.mResizeList = new MList<IResizeObject>();
        }

        override public void init()
        {

        }

        override public void dispose()
        {
            this.mResizeList.Clear();
        }

        public int getWidth()
        {
            return this.mCurWidth;
        }

        public int getHeight()
        {
            return this.mCurHeight;
        }

        public int getHalfWidth()
        {
            return this.mCurHalfWidth;
        }

        public int getHalfHeight()
        {
            return this.mCurHalfHeight;
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if(this.isInDepth())
            {
                base.addObject(delayObject, priority);
            }
            else
            {
                this.addResizeObject(delayObject as IResizeObject, priority);
            }
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            if(this.isInDepth())
            {
                base.removeObject(delayObject);
            }
            else
            {
                this.removeResizeObject(delayObject as IResizeObject);
            }
        }

        public void addResizeObject(IResizeObject obj, float priority = 0)
        {
            if (!this.mResizeList.Contains(obj))
            {
                this.mResizeList.Add(obj);
            }
        }

        public void removeResizeObject(IResizeObject obj)
        {
            if (this.mResizeList.IndexOf(obj) != -1)
            {
                this.mResizeList.Remove(obj);
            }
        }

        public void onTick(float delta, TickMode tickMode)
        {
            this.mPreWidth = this.mCurWidth;
            this.mCurWidth = UtilApi.getScreenWidth();
            this.mCurHalfWidth = this.mCurWidth / 2;

            this.mPreHeight = this.mCurHeight;
            this.mCurHeight = UtilApi.getScreenHeight();
            this.mCurHalfHeight = this.mCurHeight / 2;

            if (this.mPreWidth != this.mCurWidth || this.mPreHeight != this.mCurHeight)
            {
                this.onResize(this.mCurWidth, this.mCurHeight);
            }
        }

        public void onResize(int viewWidth, int viewHeight)
        {
            foreach (IResizeObject resizeObj in mResizeList.list())
            {
                resizeObj.onResize(viewWidth, viewHeight);
            }
        }

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }
    }
}