namespace SDK.Lib
{
    public class DelayTaskMgr : DelayPriorityHandleMgr, IDelayHandleItem, ITickedObject, INoOrPriorityObject
    {
        protected NumInterval mNumInterval;
        protected FrameInterval mFrameInterval; // Ö¡¼ä¸ô

        public DelayTaskMgr()
        {
            this.mNumInterval = new NumInterval();
            this.mNumInterval.setTotalValue(1);

            this.mFrameInterval = new FrameInterval();
            this.mFrameInterval.setInterval(1);
        }

        override public void init()
        {

        }

        override public void dispose()
        {

        }

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        public virtual void onTick(float delta, TickMode tickMode)
        {
            if (this.mFrameInterval.canExec(1))
            {
                this.execTask();
            }
        }

        public void addTask(IDelayTask task, float priority = 0.0f)
        {
            this.addPriorityObject(task as INoOrPriorityObject, priority);

            this.execTask();
        }

        public void removeTask(IDelayTask task)
        {
            this.mNoOrPriorityList.removeNoOrPriorityObject(task as INoOrPriorityObject);
        }

        public void execTask()
        {
            IDelayTask task = null;

            while (this.mNumInterval.canExec(1))
            {
                if (this.mNoOrPriorityList.Count() > 0)
                {
                    task = this.mNoOrPriorityList.get(0) as IDelayTask;
                    this.mNoOrPriorityList.RemoveAt(0);
                    task.delayExec();
                }
            }

            this.mNumInterval.reset();
        }
    }
}