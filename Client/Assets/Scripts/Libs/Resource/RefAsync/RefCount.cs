namespace SDK.Lib
{
    public class RefCount
    {
        protected uint m_refNum;                // 引用计数

        public RefCount()
        {
            m_refNum = 0;       // 引用计数从 1 改成 0 
        }

        public uint refNum
        {
            get
            {
                return m_refNum;
            }
            set
            {
                m_refNum = value;
            }
        }

        public void incRef()
        {
            ++m_refNum;
        }

        public void decRef()
        {
            --m_refNum;
        }

        public bool bNoRef()
        {
            return m_refNum == 0;
        }

        public void copyFrom(RefCount rhv)
        {
            m_refNum = rhv.refNum;
        }
    }
}