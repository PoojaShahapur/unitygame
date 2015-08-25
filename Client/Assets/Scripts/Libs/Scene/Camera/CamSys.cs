using SDK.Lib;
using UnityEngine;

namespace SDK.Lib
{
    public class CamSys
    {
        //public CamEntity m_sceneCam = new CamEntity();
        public BoxCam m_boxCam;
        public DzCam m_dzCam;

        public UICamera m_uiCam;

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