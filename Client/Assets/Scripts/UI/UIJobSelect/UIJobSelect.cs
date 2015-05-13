using Game.Msg;
using SDK.Common;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 职业选择界面
     */
    public class UIJobSelect : Form
    {
        public Text m_logText;

        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            //hideOnCreate = true;
            base.onInit();
        }

        override public void onShow()
        {

        }
        
        // 初始化控件
        override public void onReady()
        {
            findWidget();
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

        protected void findWidget()
        {
            //m_logText = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, "LogText");
        }

        protected void addEventHandle()
        {
            //UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnTest", onBtnClkTest);
        }

        protected void onBtnClkTest()
        {

        }
    }
}