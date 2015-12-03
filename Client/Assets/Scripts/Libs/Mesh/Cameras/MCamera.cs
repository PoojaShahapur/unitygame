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

        public MCamera(Camera camera_ = null)
        {
            m_viewProjDirty = true;
            m_lens = new MPerspectiveLens();    // 默认透视投影

            m_frustumPlanes = new MPlane3D[6];
            for (int idx = 0; idx < 6; ++idx)
            {
                m_frustumPlanes[idx] = new MPlane3D();
            }

            setCamera(camera_);
        }

        public void setCamera(Camera camera_ = null)
        {
            m_camera = camera_;
            update();
        }

        protected void update()
        {
            // 更新摄像机的参数信息，真正的摄像机的设置
            // fov = 60
            // near = 10
            // far = 1000
            if(null != m_camera)
            {
                m_viewMat = m_camera.worldToCameraMatrix;
                m_projMat = m_camera.projectionMatrix;

                m_lens.setFieldOfView(m_camera.fieldOfView);
                m_lens.setFarDist(m_camera.farClipPlane);
                m_lens.setNearDist(m_camera.nearClipPlane);
                m_lens.setAspectRatio(m_camera.aspect);

                m_viewProjMat = m_viewMat * m_projMat;
                testLogMatrix(m_projMat);

                m_lens.updateMatrix();
            }
        }

        /**
         * @brief 更新 Frustum 6 个面板
         */
        protected void updateFrustum()
		{
			float a = 0, b = 0, c = 0;
            MPlane3D p;
            float invLen;

            // View Project 矩阵，注意不是只有 Project 矩阵，因此这个求的面板是世界空间中的面板
			// 左边 Plane
			p = m_frustumPlanes[0];
			a = m_viewProjMat.m03 + m_viewProjMat.m00;
			b = m_viewProjMat.m13 + m_viewProjMat.m10;
			c = m_viewProjMat.m23 + m_viewProjMat.m20;
			invLen = (float)(1/UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = -(m_viewProjMat.m33 + m_viewProjMat.m30) *invLen;
			
			// 右边 Plane
			p = m_frustumPlanes[1];
			a = m_viewProjMat.m03 - m_viewProjMat.m00;
			b = m_viewProjMat.m13 - m_viewProjMat.m10;
			c = m_viewProjMat.m23 - m_viewProjMat.m20;
			invLen = (float)(1 / UtilApi.Sqrt(a * a + b * b + c * c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = (m_viewProjMat.m30 - m_viewProjMat.m33) * invLen;
			
			// 底边 Plane
			p = m_frustumPlanes[2];
			a = m_viewProjMat.m32 + m_viewProjMat.m01;
			b = m_viewProjMat.m13 + m_viewProjMat.m11;
			c = m_viewProjMat.m23 + m_viewProjMat.m21;
			invLen = (float)(1 / UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = -(m_viewProjMat.m33 + m_viewProjMat.m31) *invLen;
			
			// 顶端 Plane
			p = m_frustumPlanes[3];
			a = m_viewProjMat.m03 - m_viewProjMat.m01;
			b = m_viewProjMat.m13 - m_viewProjMat.m11;
			c = m_viewProjMat.m23 - m_viewProjMat.m21;
			invLen = (float)(1 / UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = (m_viewProjMat.m31 - m_viewProjMat.m33) *invLen;
			
			// 近 Plane
			p = m_frustumPlanes[4];
			a = m_viewProjMat.m02;
			b = m_viewProjMat.m12;
			c = m_viewProjMat.m22;
			invLen = (float)(1 / UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = -m_viewProjMat.m32 * invLen;

            // 远 Plane
            p = m_frustumPlanes[5];
			a = m_viewProjMat.m03 - m_viewProjMat.m02;
			b = m_viewProjMat.m13 - m_viewProjMat.m12;
			c = m_viewProjMat.m23 - m_viewProjMat.m22;
			invLen = (float)(1 / UtilApi.Sqrt(a* a + b* b + c* c));
            p.m_a = a* invLen;
            p.m_b = b* invLen;
            p.m_c = c* invLen;
            p.m_d = (m_viewProjMat.m23 - m_viewProjMat.m33) *invLen;
			
			m_frustumPlanesDirty = false;
		}

        /**
         * brief 测试输出投影矩阵
         */
        public void testLogMatrix(Matrix4x4 projMat)
        {
            string str = "";
            for(int y = 0; y < 4; ++y)
            {
                for(int x = 0; x < 4; ++x)
                {
                    str += string.Format("m{0}{1} = ", y, x);
                    str += string.Format("{0} ,", projMat[y, x]);
                }
            }

            Debug.Log(str);
        }
    }
}