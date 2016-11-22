using SDK.Lib;
using UnityEngine;

namespace UnitTest
{
    public class CoroutineTaskOut : CoroutineTaskBase
    {
        override public void run()
        {
            //base.run();
            for (int idx = 0; idx < 10; ++idx)
            {
                Debug.Log("CoroutineTaskOut::CoroutineTaskOut");
            }
            //Ctx.mInstance.mCoroutineTaskMgr.stop();
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
            task.setNeedRemove(false);
            Ctx.mInstance.mCoroutineTaskMgr.addTask(task);
        }
    }
}