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
        protected float m_yMax;
		protected float m_xMax;

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

        /**
         * @brief 更新投影矩阵
         */
        override protected void updateMatrix()
		{
			m_yMax = m_nearDist * m_focalLengthInv;
            m_xMax = m_yMax* m_aspectRatio;

            float left, right, top, bottom;
			
			left = -m_xMax;
			right = m_xMax;
			top = -m_yMax;
			bottom = m_yMax;

            m_matrix3D.m[0, 0] = m_nearDist / m_xMax;
            m_matrix3D.m[1, 1] = m_nearDist / m_yMax;
            m_matrix3D.m[2, 2] = m_farDist / (m_farDist - m_nearDist);
            m_matrix3D.m[2, 3] = 1;
            m_matrix3D.m[0, 1] = m_matrix3D.m[0, 2] = raw[0, 3] = raw[1, 0] =
                m_matrix3D.m[uint(6)] = m_matrix3D.m[uint(7)] = raw[uint(8)] = raw[uint(9)] =
                m_matrix3D.m[uint(12)] = m_matrix3D.m[uint(13)] = raw[uint(15)] = 0;
            m_matrix3D.m[uint(14)] = -_near* raw[uint(10)];

			float yMaxFar = m_farDist * m_focalLengthInv;
            float xMaxFar = yMaxFar* m_aspectRatio;

            m_frustumCorners[0] = m_frustumCorners[9] = left;
			m_frustumCorners[3] = m_frustumCorners[6] = right;
			m_frustumCorners[1] = m_frustumCorners[4] = top;
			m_frustumCorners[7] = m_frustumCorners[10] = bottom;
			
			m_frustumCorners[12] = m_frustumCorners[21] = -xMaxFar;
			m_frustumCorners[15] = m_frustumCorners[18] = xMaxFar;
			m_frustumCorners[13] = m_frustumCorners[16] = -yMaxFar;
			m_frustumCorners[19] = m_frustumCorners[22] = yMaxFar;
			
			m_frustumCorners[2] = m_frustumCorners[5] = m_frustumCorners[8] = m_frustumCorners[11] = m_nearDist;
			m_frustumCorners[14] = m_frustumCorners[17] = m_frustumCorners[20] = m_frustumCorners[23] = m_farDist;
			
			m_matrixInvalid = false;
		}
    }
}