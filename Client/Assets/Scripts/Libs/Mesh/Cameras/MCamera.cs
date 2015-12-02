using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 摄像机，左手系，深度 [-1, 1] 范围
     */
    public class MCamera
    {
        protected Camera m_camera;          // 保存的 Unity 的摄像机
        protected Matrix4x4 m_viewMat;      // View 矩阵
        protected Matrix4x4 m_projMat;      // Proj 矩阵
        protected Matrix4x4 m_viewProjMat;  // view Proj 矩阵
        protected bool m_viewProjDirty;
		protected MLensBase m_lens;         // 镜头
        protected MPlane3D[] m_frustumPlanes;   // 6 个裁剪面板，这个面板是世界空间中的面板，因为计算的时候使用的是 ViewProject 矩阵
        protected bool m_frustumPlanesDirty;

        public MCamera()
        {
            m_viewProjDirty = true;
            m_lens = new MPerspectiveLens();    // 默认透视投影

            m_frustumPlanes = new MPlane3D[6];
            for (int idx = 0; idx < 6; ++idx)
            {
                m_frustumPlanes[idx] = new MPlane3D();
            }

            update();
        }

        protected void update()
        {
            // 更新摄像机的参数信息
            if(null != m_camera)
            {
                m_viewMat = m_camera.worldToCameraMatrix;
                m_projMat = m_camera.projectionMatrix;

                m_lens.setFieldOfView(m_camera.fieldOfView);
                m_lens.setFarDist(m_camera.farClipPlane);
                m_lens.setNearDist(m_camera.nearClipPlane);
                m_lens.setAspectRatio(m_camera.aspect);
                m_lens.setFocalLength(1);

                m_viewProjMat = m_viewMat * m_projMat;
            }
        }

        /**
         * @brief 更新 Frustum 6 个面板
         */
        protected void updateFrustum()
		{
			float a = 0, b = 0, c = 0;
            //var d : Number;
            float c11, c12, c13, c14;
            float c21, c22, c23, c24;
            float c31, c32, c33, c34;
            float c41, c42, c43, c44;
            MPlane3D p;
            float invLen;

            // View Project 矩阵，注意不是只有 Project 矩阵，因此这个求的面板是世界空间中的面板
            c11 = m_viewProjMat.m00;
			c12 = m_viewProjMat.m10;
			c13 = m_viewProjMat.m20;
			c14 = m_viewProjMat.m30;
			c21 = m_viewProjMat.m01;
			c22 = m_viewProjMat.m11;
			c23 = m_viewProjMat.m21;
			c24 = m_viewProjMat.m31;
			c31 = m_viewProjMat.m02;
			c32 = m_viewProjMat.m12;
			c33 = m_viewProjMat.m22;
			c34 = m_viewProjMat.m32;
			c41 = m_viewProjMat.m03;
			c42 = m_viewProjMat.m13;
			c43 = m_viewProjMat.m23;
			c44 = m_viewProjMat.m33;
			
			// 左边 Plane
			p = m_frustumPlanes[0];
			a = c41 + c11;
			b = c42 + c12;
			c = c43 + c13;
			invLen = (float)(1/UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = -(c44 + c14)*invLen;
			
			// 右边 Plane
			p = m_frustumPlanes[1];
			a = c41 - c11;
			b = c42 - c12;
			c = c43 - c13;
			invLen = (float)(1 /UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = (c14 - c44)*invLen;
			
			// 底边 Plane
			p = m_frustumPlanes[2];
			a = c41 + c21;
			b = c42 + c22;
			c = c43 + c23;
			invLen = (float)(1 /UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = -(c44 + c24)*invLen;
			
			// 顶端 Plane
			p = m_frustumPlanes[3];
			a = c41 - c21;
			b = c42 - c22;
			c = c43 - c23;
			invLen = (float)(1 /UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = (c24 - c44)*invLen;
			
			// 近 Plane
			p = m_frustumPlanes[4];
			a = c31;
			b = c32;
			c = c33;
			invLen = (float)(1 /UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = -c34* invLen;

            // 远 Plane
            p = m_frustumPlanes[5];
			a = c41 - c31;
			b = c42 - c32;
			c = c43 - c33;
			invLen = (float)(1 /UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = (c34 - c44)*invLen;
			
			m_frustumPlanesDirty = false;
		}
    }
}