using SDK.Lib;
using UnityEngine;

namespace UnitTest
{
    public class CoroutineTaskOut : CoroutineTaskBase
    {
        override public void runTask()
        {
            Debug.Log("CoroutineTaskOut::CoroutineTaskOut");
        }
    }

    public class TestCoroutineTask
    {
        public void run()
        {
            testTask();
        }

        protected void testTask()
        {
            CoroutineTaskOut task = new CoroutineTaskOut();
            Ctx.m_instance.mCoroutineTaskMgr.addTask(task);
        }
    }
}