namespace SDK.Lib
{
    public class MSizeF
    {
        protected float m_width;
        protected float m_height;

        public MSizeF(float width, float height)
        {
            m_width = width;
            m_height = height;
        }

        public float Width
        {
            get
            {
                return m_width;
            }
            set
            {
                m_width = value;
            }
        }

        public float Height
        {
            get
            {
                return m_height;
            }
            set
            {
                m_height = value;
            }
        }
    }
}