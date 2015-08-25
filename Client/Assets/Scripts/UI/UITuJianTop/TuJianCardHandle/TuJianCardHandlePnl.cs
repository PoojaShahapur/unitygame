using SDK.Lib;
using SDK.Lib;

namespace Game.UI
{
    public class TuJianCardHandlePnl : TuJianTopPnlBase
    {
        protected AuxBasicButton[] m_btnArr = new AuxBasicButton[2];

        public TuJianCardHandlePnl(TuJianTopData data) : 
            base(data)
        {

        }

        override public void findWidget()
        {
            m_btnArr[0] = new AuxBasicButton(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Card_CardSetEdit_BtnAdd);
            m_btnArr[1] = new AuxBasicButton(m_tuJianTopData.m_form.m_GUIWin.m_uiRoot, TuJianTopComPath.Card_BtnExit);
        }

        override public void addEventHandle()
        {
            m_btnArr[0].addEventHandle(onAddBtnClk);
            m_btnArr[1].addEventHandle(onExitBtnClk);
        }

        protected void onAddBtnClk(IDispatchObject dispObj)
        {
            IUITuJian uiTuJian = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if (uiTuJian != null)
            {
                uiTuJian.addCurCard2CardSet();
            }
            m_tuJianTopData.m_form.exit();
        }

        public void onExitBtnClk(IDispatchObject dispObj)
        {
            m_tuJianTopData.m_form.exit();
        }
    }
}