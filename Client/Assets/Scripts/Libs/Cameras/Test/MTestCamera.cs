﻿using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 摄像机，左手系，深度 [-1, 1] 范围
     */
    public class MTestCamera
    {
        protected Camera m_camera;          // 保存的 Unity 的摄像机
        protected Matrix4x4 m_viewMat;      // View 矩阵
        protected Matrix4x4 m_projMat;      // Proj 矩阵
        protected Matrix4x4 m_viewProjMat;  // view Proj 矩阵
        protected bool m_viewProjDirty;     // view Proj 矩阵是否无效
        protected MLensBase m_lens;         // 镜头
        protected MList<MPlane> m_frustumPlanes;   // 6 个裁剪面板，这个面板是世界空间中的面板，因为计算的时候使用的是 ViewProject 矩阵
        protected bool m_frustumPlanesDirty;    // FrustumPlane 是否无效

        protected MCamera m_localCamera;

        public MTestCamera(Camera camera_ = null)
        {
            m_viewProjDirty = true;
            m_lens = new MPerspectiveLens();    // 默认透视投影

            m_frustumPlanes = new MList<MPlane>(6);
            for (int idx = 0; idx < 6; ++idx)
            {
                m_frustumPlanes.Add(new MPlane());
            }

            setCamera(camera_);
        }

        /**
         * @brief 返回 Frustum 的六个面板
         */
        public MList<MPlane> getFrustumPlanes()
        {
            return m_frustumPlanes;
        }

        public void setCamera(Camera camera_ = null)
        {
            m_camera = camera_;
            update();
        }

        public void update()
        {
            // 更新摄像机的参数信息，真正的摄像机的设置
            // fov = 60
            // near = 10
            // far = 1000
            if(false && null != m_camera)
            {
                m_viewMat = m_camera.worldToCameraMatrix;
                m_projMat = m_camera.projectionMatrix;

                m_lens.setFieldOfView(m_camera.fieldOfView);
                m_lens.setFarDist(m_camera.farClipPlane);
                m_lens.setNearDist(m_camera.nearClipPlane);
                m_lens.setAspectRatio(m_camera.aspect);

                m_viewProjMat = m_projMat * m_viewMat;
                //testLogMatrix(m_projMat);

                updateFrustum_A();
                //updateFrustum_B();
                m_lens.updateMatrix();
                m_lens.convCornerView2World(m_viewMat);
                m_lens.buildPanel();
                if (m_lens.getShowBoundBox())
                {
                    m_lens.updateFrustumRender();
                }
            }

            //testLocalCamera();
            testLocalUpdate();
        }

        /**
         * @brief 更新 Frustum 6 个面板
         */
        protected void updateFrustum_A()
		{
			float a = 0, b = 0, c = 0;
            MPlane p;
            float invLen;

            // View Project 矩阵，注意不是只有 Project 矩阵，因此这个求的面板是世界空间中的面板
			// 左边 Plane
			p = m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_LEFT];
			a = m_viewProjMat.m30 + m_viewProjMat.m00;
			b = m_viewProjMat.m31 + m_viewProjMat.m01;
			c = m_viewProjMat.m32 + m_viewProjMat.m02;
			invLen = (float)(1/ UtilMath.Sqrt(a* a + b* b + c* c));
            p.normal.x = a* invLen;
            p.normal.y = b* invLen;
            p.normal.z = c* invLen;
            p.d = (m_viewProjMat.m33 + m_viewProjMat.m03) *invLen;
            m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_LEFT] = p;

            // 右边 Plane
            p = m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_RIGHT];
			a = m_viewProjMat.m30 - m_viewProjMat.m00;
			b = m_viewProjMat.m31 - m_viewProjMat.m01;
			c = m_viewProjMat.m32 - m_viewProjMat.m02;
			invLen = (float)(1 / UtilMath.Sqrt(a * a + b * b + c * c));
            p.normal.x = a* invLen;
            p.normal.y = b* invLen;
            p.normal.z = c* invLen;
            p.d = (m_viewProjMat.m33 - m_viewProjMat.m03) * invLen;
            m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_RIGHT] = p;

            // 顶端 Plane
            p = m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_TOP];
			a = m_viewProjMat.m30 - m_viewProjMat.m10;
			b = m_viewProjMat.m31 - m_viewProjMat.m11;
			c = m_viewProjMat.m32 - m_viewProjMat.m12;
			invLen = (float)(1 / UtilMath.Sqrt(a * a + b * b + c * c));
            p.normal.x = a * invLen;
            p.normal.y = b * invLen;
            p.normal.z = c * invLen;
            p.d = (m_viewProjMat.m33 - m_viewProjMat.m13) *invLen;
            m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_TOP] = p;

            // 底边 Plane
            p = m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_BOTTOM];
            a = m_viewProjMat.m30 + m_viewProjMat.m10;
            b = m_viewProjMat.m31 + m_viewProjMat.m11;
            c = m_viewProjMat.m32 + m_viewProjMat.m12;
            invLen = (float)(1 / UtilMath.Sqrt(a * a + b * b + c * c));
            p.normal.x = a * invLen;
            p.normal.y = b * invLen;
            p.normal.z = c * invLen;
            p.d = (m_viewProjMat.m33 + m_viewProjMat.m13) * invLen;
            m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_BOTTOM] = p;

            // 近 Plane
            p = m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_NEAR];
			a = m_viewProjMat.m30 + m_viewProjMat.m20;
			b = m_viewProjMat.m31 + m_viewProjMat.m21;
			c = m_viewProjMat.m32 + m_viewProjMat.m22;
			invLen = (float)(1 / UtilMath.Sqrt(a * a + b * b + c * c));
            p.normal.x = a * invLen;
            p.normal.y = b * invLen;
            p.normal.z = c * invLen;
            p.d = (m_viewProjMat.m33 + m_viewProjMat.m23) * invLen;
            m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_NEAR] = p;

            // 远 Plane
            p = m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_FAR];
			a = m_viewProjMat.m30 - m_viewProjMat.m20;
			b = m_viewProjMat.m31 - m_viewProjMat.m21;
			c = m_viewProjMat.m32 - m_viewProjMat.m22;
			invLen = (float)(1 / UtilMath.Sqrt(a * a + b * b + c * c));
            p.normal.x = a * invLen;
            p.normal.y = b * invLen;
            p.normal.z = c * invLen;
            p.d = (m_viewProjMat.m33 - m_viewProjMat.m23) *invLen;
            m_frustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_FAR] = p;

            m_frustumPlanesDirty = false;
        }

        protected void updateFrustum_B()
        {
            MPlane p;
            Vector4 pnl = new Vector4();

            // View Project 矩阵，注意不是只有 Project 矩阵，因此这个求的面板是世界空间中的面板
            // 左边 Plane
            p = m_frustumPlanes[0];
            // 摄像机空间
            pnl.x = m_projMat.m30 + m_projMat.m00;
            pnl.y = m_projMat.m31 + m_projMat.m01;
            pnl.z = m_projMat.m32 + m_projMat.m02;
            pnl.w = m_projMat.m33 + m_projMat.m03;
            // 世界空间
            pnl = m_camera.cameraToWorldMatrix * pnl;
            // 单位化
            pnl.Normalize();
            p.normal.x = pnl.x;
            p.normal.y = pnl.y;
            p.normal.z = pnl.z;
            p.d = pnl.w;

            // 右边 Plane
            p = m_frustumPlanes[1];
            pnl.x = m_projMat.m30 - m_projMat.m00;
            pnl.y = m_projMat.m31 - m_projMat.m01;
            pnl.z = m_projMat.m32 - m_projMat.m02;
            pnl.w = m_projMat.m33 - m_projMat.m03;
            pnl = m_camera.cameraToWorldMatrix * pnl;
            pnl.Normalize();
            p.normal.x = pnl.x;
            p.normal.y = pnl.y;
            p.normal.z = pnl.z;
            p.d = pnl.w;

            // 顶端 Plane
            p = m_frustumPlanes[3];
            pnl.x = m_projMat.m30 - m_projMat.m10;
            pnl.y = m_projMat.m31 - m_projMat.m11;
            pnl.z = m_projMat.m32 - m_projMat.m12;
            pnl.w = m_projMat.m33 - m_projMat.m13;
            pnl = m_camera.cameraToWorldMatrix * pnl;
            pnl.Normalize();
            p.normal.x = pnl.x;
            p.normal.y = pnl.y;
            p.normal.z = pnl.z;
            p.d = pnl.w;

            // 底边 Plane
            p = m_frustumPlanes[2];
            pnl.x = m_projMat.m30 + m_projMat.m10;
            pnl.y = m_projMat.m31 + m_projMat.m11;
            pnl.z = m_projMat.m32 + m_projMat.m12;
            pnl.w = m_projMat.m33 + m_projMat.m13;
            pnl = m_camera.cameraToWorldMatrix * pnl;
            pnl.Normalize();
            p.normal.x = pnl.x;
            p.normal.y = pnl.y;
            p.normal.z = pnl.z;
            p.d = pnl.w;

            // 近 Plane
            p = m_frustumPlanes[4];
            pnl.x = m_projMat.m30 + m_projMat.m20;
            pnl.y = m_projMat.m31 + m_projMat.m21;
            pnl.z = m_projMat.m32 + m_projMat.m22;
            pnl.w = m_projMat.m33 + m_projMat.m23;
            pnl = m_camera.cameraToWorldMatrix * pnl;
            pnl.Normalize();
            p.normal.x = pnl.x;
            p.normal.y = pnl.y;
            p.normal.z = pnl.z;
            p.d = pnl.w;

            // 远 Plane
            p = m_frustumPlanes[5];
            pnl.x = m_projMat.m30 - m_projMat.m20;
            pnl.y = m_projMat.m31 - m_projMat.m21;
            pnl.z = m_projMat.m32 - m_projMat.m22;
            pnl.w = m_projMat.m33 - m_projMat.m23;
            pnl = m_camera.cameraToWorldMatrix * pnl;
            pnl.Normalize();
            p.normal.x = pnl.x;
            p.normal.y = pnl.y;
            p.normal.z = pnl.z;
            p.d = pnl.w;

            m_frustumPlanesDirty = false;
        }

        public bool isVisible(MVector3 vert)
        {
            for (int plane = 0; plane < 6; ++plane)
            {
                // Skip far plane if infinite view frustum
                if (plane == (int)MLensBase.FrustumPlane.FRUSTUM_PLANE_FAR && m_lens.getNearDist() == 0)
                    continue;

                if (m_frustumPlanes[plane].getSide(ref vert) == MPlane.Side.NEGATIVE_SIDE)
                {
                    return false;
                }

            }

            return true;
        }

        public string getFrustumPlanesStr()
        {
            int idx = 0;
            string ret = "";
            while (idx < 6)
            {
                ret += m_frustumPlanes[idx].ToString();
                ++idx;
            }

            return ret;
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

        protected string testOutClipPanel()
        {
            string ret = getFrustumPlanesStr();
            return ret;
        }

        public void testLocalCamera()
        {
            if (null != m_camera)
            {
                MCamera camera = new MCamera(m_camera.gameObject.transform);
                camera.setFOVy(new MRadian(UtilMath.DegreesToRadians(m_camera.fieldOfView)));
                camera.setFarClipDistance(m_camera.farClipPlane);
                camera.setNearClipDistance(m_camera.nearClipPlane);
                camera.setAspectRatio(m_camera.aspect);

                MMatrix4 viewMatrix = camera.getViewMatrix();
                MMatrix4 projMatrix = camera.getProjectionMatrix();

                MPlane[] planes = camera.getFrustumPlanes();
                string planeStr = "";
                planeStr = testOutClipPanel();
                planeStr = camera.getFrustumPlanesStr();
                MVector3[] worldCorners = camera.getWorldSpaceCorners();

                string worldCornerStr = "";
                worldCornerStr = m_lens.getWorldCornerStr();
                worldCornerStr = camera.getWorldCornerStr();

                Debug.Log("aaaa");
            }
        }

        protected void testLocalUpdate()
        {
            if (null != m_camera)
            {
                if (m_localCamera == null)
                {
                    m_localCamera = new MCamera(m_camera.gameObject.transform);

                    m_localCamera.setFOVy(new MRadian(UtilMath.DegreesToRadians(m_camera.fieldOfView)));
                    m_localCamera.setFarClipDistance(m_camera.farClipPlane);
                    m_localCamera.setNearClipDistance(m_camera.nearClipPlane);
                    m_localCamera.setAspectRatio(m_camera.aspect);
                }
                m_localCamera.updateVertexData();
            }
        }
    }
}