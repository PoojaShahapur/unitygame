﻿using SDK.Lib;

#if UNIT_TEST
using UnitTest;
using UnityEngine;
#endif

namespace Game.Game
{
    public class GameSceneEventCB : ISceneEventCB
    {
        // 场景加载完成处理事件
        public void onLevelLoaded()
        {
            Ctx.mInstance.mLuaSystem.onSceneLoaded();
            // 关联相机
            GameObject cam = UtilApi.GoFindChildByName("MainCamera").gameObject;
            Ctx.mInstance.mCamSys.setMainCamera(cam.GetComponent<Camera>());
            // 创建主角
            Ctx.mInstance.mPlayerMgr.createPlayer();
            runTest();
        }

        protected void runTest()
        {
            // 运行单元测试
#if UNIT_TEST
            TestMain pTestMain = new TestMain();
            pTestMain.run();
#endif
            //Ctx.mInstance.mNetMgr.openSocket("106.14.32.169", 20013);
        }

        protected void init()
        {

        }
    }
}