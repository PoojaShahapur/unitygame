namespace SDK.Lib
{
    /**
     * @brief 任务线程
     */
    public class TaskThread : MThread
    {
        protected TaskQueue mTaskQueue;
        protected MCondition mCondition;
        protected ITask mCurTask;

        public TaskThread(string name, TaskQueue taskQueue)
            : base(null, null)
        {
            mTaskQueue = taskQueue;
            mCondition = new MCondition(name);
        }

        /**
         *brief 线程回调函数
         */
        override public void threadHandle()
        {
            while (!m_ExitFlag)
            {
                mCurTask = mTaskQueue.pop();
                if(mCurTask != default(ITask))
                {
                    mCurTask.runTask();
                }
                else
                {
                    mCondition.wait();
                }
            }
        }

        public bool notifySelf()
        {
            if(mCondition.canEnterWait)
            {
                mCondition.notifyAll();
                return true;
            }

            return false;
        }
    }
}