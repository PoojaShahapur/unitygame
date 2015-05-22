using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITuJianCardMenu : Form
    {
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

        }

        public void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.getComByP<Button>(this.m_GUIWin.m_uiRoot, TuJianCardMenuComPath.CardSetEdit_BtnAdd), onAddBtnClk);
            UtilApi.addEventHandle(UtilApi.getComByP<Button>(this.m_GUIWin.m_uiRoot, TuJianCardMenuComPath.BtnExit), onExitBtnClk);
        }

        protected void onAddBtnClk()
        {
            UITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);
            if (uiTuJian != null)
            {
                uiTuJian.addCurCard2CardSet();
            }
            exit();
        }

        public void onExitBtnClk()
        {
            exit();
        }
    }
}