namespace SDK.Lib
{
    /**
     * @brief 透视投影镜头
     */
    public class MPerspectiveLens : MLensBase
    {
        // y-direction field-of-view (default 45)
        protected float m_fieldOfView;
        // 焦距长度
        protected float m_focalLength;

        public MPerspectiveLens()
        {
            m_fieldOfView = 45;
            m_focalLength = 1;
        }

        override public void setFieldOfView(float value)
        {
            m_fieldOfView = value;
        }

        override public void setFocalLength(float value)
        {
            m_focalLength = value;
        }
    }
}