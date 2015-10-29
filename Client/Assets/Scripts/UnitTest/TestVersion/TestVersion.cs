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
            //Ctx.m_instance.m_versionSys.loadVerFile();
            Ctx.m_instance.m_versionSys.loadMiniVerFile();
        }
    }
}