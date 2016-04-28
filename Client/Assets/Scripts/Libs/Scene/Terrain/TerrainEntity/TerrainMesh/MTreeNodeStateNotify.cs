using System;

namespace SDK.Lib
{
    /**
     * @breif Tree Node 状态变化通知
     */
    public class MTreeNodeStateNotify : IDispatchObject
    {
        public MTerrainQuadTreeNode mNode;
        protected AddOnceEventDispatch mShowDispatch;
        protected AddOnceEventDispatch mHideDispatch;

        public MTreeNodeStateNotify(MTerrainQuadTreeNode node)
        {
            mNode = node;
            mShowDispatch = new AddOnceEventDispatch();
            mHideDispatch = new AddOnceEventDispatch();
        }

        public void init()
        {

        }

        public void onInit()
        {

        }

        public void onShow()
        {
            mShowDispatch.dispatchEvent(this);
        }

        public void onHide()
        {
            mHideDispatch.dispatchEvent(this);
        }

        public void onExit()
        {

        }

        public void addShowEventHandle(Action<IDispatchObject> handle)
        {
            mShowDispatch.addEventHandle(handle);
        }

        public void addHideEventHandle(Action<IDispatchObject> handle)
        {
            mHideDispatch.addEventHandle(handle);
        }
    }
}