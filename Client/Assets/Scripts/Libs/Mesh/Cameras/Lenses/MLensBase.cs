namespace SDK.Lib
{
    /**
     * brief 镜头
     */
    public class MLensBase
    {
        // Far clip distance - default 10000
        protected float m_farDist;
        // Near clip distance - default 100
        protected float m_nearDist;
        // x/y viewport ratio - default 1.3333
        protected float m_aspectRatio;

        protected MLensBase()
        {
            m_farDist = 1000;
            m_nearDist = 1000;

            m_aspectRatio = 1.3333f;
        }

        virtual public void setFieldOfView(float value)
        {
            
        }

        public void setFarDist(float value)
        {
            m_farDist = value;
        }

        public void setNearDist(float value)
        {
            m_nearDist = value;
        }

        public void setAspectRatio(float value)
        {
            m_aspectRatio = value;
        }

        virtual public void setFocalLength(float value)
        {
            
        }
    }
}