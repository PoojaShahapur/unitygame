using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.IO;
using UnityEngine;

namespace Game.UI
{
    public class UITuJianCardSetMenu : Form
    {
        protected AuxButton[] m_btnArr = new AuxButton[4];

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
            m_btnArr[0] = new AuxButton(this.m_GUIWin.m_uiRoot, TuJianCardSetMenuComPath.CardSetEdit_BtnDel);
            m_btnArr[1] = new AuxButton(this.m_GUIWin.m_uiRoot, TuJianCardSetMenuComPath.CardSetEdit_BtnRename);
            m_btnArr[2] = new AuxButton(this.m_GUIWin.m_uiRoot, TuJianCardSetMenuComPath.CardSetEdit_BtnEdit);
            m_btnArr[3] = new AuxButton(this.m_GUIWin.m_uiRoot, TuJianCardSetMenuComPath.BtnExit);
        }

        public void addEventHandle()
        {
            m_btnArr[0].addEventHandle(onDelBtnClk);
            m_btnArr[1].addEventHandle(onRenameBtnClk);
            m_btnArr[2].addEventHandle(onEditBtnClk);
            m_btnArr[3].addEventHandle(onExitBtnClk);
        }

        protected void onDelBtnClk(IDispatchObject dispObj)
        {
            InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
            param.m_btnClkDisp = delRet;
            param.m_midDesc = Ctx.m_instance.m_langMgr.getText(LangTypeId.eTuJian6, LangItemID.eItem0);
            UIInfo.showMsg(param);
        }

        protected void onRenameBtnClk(IDispatchObject dispObj)
        {
            exit();
        }

        protected void onEditBtnClk(IDispatchObject dispObj)
        {
            UITuJian tujian = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);
            tujian.editCurCardSet();
            exit();
        }

        protected void delRet(InfoBoxBtnType type)
        {
            if (InfoBoxBtnType.eBTN_OK == type)
            {
                UITuJian tujian = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);
                tujian.delCardSet();
            }

            exit();
        }

        public void onExitBtnClk(IDispatchObject dispObj)
        {
            exit();
        }
    }
}