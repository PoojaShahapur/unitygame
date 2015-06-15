using SDK.Common;
using SDK.Lib;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class TuJianFilterMenuPnl : TuJianTopPnlBase
    {
        public TuJianFilterMenuPnl(TuJianTopData data) :
            base(data)
        {
            
        }

        virtual public new void findWidget()
        {

        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter0f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter1f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter2f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter3f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter4f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter5f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter6f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter7f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Filter8f, onFilterTypeBtnClk);
        }

        protected void onFilterTypeBtnClk()
        {
            int idx = UtilLogic.findIdxByUnderline(EventSystem.current.currentSelectedGameObject.name);
            UITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);
            if(uiTuJian != null)
            {
                uiTuJian.updateFilter(idx);
            }

            m_tuJianTopData.m_form.exit();
        }
    }
}