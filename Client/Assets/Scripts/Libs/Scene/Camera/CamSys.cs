using UnityEngine;

namespace SDK.Lib
{
    public class CamSys
    {
        public BoxCam m_boxCam;
        public DzCam m_dzCam;

        public UICamera m_uiCam;

        protected MCamera m_camera;        // 这个是系统摄像机，主要进行裁剪使用的

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
    }
}