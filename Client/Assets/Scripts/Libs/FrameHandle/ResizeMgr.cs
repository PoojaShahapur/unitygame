using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class ResizeMgr
    {
        protected List<IResizeObject> m_ResizeLst = new List<IResizeObject>();

        public void addResizeObject(IResizeObject obj)
        {
            m_ResizeLst.Add(obj);
        }

        public void onResize(int viewWidth, int viewHeight)
        {
            foreach (IResizeObject resizeObj in m_ResizeLst)
            {
                resizeObj.onResize(viewWidth, viewHeight);
            }
        }
    }
}