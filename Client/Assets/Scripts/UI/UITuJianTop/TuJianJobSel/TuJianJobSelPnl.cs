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
            m_btnArr[(int)Job_BtnIndex.eBtnJob0f] = new AuxBasicButton(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Job0f);
            m_btnArr[(int)Job_BtnIndex.eBtnJob1f] = new AuxBasicButton(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Job1f);
            m_btnArr[(int)Job_BtnIndex.eBtnJob2f] = new AuxBasicButton(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Job2f);
            m_btnArr[(int)Job_BtnIndex.eBtnJob3f] = new AuxBasicButton(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Job3f);

            IUITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if(uiTuJian != null)
            {
                if(uiTuJian.bInEditMode())
                {
                    m_btnArr[(int)Job_BtnIndex.eBtnJob1f].hide();
                    m_btnArr[(int)Job_BtnIndex.eBtnJob2f].hide();
                    m_btnArr[(int)Job_BtnIndex.eBtnJob3f].hide();

                    m_btnArr[(int)uiTuJian.getEditCareerID()].show();
                }
            }
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
            IUITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;

            if (uiTuJian != null)
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                int idx = UtilLogic.findIdxByUnderline(EventSystem.current.currentSelectedGameObject.name);
                uiTuJian.updateByCareer(idx);
            }

            m_tuJianTopData.m_form.exit();
        }
    }
}