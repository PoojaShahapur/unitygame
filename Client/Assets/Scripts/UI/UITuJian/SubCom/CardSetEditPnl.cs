using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    /**
     * @brief 卡牌编辑面板
     */
    public class CardSetEditPnl : TuJianPnlBase
    {
        protected GameObject m_rootGo;

        public CardSetEditPnl(TuJianData data) :
            base(data)
        {
            
        }

        public new void findWidget()
        {
            m_rootGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetEditGo);
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.getComByP<Button>(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetEdit_BtnDel), onDelBtnClk);
            UtilApi.addEventHandle(UtilApi.getComByP<Button>(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetEdit_BtnRename), onRenameBtnClk);
            UtilApi.addEventHandle(UtilApi.getComByP<Button>(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.CardSetEdit_BtnEdit), onEditBtnClk);
        }

        public new void init()
        {
            UtilApi.SetActive(m_rootGo, false);
        }

        public void showCardSetEdit()
        {
            UtilApi.SetActive(m_rootGo, true);
        }

        protected void onDelBtnClk()
        {
            UtilApi.SetActive(m_rootGo, false);

            InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
            param.m_btnClkDisp = delRet;
            param.m_midDesc = Ctx.m_instance.m_langMgr.getText(LangTypeId.eTuJian6, LangItemID.eItem0);
            UIInfo.showMsg(param);
        }

        protected void onRenameBtnClk()
        {
            UtilApi.SetActive(m_rootGo, false);
        }

        protected void onEditBtnClk()
        {
            UtilApi.SetActive(m_rootGo, false);
            m_tuJianData.m_wdscCardSetPnl.editCurCardSet();
        }

        protected void delRet(InfoBoxBtnType type)
        {
            if(InfoBoxBtnType.eBTN_OK == type)
            {
                m_tuJianData.m_wdscCardSetPnl.delCardSet();
            }
        }
    }
}