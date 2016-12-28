using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 背包界面
     */
    public class UIPack : Form
    {
        protected AuxScrollView mScrollView;

        public UIPack()
        {
            mScrollView = new AuxScrollView();
        }

        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            base.onInit();
        }

        override public void onShow()
        {
            base.onShow();
        }
        
        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
            test();
        }

        // 每一次隐藏都会调用一次
        override public void onHide()
		{
            base.onHide();
		}

        // 每一次关闭都会调用一次
        override public void onExit()
        {
            base.onExit();
        }

        protected void findWidget()
        {
            mScrollView.setSelfGo(mGuiWin.mUiRoot, PackComPath.Group);
        }

        protected void addEventHandle()
        {
            
        }

        protected void test()
        {
            mScrollView.setResPath("UI/UIPack/PackItem.prefab");

            PackItem item = null;

            int idx = 0;
            int len = 10;

            while(idx < len)
            {
                item = new PackItem();

                mScrollView.addItem(item);
                ++idx;
            }

            mScrollView.updateItemList();
        }
    }
}