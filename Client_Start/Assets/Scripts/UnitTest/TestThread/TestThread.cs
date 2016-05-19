﻿using Game.Game;
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
            Ctx.m_instance.m_logSys.log("aaaaa");
        }

        protected void testThreadTask()
        {
            GameRouteCB m_gameRouteCB = new GameRouteCB();
            Ctx.m_instance.m_msgRouteNotify.addOneDisp(m_gameRouteCB);
        }
    }
}