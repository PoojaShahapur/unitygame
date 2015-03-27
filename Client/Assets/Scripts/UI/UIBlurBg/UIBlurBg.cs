using Game.Msg;
using SDK.Common;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 模糊的背景
     */
    public class UIBlurBg : Form
    {
        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            hideOnCreate = true;
            base.onInit();
        }

        override public void onShow()
        {

        }
        
        // 初始化控件
        override public void onReady()
        {
            addEventHandle();
        }

        // 每一次隐藏都会调用一次
        override public void onHide()
		{
             
		}

        // 每一次关闭都会调用一次
        override public void onExit()
        {

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