using SDK.Common;
using SDK.Lib;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class TuJianJobSelPnl : TuJianTopPnlBase
    {
        protected AuxStaticImageStaticGoImage m_jobBtnImage;
        protected AuxBasicButton[] m_btnArr = new AuxBasicButton[(int)LeftBtnPnl_BtnIndex.eBtnJobTotal];

        public TuJianJobSelPnl(TuJianTopData data) :
            base(data)
        {
            
        }

        override public void findWidget()
        {
            
        }

        override public void addEventHandle()
        {
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Job0f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Job1f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Job2f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Job3f, onJobTypeBtnClk);
        }

        public void onJobTypeBtnClk()
        {
            UITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm<UITuJian>(UIFormID.eUITuJian);

            if (uiTuJian != null)
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                int idx = UtilApi.findIdxByUnderline(EventSystem.current.currentSelectedGameObject.name);
                uiTuJian.updateByCareer(idx);
            }

            m_tuJianTopData.m_form.exit();
        }
    }
}