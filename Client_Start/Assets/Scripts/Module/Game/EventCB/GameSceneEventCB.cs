using SDK.Lib;

#if UNIT_TEST
using UnitTest;
#endif

namespace Game.Game
{
    public class GameSceneEventCB : ISceneEventCB
    {
        // 场景加载完成处理事件
        public void onLevelLoaded()
        {
            Ctx.m_instance.m_luaSystem.onSceneLoaded();

            runTest();
        }

        protected void runTest()
        {
            // 运行单元测试
#if UNIT_TEST
            TestMain pTestMain = new TestMain();
            pTestMain.run();
#endif
            //Ctx.m_instance.m_netMgr.openSocket("106.14.32.169", 20013);
        }
    }
}