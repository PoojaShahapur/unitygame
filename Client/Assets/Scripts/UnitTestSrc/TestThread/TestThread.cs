using SDK.Common;
using SDK.Lib;
namespace UnitTestSrc
{
    public class ThreadTest
    {
        public void run()
        {
            //testEvent();
        }

        public void testEvent()
        {
            MEvent pMEvent = new MEvent(false);
            pMEvent.WaitOne();
            Ctx.m_instance.m_log.log("aaaaa");
        }
    }
}