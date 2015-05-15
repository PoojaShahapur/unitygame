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
        protected JobSelectData m_jobSelectData;

        protected Text m_logText;

        public override void onInit()
        {
            //exitMode = false;         // 直接隐藏
            //hideOnCreate = true;
            base.onInit();
        }
        
        // 初始化控件
        override public void onReady()
        {
            m_jobSelectData = new JobSelectData();      // 最先初始化全局数据
            m_jobSelectData.m_form = this;

            findWidget();
            addEventHandle();

            m_jobSelectData.init();
        }

        override public void onShow()
        {

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
            m_jobSelectData.findWidget();
        }

        protected void addEventHandle()
        {
            m_jobSelectData.addEventHandle();
        }

        protected void onBtnClkTest()
        {

        }
    }
}