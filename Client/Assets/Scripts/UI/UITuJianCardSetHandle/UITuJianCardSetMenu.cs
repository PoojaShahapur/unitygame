using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITuJianCardSetMenu : Form
    {
        override public void onShow()
        {

        }

        // 初始化控件
        override public void onReady()
        {
            findWidget();
            addEventHandle();
        }

        public void findWidget()
        {
            
        }

        public void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.getComByP<Button>(this.m_GUIWin.m_uiRoot, TuJianCardSetMenuComPath.CardSetEdit_BtnDel), onDelBtnClk);
            UtilApi.addEventHandle(UtilApi.getComByP<Button>(this.m_GUIWin.m_uiRoot, TuJianCardSetMenuComPath.CardSetEdit_BtnRename), onRenameBtnClk);
            UtilApi.addEventHandle(UtilApi.getComByP<Button>(this.m_GUIWin.m_uiRoot, TuJianCardSetMenuComPath.CardSetEdit_BtnEdit), onEditBtnClk);
        }

        protected void onDelBtnClk()
        {
            InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
            param.m_btnClkDisp = delRet;
            param.m_midDesc = Ctx.m_instance.m_langMgr.getText(LangTypeId.eTuJian6, LangItemID.eItem0);
            UIInfo.showMsg(param);
        }

        protected void onRenameBtnClk()
        {
            exit();
        }

        protected void onEditBtnClk()
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
    }
}