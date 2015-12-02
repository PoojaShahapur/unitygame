namespace SDK.Lib
{
    /**
     * brief 镜头基类
     */
    public class MLensBase
    {
        // Far clip distance - default 10000
        protected float m_farDist;
        // Near clip distance - default 100
        protected float m_nearDist;
        // x/y viewport ratio - default 1.3333
        protected float m_aspectRatio;
        // 存放 Frustum 的四面体的八个顶点
        protected MList<float> m_frustumCorners;

        protected MLensBase()
        {
            m_farDist = 1000;
            m_nearDist = 1000;

            m_aspectRatio = 1.3333f;
            m_frustumCorners = new MList<float>();
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