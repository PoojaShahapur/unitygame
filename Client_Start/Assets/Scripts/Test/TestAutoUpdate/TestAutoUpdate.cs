using SDK.Lib;

namespace UnitTest
{
    public class TestAutoUpdate
    {
        public void run()
        {
            testAutoUpdate();
        }

        protected void testAutoUpdate()
        {
            Ctx.mInstance.mAutoUpdateSys.startUpdate();
        }
    }
}