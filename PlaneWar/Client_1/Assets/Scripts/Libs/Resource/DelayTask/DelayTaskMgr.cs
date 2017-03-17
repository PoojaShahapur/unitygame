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

        public virtual void onTick(float delta)
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
            this.mPriorityList.removePriorityObject(task as INoOrPriorityObject);
        }

        public void execTask()
        {
            IDelayTask task = null;

            while (this.mNumInterval.canExec(1))
            {
                if (this.mPriorityList.Count() > 0)
                {
                    task = this.mPriorityList.get(0) as IDelayTask;
                    this.mPriorityList.RemoveAt(0);
                    task.delayExec();
                }
            }

            this.mNumInterval.reset();
        }
    }
}