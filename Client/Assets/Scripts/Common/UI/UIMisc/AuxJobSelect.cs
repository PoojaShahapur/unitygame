namespace SDK.Common
{
    /**
     * @brief 职业选择或者模式
     */
    public enum JobSelectMode
    {
        eNewCardSet,        // 建立卡牌组模式
        eDz,                // 对战模式
        //eMaoXian,           // 冒险
    }

    public class AuxJobSelectData
    {
        protected JobSelectMode m_jobSelectMode;

        public JobSelectMode jobSelectMode
        {
            get
            {
                return m_jobSelectMode;
            }
            set
            {
                m_jobSelectMode = value;
            }
        }

        public void enterJobSelectMode()
        {
            m_jobSelectMode = JobSelectMode.eNewCardSet;
        }

        public void enterDZMode()
        {
            m_jobSelectMode = JobSelectMode.eDz;
        }
    }
}
