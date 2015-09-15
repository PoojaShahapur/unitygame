using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 职业选择数据
     */
    public class JobSelectData
    {
        public Form m_form;
        public JobSelLeftPnl m_leftPnl;
        public JobSelMidPnl m_midPnl;
        public JobSelRightPnl m_rightPnl;

        public JobSelectData()
        {
            m_leftPnl = new JobSelLeftPnl(this);
            m_midPnl = new JobSelMidPnl(this);
            m_rightPnl = new JobSelRightPnl(this);
        }

        public void findWidget()
        {
            m_leftPnl.findWidget();
            m_midPnl.findWidget();
            m_rightPnl.findWidget();
        }

        public void addEventHandle()
        {
            m_leftPnl.addEventHandle();
            m_midPnl.addEventHandle();
            m_rightPnl.addEventHandle();
        }

        public void init()
        {
            m_leftPnl.init();
            m_midPnl.init();
            m_rightPnl.init();
        }

        public void dispose()
        {
            m_leftPnl.dispose();
            m_midPnl.dispose();
            m_rightPnl.dispose();
        }
    }
}