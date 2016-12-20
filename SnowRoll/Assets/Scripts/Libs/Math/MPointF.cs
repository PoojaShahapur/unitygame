namespace SDK.Lib
{
    public class MPointF
    {
        protected float m_x;
        protected float m_y;

        public MPointF(float x, float y)
        {
            m_x = x;
            m_y = y;
        }

        public float X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }

        public float Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }

        public float x
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }

        public float y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }
    }
}