using Game.Game;
using SDK.Common;
using SDK.Lib;

namespace UnitTestSrc
{
    public class ThreadTest
    {
        public void run()
        {
            //testEvent();
            testThreadTask();
        }

        protected void testEvent()
        {
            MEvent pMEvent = new MEvent(false);
            pMEvent.WaitOne();
            Ctx.m_instance.m_logSys.log("aaaaa");
        }

        protected void testThreadTask()
        {
            GameRouteCB m_gameRouteCB = new GameRouteCB();
            Ctx.m_instance.m_msgRouteList.addOneDisp(m_gameRouteCB);
        }
    }
}