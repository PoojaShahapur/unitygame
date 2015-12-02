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
        protected float m_focalLength;          // 焦距
        protected float m_focalLengthInv;       // 焦距的倒数

        public MPerspectiveLens()
        {
            m_fieldOfView = 45;
            m_focalLength = 1;
        }

        override public void setFieldOfView(float value)
        {
            if (value == m_fieldOfView)
            {
                return;
            }

            m_fieldOfView = value;

            m_focalLengthInv = UtilApi.tan(m_fieldOfView * UtilApi.PI / 360);
            m_focalLength = 1 / m_focalLengthInv;
        }

        override public void setFocalLength(float value)
        {
            if (value == m_focalLength)
            {
                return;
            }

            m_focalLength = value;

            m_focalLengthInv = 1 / m_focalLength;
            m_fieldOfView = (float)(UtilApi.atan(m_focalLengthInv) * 360 / UtilApi.PI);
        }
    }
}