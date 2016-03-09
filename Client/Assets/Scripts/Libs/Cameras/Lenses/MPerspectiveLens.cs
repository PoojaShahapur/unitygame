﻿using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 透视投影镜头
     */
    public class MPerspectiveLens : MLensBase
    {
        protected float m_fieldOfView;          // y 方向 FieldOfView，默认值是 60
        protected float m_focalLength;          // 焦距长度
        protected float m_focalLengthInv;       // 焦距的倒数

        protected MPlane m_leftPanel;         // 左边面板

        public MPerspectiveLens()
        {
            m_fieldOfView = 0;
            m_focalLength = 1;
            setFieldOfView(60); // 设置对应的值
            m_leftPanel = new MPlane();
        }

        override public void setFieldOfView(float value)
        {
            if (value == m_fieldOfView)
            {
                return;
            }

            m_fieldOfView = value;
            m_focalLengthInv = UtilMath.tan((float)(m_fieldOfView * UtilMath.PI / 360));
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
            m_fieldOfView = (float)(UtilMath.atan(m_focalLengthInv) * 360 / UtilMath.PI);
        }

        /**
         * @brief 更新投影矩阵和 Frustum 的八个顶点
         */
        override public void updateMatrix()
		{
            // 计算投影矩阵，投影矩阵使用的是 [-1, 1] x [-1, 1] x [-1, 1]，使用 OpenGL 的投影矩阵
            float nearHalfHeight = m_nearDist * m_focalLengthInv;
            float nearHalfWidth = nearHalfHeight * m_aspectRatio;

            float left = 0, right = 0, top = 0, bottom = 0;
			
			left = -nearHalfWidth;
			right = nearHalfWidth;
			top = nearHalfHeight;
			bottom = -nearHalfHeight;

            float invWidth = 1 / (right - left);
            float invHeight = 1 / (top - bottom);
            float invDepth = 1 / (m_farDist - m_nearDist);

            m_matrix3D[0, 0] = 2 * m_nearDist * invWidth;
            m_matrix3D[0, 2] = (right + left) * (right - left);
            m_matrix3D[1, 1] = 2 * m_nearDist * invHeight;
            m_matrix3D[1, 2] = (top + bottom) / (top - bottom);
            m_matrix3D[2, 2] = -(m_farDist + m_nearDist) / (m_farDist - m_nearDist);
            m_matrix3D[2, 3] = -2 * (m_farDist * m_nearDist) / (m_farDist - m_nearDist);
            m_matrix3D[3, 2] = -1;
            m_matrix3D[0, 1] = m_matrix3D[0, 3] = m_matrix3D[1, 0] = m_matrix3D[1, 3] = m_matrix3D[2, 0] = m_matrix3D[2, 1] =
                m_matrix3D[3, 0] = m_matrix3D[3, 1] = m_matrix3D[3, 3] = 0;

            // 更新 Frustum 的八个顶点，这个是相机空间的坐标位置
            float farHalfHeight = m_farDist * m_focalLengthInv;
            float farHalfWidth = farHalfHeight * m_aspectRatio;

            // 顶点位置是 0 - near x left x top
            // 顶点位置是 1 - near x right x top
            // 顶点位置是 2 - near x right x bottom
            // 顶点位置是 3 - near x left x bottom
            // 顶点位置是 4 - far x left x top
            // 顶点位置是 5 - far x right x top
            // 顶点位置是 6 - far x right x bottom
            // 顶点位置是 7 - far x left x bottom
            // 0 - 1    4 - 5
            // |   |    |   |
            // 3 - 2    7 - 6
            m_frustumCorners[0] = m_frustumCorners[9] = left;
			m_frustumCorners[3] = m_frustumCorners[6] = right;
			m_frustumCorners[1] = m_frustumCorners[4] = top;
			m_frustumCorners[7] = m_frustumCorners[10] = bottom;
			
			m_frustumCorners[12] = m_frustumCorners[21] = -farHalfWidth;
			m_frustumCorners[15] = m_frustumCorners[18] = farHalfWidth;
			m_frustumCorners[13] = m_frustumCorners[16] = farHalfHeight;
			m_frustumCorners[19] = m_frustumCorners[22] = -farHalfHeight;
			
			m_frustumCorners[2] = m_frustumCorners[5] = m_frustumCorners[8] = m_frustumCorners[11] = -m_nearDist;
			m_frustumCorners[14] = m_frustumCorners[17] = m_frustumCorners[20] = m_frustumCorners[23] = -m_farDist;

            m_matrixInvalid = false;

            //testLogMatrix(m_matrix3D);
        }

        // 转换相机空间 Frustum 八个顶点到世界空间
        override public void convCornerView2World(Matrix4x4 viewMat)
        {
            Matrix4x4 eyeToWorld = viewMat.inverse;
            int index = 0;
            Vector4 pos = Vector4.zero;
            while(index < 8)
            {
                pos = new Vector4(m_frustumCorners[index * 3 + 0], m_frustumCorners[index * 3 + 1], m_frustumCorners[index * 3 + 2], 1);
                pos = eyeToWorld * pos;
                m_frustumCorners[index * 3 + 0] = pos.x;
                m_frustumCorners[index * 3 + 1] = pos.y;
                m_frustumCorners[index * 3 + 2] = pos.z;
                index = index + 1;
            }
        }

        override public void buildPanel()
        {
            MVector3 p0 = new MVector3(m_frustumCorners[0], m_frustumCorners[1], m_frustumCorners[2]);
            MVector3 p1 = new MVector3(m_frustumCorners[12], m_frustumCorners[13], m_frustumCorners[14]);
            MVector3 p2 = new MVector3(m_frustumCorners[9], m_frustumCorners[10], m_frustumCorners[11]);
            m_leftPanel.redefine(ref p0, ref p1, ref p2);
        }
    }
}