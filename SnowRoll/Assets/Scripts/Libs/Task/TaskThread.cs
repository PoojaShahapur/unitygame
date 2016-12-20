namespace SDK.Lib
{
    /**
     * @brief 任务线程
     */
    public class TaskThread : MThread
    {
        protected TaskQueue m_taskQueue;
        protected MCondition m_condition;
        protected ITask m_curTask;

        public TaskThread(string name, TaskQueue taskQueue)
            : base(null, null)
        {
            m_taskQueue = taskQueue;
            m_condition = new MCondition(name);
        }

        /**
         *brief 线程回调函数
         */
        override public void threadHandle()
        {
            while (!m_ExitFlag)
            {
                m_curTask = m_taskQueue.pop();
                if(m_curTask != default(ITask))
                {
                    m_curTask.runTask();
                }
                else
                {
                    m_condition.wait();
                }
            }
        }

        public bool notifySelf()
        {
            if(m_condition.canEnterWait)
            {
                m_condition.notifyAll();
                return true;
            }

            return false;
        }
    }
}