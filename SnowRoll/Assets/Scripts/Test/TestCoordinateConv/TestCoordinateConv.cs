using SDK.Lib;
using UnityEngine;

namespace UnitTest
{
    public class TestCoordinateConv
    {
        protected GameObject m_plane;       // 保存的面板
        protected Vector3 m_currentPos;         // 当前鼠标场景中的位置

        public void run()
        {
            testConv();
        }

        public void testConv()
        {
            Ctx.mInstance.mSceneSys.loadScene("TestCoordinateConv.unity", onResLoadScene);
        }

        protected void onResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            // 获取主摄像机
            Ctx.mInstance.mCamSys.setUGuiCamera(UtilApi.GoFindChildByName("NoDestroy/UICamera").GetComponent<Camera>());
            Ctx.mInstance.mCamSys.setMainCamera(UtilApi.GoFindChildByName("MainCamera").GetComponent<Camera>());
            m_plane = UtilApi.GoFindChildByName("Plane");
            UtilApi.addEventHandle(m_plane, onPlaneClick);
            Ctx.mInstance.mUiMgr.loadAndShow((UIFormId)100);
        }

        public void onPlaneClick(GameObject go)
        {
            m_currentPos = Ctx.mInstance.mCoordConv.getCurTouchScenePos();
            //Vector3 screenPos = UtilApi.convPosFromSceneToUICam(Ctx.mInstance.mCamSys.getMainCamera(), m_currentPos);
            Vector3 screenPos = UtilApi.convPosFromSrcToDestCam(Ctx.mInstance.mCamSys.getMainCamera(), Ctx.mInstance.mCamSys.getUGuiCamera(), m_currentPos);
            Form form = Ctx.mInstance.mUiMgr.getForm((UIFormId)100);
            UtilApi.setRectPos(form.m_guiWin.m_uiRoot.GetComponent<RectTransform>(), screenPos);
        }
    }
}