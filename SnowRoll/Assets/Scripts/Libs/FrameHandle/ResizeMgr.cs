namespace SDK.Lib
{
    public class ResizeMgr : DelayHandleMgrBase, ITickedObject
    {
        protected int mPreWidth;       // 之前宽度
        protected int mPreHeight;
        protected int mCurWidth;       // 现在宽度
        protected int mCurHeight;

        protected MList<IResizeObject> mResizeList;

        public ResizeMgr()
        {
            mResizeList = new MList<IResizeObject>();
        }

        public void init()
        {

        }

        public void dispose()
        {
            this.mResizeList.Clear();
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
            if (mResizeList.IndexOf(obj) == -1)
            {
                mResizeList.Add(obj);
            }
        }

        public void removeResizeObject(IResizeObject obj)
        {
            if (mResizeList.IndexOf(obj) != -1)
            {
                mResizeList.Remove(obj);
            }
        }

        public void onTick(float delta)
        {
            mPreWidth = this.mCurWidth;
            this.mCurWidth = UtilApi.getScreenWidth();
            this.mPreHeight = this.mCurHeight;
            this.mCurHeight = UtilApi.getScreenHeight();

            if(this.mPreWidth != this.mCurWidth || this.mPreHeight != this.mCurHeight)
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
    }
}