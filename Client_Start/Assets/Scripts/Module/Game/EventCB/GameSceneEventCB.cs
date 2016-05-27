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
            testLoadModel();
            Ctx.m_instance.m_luaSystem.onSceneLoaded();
        }

        protected void testLoadModel()
        {
            // 运行单元测试
#if UNIT_TEST
            UnitTestMain pUnitTestMain = new UnitTestMain();
            pUnitTestMain.run();
#endif
            Ctx.m_instance.m_netMgr.openSocket("192.168.123.71", 8001);
        }
    }
}