namespace SDK.Lib
{
    public class ScriptRes : InsResBase
    {
        protected System.Type m_scriptType;
        protected System.Object m_retObj;

        public System.Type scriptType
        {
            get
            {
                return m_scriptType;
            }
            set
            {
                m_scriptType = value;
            }
        }

        public System.Object createInstance()
        {
            m_retObj = m_scriptType.Assembly.CreateInstance(m_scriptType.FullName);
            return m_retObj;
        }
    }
}