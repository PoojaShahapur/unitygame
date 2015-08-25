using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 左边面板
     */
    public class JobSelLeftPnl : JobSelPnlBase
    {
        public JobSelLeftPnl(JobSelectData data):
            base(data)
        {
            
        }

        public new void findWidget()
        {
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.BtnJobSel, onJobSelBtnClk);
            UtilApi.addEventHandle(m_jobSelectData.m_form.m_GUIWin.m_uiRoot, JobSelectPath.BtnRet, onRetBtnClk);
        }

        public new void init()
        {

        }

        protected void onJobSelBtnClk()
        {

        }

        protected void onRetBtnClk()
        {
            m_jobSelectData.m_form.exit();
        }
    }
}