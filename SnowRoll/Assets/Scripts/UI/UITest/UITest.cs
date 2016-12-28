using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 测试界面
     */
    public class UITest : Form, IUITest
    {
        public AuxLabel mLogText;

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
            mLogText = new AuxLabel(mGuiWin.mUiRoot, "LogText");
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