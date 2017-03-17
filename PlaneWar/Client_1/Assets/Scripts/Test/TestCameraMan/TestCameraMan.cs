using SDK.Lib;
using UnityEngine;

namespace UnitTest
{
    public class TestCameraMan
    {
        public void run()
        {
            test();
        }

        public void test()
        {
            Ctx.mInstance.mSceneSys.loadScene("TestCameraControl.unity", onResLoadScene);
        }

        public void onResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            GameObject camera = UtilApi.GoFindChildByName("MainCamera");
            GameObject man = UtilApi.GoFindChildByName("Cube");
            Ctx.mInstance.mCamSys.setMainCamera(camera.GetComponent<Camera>());
            Ctx.mInstance.mCamSys.setCameraActor(man);
        }
    }
}