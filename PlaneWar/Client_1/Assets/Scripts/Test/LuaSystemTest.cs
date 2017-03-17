using SDK.Lib;

namespace UnitTest
{
    public class LuaSystemTest
    {
        public void testSendMsg()
        {
            Ctx.mInstance.mLuaSystem.callLuaFunction("GlobalNS.GlobalEventCmdTest.testSendMsg");
        }
    }
}