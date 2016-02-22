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
            GameObject camera = UtilApi.GoFindChildByName("MainCamera");
        }
    }
}