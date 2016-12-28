using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 测试界面
     */
    public class UIPack : Form
    {
        public AuxLabel m_logText;

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
            m_logText = new AuxLabel(m_guiWin.m_uiRoot, "LogText");
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_guiWin.m_uiRoot, "BtnTest", onBtnClkTest);
        }

        protected void onBtnClkTest()
        {
            Ctx.mInstance.mUiMgr.exitForm(UIFormID.eUITest);
            Ctx.mInstance.mModuleSys.unloadModule(ModuleID.LOGINMN);
            Ctx.mInstance.mModuleSys.loadModule(ModuleID.GAMEMN);
        }
    }
}