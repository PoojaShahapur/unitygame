using SDK.Lib;

namespace UnitTestSrc
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