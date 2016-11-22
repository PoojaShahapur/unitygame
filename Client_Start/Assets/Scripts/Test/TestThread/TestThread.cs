using Game.Game;
using SDK.Lib;

namespace UnitTest
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
            Ctx.mInstance.mLogSys.log("aaaaa");
        }

        protected void testThreadTask()
        {
            GameRouteCB m_gameRouteCB = new GameRouteCB();
            Ctx.mInstance.mMsgRouteNotify.addOneDisp(m_gameRouteCB);
        }
    }
}