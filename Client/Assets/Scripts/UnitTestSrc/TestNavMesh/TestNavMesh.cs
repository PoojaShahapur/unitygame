using Game.UI;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace UnitTestSrc
{
    public class TestNavMesh
    {
        public void run()
        {
            testNavMesh();
        }

        protected void testNavMesh()
        {
            Ctx.m_instance.m_sceneSys.loadScene("TestNavMesh.unity", onResLoadScene);
        }

        public void onResLoadScene(Scene scene)
        {
            // 加载完成场景
            GameObject agentGo = UtilApi.GoFindChildByPObjAndName("RootGo/AgentGo");
            GameObject destGo = UtilApi.GoFindChildByPObjAndName("RootGo/DestGo");
            NavMeshAgent agent = agentGo.GetComponent<NavMeshAgent>();
            agent.destination = destGo.transform.localPosition;

            Ctx.m_instance.m_maze.init();

            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIMaze", typeof(UIMaze));
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIMaze);
        }
    }
}