using SDK.Lib;

namespace Game.UI
{
    public class TuJianCardSetMenuPnl : TuJianTopPnlBase
    {
        protected AuxBasicButton[] m_btnArr = new AuxBasicButton[4];

        public TuJianCardSetMenuPnl(TuJianTopData data) : 
            base(data)
        {

        }

        override public void findWidget()
        {
            m_btnArr[0] = new AuxBasicButton(m_tuJianTopData.m_form.m_guiWin.m_uiRoot, TuJianTopComPath.CardSet_CardSetEdit_BtnDel);
            m_btnArr[1] = new AuxBasicButton(m_tuJianTopData.m_form.m_guiWin.m_uiRoot, TuJianTopComPath.CardSet_CardSetEdit_BtnRename);
            m_btnArr[2] = new AuxBasicButton(m_tuJianTopData.m_form.m_guiWin.m_uiRoot, TuJianTopComPath.CardSet_CardSetEdit_BtnEdit);
            m_btnArr[3] = new AuxBasicButton(m_tuJianTopData.m_form.m_guiWin.m_uiRoot, TuJianTopComPath.CardSet_BtnExit);
        }

        override public void addEventHandle()
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
            m_tuJianTopData.m_form.exit();
        }

        protected void onEditBtnClk(IDispatchObject dispObj)
        {
            IUITuJian tujian = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            tujian.editCurCardSet();
            m_tuJianTopData.m_form.exit();
        }

        protected void delRet(InfoBoxBtnType type)
        {
            if (InfoBoxBtnType.eBTN_OK == type)
            {
                IUITuJian tujian = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
                tujian.delCardSet();
            }

            m_tuJianTopData.m_form.exit();
        }

        public void onExitBtnClk(IDispatchObject dispObj)
        {
            m_tuJianTopData.m_form.exit();
        }
    }
}