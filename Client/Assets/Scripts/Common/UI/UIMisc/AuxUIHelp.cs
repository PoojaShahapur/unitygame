namespace SDK.Common
{
    /**
     * @brief 用来在帮助 UI 完成一些动态加载的内容
     */
    public class AuxUIHelp
    {
        public AuxJobSelectData m_auxJobSelectData;
        public AuxTuJian m_auxTuJian;

        public AuxUIHelp()
        {
            m_auxJobSelectData = new AuxJobSelectData();
            m_auxTuJian = new AuxTuJian();
        }
    }
}