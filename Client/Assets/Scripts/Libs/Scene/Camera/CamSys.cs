using UnityEngine;

namespace SDK.Lib
{
    public class CamSys
    {
        public BoxCam m_boxCam;
        public DzCam m_dzCam;

        public UICamera m_uiCam;            // 这个不是 UI 相机，这个是场景相机

        protected MCamera m_localCamera;         // 这个是系统摄像机，主要进行裁剪使用的
        protected Camera m_mainCamera;          // 主相机
        protected Camera m_uguiCam;             // UGUI 相机
        protected ThirdCameraController m_cameraController; // 摄像机控制器
        protected CameraMan m_cameraMan;        // 摄像机玩家

        public CamSys()
        {
            
        }

        public MCamera getLocalCamera()
        {
            return m_localCamera;
        }

        public void setLocalCamera(Camera cam)
        {
            //m_localCamera = new MCamera(cam.gameObject.transform);
            m_localCamera = new MOctreeCamera("OctreeCamera", Ctx.m_instance.m_sceneManager, cam.gameObject.transform);
            if (cam.orthographic)
            {
                m_localCamera.setProjectionType(ProjectionType.PT_ORTHOGRAPHIC);
                m_localCamera.setOrthoWindow(cam.orthographicSize * 2, cam.orthographicSize * 2);
            }
            else
            {
                m_localCamera.setProjectionType(ProjectionType.PT_PERSPECTIVE);
                m_localCamera.setFOVy(new MRadian(UtilMath.DegreesToRadians(cam.fieldOfView)));
                m_localCamera.setFarClipDistance(cam.farClipPlane);
                m_localCamera.setNearClipDistance(cam.nearClipPlane);
                m_localCamera.setAspectRatio(cam.aspect);

                m_localCamera.testClipPlane();
                testAABB();
            }

            //testCameraCull();
            //testFrustumDir();
        }

        public void setSceneCamera2UICamera()
        {
            m_uiCam.mCam = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICamera].GetComponent<Camera>();
        }

        public void setSceneCamera2MainCamera()
        {
            m_uiCam.mCam = null;
        }

        public Camera getMainCamera()
        {
            return m_mainCamera;
        }

        public void setMainCamera(Camera camera)
        {
            m_mainCamera = camera;
        }

        public Camera getUGuiCamera()
        {
            return m_uguiCam;
        }

        public void setUGuiCamera(Camera camera)
        {
            m_uguiCam = camera;
        }

        // 设置摄像机 Man Actor
        public void setCameraActor(GameObject go)
        {
            if (m_cameraController == null)
            {
                m_cameraController = new ThirdCameraController(m_mainCamera, go);
            }
            else
            {
                m_cameraController.setTarget(go);
            }

            if (m_cameraMan == null)
            {
                //m_cameraMan = new CameraMan(go);
                m_cameraMan = new TerrainCameraMan(go);
                m_cameraMan.setCameraController(m_cameraController);
            }
            else
            {
                m_cameraMan.setActor(go);
            }
        }

        public MPlane[] getFrustumPlanes()
        {
            return m_localCamera.getFrustumPlanes();
        }

        public void invalidCamera()
        {
            if (m_localCamera != null)
            {
                m_localCamera.invalid();
                if (Ctx.m_instance.mTerrainGlobalOption.mNeedCull)
                {
                    //Ctx.m_instance.m_terrainGroup.cullTerrain(0, 0, Ctx.m_instance.m_camSys.getLocalCamera());
                    Ctx.m_instance.m_sceneManager._updateSceneGraph(m_localCamera);
                    Ctx.m_instance.m_sceneManager._findVisibleObjects(m_localCamera);
                    //testFrustumDir();
                }
            }
        }

        protected void testAABB()
        {
            MAxisAlignedBox aabb = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            aabb.setMinimum(new MVector3(187.5f, -10, 0));
            aabb.setMaximum(new MVector3(375, 10, 187.5f));

            FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
            if (m_localCamera.isVisible(ref aabb, ref plane))
            {
                Debug.Log("aaaa");
                m_localCamera.isVisible(ref aabb, ref plane);
            }
        }

        protected void testCameraCull()
        {
            UtilApi.setPos(m_localCamera.getTrans(), new Vector3(780, 41, 620));

            MAxisAlignedBox aabb = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            aabb.setMaximum(937.5f, -206.5529f, 1312.5f);
            aabb.setMinimum(750, -270.7412f, 1125);
            FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
            m_localCamera.isVisible(ref aabb, ref plane);
            Debug.Log("aaa");
        }

        public void testFrustumDir()
        {
            Transform trans = m_localCamera.getTrans();
            GameObject cube = UtilApi.TransFindChildByPObjAndPath(trans.gameObject, "Cube");
            MVector3 pos = MVector3.fromNative(cube.transform.position);
            FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
            if(!m_localCamera.isVisible(ref pos, ref plane))
            {
                Debug.Log("Error");
            }
        }
    }
}