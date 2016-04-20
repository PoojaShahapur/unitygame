using System.Collections;

namespace SDK.Lib
{
    public class CoroutineTaskMgr
    {
        protected MList<CoroutineTaskBase> mCoroutineTaskList;
        protected eCoroutineTaskState mState;
        protected CoroutineTaskBase mTmp;

        public CoroutineTaskMgr()
        {
            mCoroutineTaskList = new MList<CoroutineTaskBase>();
            mState = eCoroutineTaskState.eStopped;
        }

        public bool isRuning()
        {
            return mState == eCoroutineTaskState.eRunning;
        }

        public bool isPause()
        {
            return mState == eCoroutineTaskState.ePaused;
        }

        public bool isStop()
        {
            return mState == eCoroutineTaskState.eStopped;
        }

        public bool isEmpty()
        {
            return mCoroutineTaskList.length() > 0;
        }

        public void start()
        {
            mState = eCoroutineTaskState.eRunning;
            Ctx.m_instance.m_coroutineMgr.StartCoroutine(run());
        }

        public void addTask(CoroutineTaskBase task)
        {
            mCoroutineTaskList.Add(task);
        }

        protected IEnumerator run()
        {
            while (isRuning())
            {
                if (!isEmpty())
                {
                    if(mCoroutineTaskList[0].isRuning())
                    {
                        mCoroutineTaskList[0].runTask();
                        mCoroutineTaskList[0].handleResult();
                    }
                    if (mCoroutineTaskList[0].isNeedRemove())
                    {
                        mCoroutineTaskList.RemoveAt(0);
                    }
                    else
                    {
                        if(mCoroutineTaskList.length() > 1)
                        {
                            // 放到最后，否则下一次还会先执行
                            mTmp = mCoroutineTaskList[0];
                            mCoroutineTaskList.RemoveAt(0);
                            mCoroutineTaskList.Add(mTmp);
                        }
                    }
                }

                yield return null;
            }

            yield break;
        }
    }
}