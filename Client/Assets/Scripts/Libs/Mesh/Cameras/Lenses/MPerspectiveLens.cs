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

        /**
         * @brief 更新投影矩阵和 Frustum 的八个顶点
         */
        override public void updateMatrix()
		{
            float halfHeight = m_nearDist * m_focalLengthInv;
            float halfWidth = halfHeight * m_aspectRatio;

            float left = 0, right = 0, top = 0, bottom = 0;
			
			left = -halfWidth;
			right = halfWidth;
			top = halfHeight;
			bottom = -halfHeight;

            float invWidth = 1 / (right - left);
            float invHeight = 1 / (top - bottom);
            float invDepth = 1 / (m_farDist - m_nearDist);

            m_matrix3D.m[0, 0] = 2 * m_nearDist * invWidth;
            m_matrix3D.m[0, 2] = (right + left) * (right - left);
            m_matrix3D.m[1, 1] = 2 * m_nearDist * invHeight;
            m_matrix3D.m[1, 2] = (top + bottom) / (top - bottom);
            m_matrix3D.m[2, 2] = -(m_farDist + m_nearDist) / (m_farDist - m_nearDist);
            m_matrix3D.m[2, 3] = -2 * (m_farDist * m_nearDist) / (m_farDist - m_nearDist);
            m_matrix3D.m[3, 2] = -1;
            m_matrix3D.m[0, 1] = m_matrix3D.m[0, 3] = m_matrix3D.m[1, 0] = m_matrix3D.m[1, 3] = m_matrix3D.m[2, 0] = m_matrix3D.m[2, 1] =
                m_matrix3D.m[3, 0] = m_matrix3D.m[3, 1] = m_matrix3D.m[3, 3] = 0;

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

            testLogMatrix(m_matrix3D);
        }
    }
}