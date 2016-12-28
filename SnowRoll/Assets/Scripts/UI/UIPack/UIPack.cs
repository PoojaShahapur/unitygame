using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 背包界面
     */
    public class UIPack : Form
    {
        protected AuxLabel mLogText;
        protected AuxScrollView mScrollView;

        public UIPack()
        {
            mLogText = new AuxLabel();
            mScrollView = new AuxScrollView();
        }

        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            //hideOnCreate = true;
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
            mLogText.setSelfGo(mGuiWin.mUiRoot, "LogText");
            mScrollView.setSelfGo(mGuiWin.mUiRoot, "LogText");
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(mGuiWin.mUiRoot, "BtnTest", onBtnClkTest);
        }

        protected void onBtnClkTest()
        {
            Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUITest);
            Ctx.mInstance.mModuleSys.unloadModule(ModuleId.LOGINMN);
            Ctx.mInstance.mModuleSys.loadModule(ModuleId.GAMEMN);
        }
    }
}