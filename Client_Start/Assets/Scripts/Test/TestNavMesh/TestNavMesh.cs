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
            Ctx.mInstance.mSceneSys.loadScene("TestNavMesh.unity", onResLoadScene);
        }

        public void onResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
        }
    }
}