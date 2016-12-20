using SDK.Lib;

namespace UnitTest
{
    public class TestVersion
    {
        public void run()
        {
            testVersion();
        }

        protected void testVersion()
        {
            //Ctx.mInstance.mVersionSys.loadVerFile();
            Ctx.mInstance.mVersionSys.loadMiniVerFile();
        }
    }
}