using UnityEngine;

namespace SDK.Lib
{
    public class CamSys
    {
        public BoxCam m_boxCam;
        public DzCam m_dzCam;

        public UICamera m_uiCam;            // 这个不是 UI 相机，这个是场景相机

        protected MCamera m_camera;         // 这个是系统摄像机，主要进行裁剪使用的
        protected Camera m_mainCamera;          // 主相机
        protected Camera m_uguiCam;             // UGUI 相机
        protected ThirdCameraController m_cameraController; // 摄像机控制器
        protected CameraMan m_cameraMan;        // 摄像机玩家

        public CamSys()
        {
            m_camera = new MCamera();
        }

        public MCamera getCamera()
        {
            return m_camera;
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
                m_cameraMan = new CameraMan(go);
                m_cameraMan.setCameraController(m_cameraController);
            }
            else
            {
                m_cameraMan.setActor(go);
            }
        }
    }
}