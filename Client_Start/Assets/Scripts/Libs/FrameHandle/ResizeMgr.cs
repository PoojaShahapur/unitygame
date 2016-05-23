namespace SDK.Lib
{
    public class ResizeMgr : DelayHandleMgrBase, ITickedObject
    {
        protected int m_preWidth;       // 之前宽度
        protected int m_preHeight;
        protected int m_curWidth;       // 现在宽度
        protected int m_curHeight;

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
            m_preWidth = m_curWidth;
            m_curWidth = UtilApi.getScreenWidth();
            m_preHeight = m_curHeight;
            m_curHeight = UtilApi.getScteedHeight();

            if(m_preWidth != m_curWidth || m_preHeight != m_curHeight)
            {
                this.onResize(m_curWidth, m_curHeight);
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