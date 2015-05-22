using Game.Msg;
using SDK.Common;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 职业选择界面和模式选择界面是一个界面
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
            base.onReady();
            m_jobSelectData = new JobSelectData();      // 最先初始化全局数据
            m_jobSelectData.m_form = this;

            findWidget();
            addEventHandle();

            m_jobSelectData.init();
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
            UITuJian tujian = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);
            if(tujian != null)
            {
                tujian.toggleCardVisible(true);
            }

            m_jobSelectData.dispose();
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

        public void psstRetHeroFightMatchUserCmd(stRetHeroFightMatchUserCmd cmd)
        {
            m_jobSelectData.m_midPnl.matchSuccess();
        }

        // 更新 hero 显示
        public void updateHeroList()
        {
            m_jobSelectData.m_midPnl.updateHeroList();
        }
    }
}