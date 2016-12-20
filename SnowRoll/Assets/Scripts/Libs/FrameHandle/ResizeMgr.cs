namespace SDK.Lib
{
    public class ResizeMgr : DelayHandleMgrBase, ITickedObject
    {
        protected int mPreWidth;       // 之前宽度
        protected int mPreHeight;
        protected int mCurWidth;       // 现在宽度
        protected int mCurHeight;

        protected MList<IResizeObject> m_ResizeLst;

        public ResizeMgr()
        {
            m_ResizeLst = new MList<IResizeObject>();
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if(this.bInDepth())
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
            if(this.bInDepth())
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
            if (m_ResizeLst.IndexOf(obj) == -1)
            {
                m_ResizeLst.Add(obj);
            }
        }

        public void removeResizeObject(IResizeObject obj)
        {
            if (m_ResizeLst.IndexOf(obj) != -1)
            {
                m_ResizeLst.Remove(obj);
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
            foreach (IResizeObject resizeObj in m_ResizeLst.list())
            {
                resizeObj.onResize(viewWidth, viewHeight);
            }
        }
    }
}