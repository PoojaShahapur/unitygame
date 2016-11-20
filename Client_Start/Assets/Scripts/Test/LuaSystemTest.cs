using SDK.Lib;

namespace UnitTest
{
    public class LuaSystemTest
    {
        public void testSendMsg()
        {
            Ctx.m_instance.m_luaSystem.callLuaFunction("GlobalNS.GlobalEventCmdTest.testSendMsg");
        }
    }
}