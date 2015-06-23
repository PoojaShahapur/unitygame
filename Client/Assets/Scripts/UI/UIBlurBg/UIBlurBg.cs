using Game.Msg;
using SDK.Common;

namespace Game.UI
{
    /**
     * @brief 模糊的背景
     */
    public class UIBlurBg : Form, IUIBlurBg
    {
        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            hideOnCreate = true;
            base.onInit();
        }

        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            addEventHandle();
        }

        override public void onShow()
        {
            base.onShow();
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

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnBg", onClkBg);
        }

        // 点击背景处理
        protected void onClkBg()
        {
            // 关闭焦点窗口
            Ctx.m_instance.m_uiMgr.exitAllWin();
        }
    }
}