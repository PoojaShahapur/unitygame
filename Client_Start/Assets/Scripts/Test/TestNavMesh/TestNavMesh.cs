using SDK.Lib;

namespace UnitTest
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

        public void onResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
        }
    }
}