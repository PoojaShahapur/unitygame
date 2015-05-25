using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.IO;
using UnityEngine;

namespace Game.UI
{
    public class UITuJianCardMenu : Form
    {
        protected AuxBasicButton[] m_btnArr = new AuxBasicButton[2];

        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        override public void onShow()
        {
            base.onShow();
        }

        public void findWidget()
        {
            m_btnArr[0] = new AuxBasicButton(this.m_GUIWin.m_uiRoot, TuJianCardMenuComPath.CardSetEdit_BtnAdd);
            m_btnArr[1] = new AuxBasicButton(this.m_GUIWin.m_uiRoot, TuJianCardMenuComPath.BtnExit);
        }

        public void addEventHandle()
        {
            m_btnArr[0].addEventHandle(onAddBtnClk);
            m_btnArr[1].addEventHandle(onExitBtnClk);
        }

        protected void onAddBtnClk(IDispatchObject dispObj)
        {
            UITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);
            if (uiTuJian != null)
            {
                uiTuJian.addCurCard2CardSet();
            }
            exit();
        }

        public void onExitBtnClk(IDispatchObject dispObj)
        {
            exit();
        }
    }
}