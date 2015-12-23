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
            Ctx.m_instance.m_sceneSys.loadScene("TestCoordinateConv.unity", onResLoadScene);
        }

        protected void onResLoadScene(Scene scene)
        {
            // 获取主摄像机
            Ctx.m_instance.m_camSys.setMainCamera(UtilApi.GoFindChildByPObjAndName("MainCamera").GetComponent<Camera>());
            m_plane = UtilApi.GoFindChildByPObjAndName("Plane");
            UtilApi.addEventHandle(m_plane, onPlaneClick);
            Ctx.m_instance.m_uiMgr.loadAndShow((UIFormID)100);
        }

        public void onPlaneClick(GameObject go)
        {
            m_currentPos = Ctx.m_instance.m_coordConv.getCurTouchScenePos();
            Vector3 screenPos = UtilApi.convPosFromSceneToUICam(Ctx.m_instance.m_camSys.getMainCamera(), m_currentPos);
            Form form = Ctx.m_instance.m_uiMgr.getForm((UIFormID)100);
            UtilApi.setPos(form.m_GUIWin.m_uiRoot.transform, screenPos);
        }
    }
}