namespace SDK.Lib
{
    public class DelayTaskMgr : ITickedObject
    {
	    protected int mFrameInterval;   // 帧间隔
        protected int mTaskNumPerFrameInterval; // 每一个帧间隔执行任务数量
        protected int mCurTaskNum;              // 当前执行的任务数量
        protected int mPreFrame;    // 之前帧
        protected int mCurFrame;	// 当前帧

	    MList<IDelayTask> mDelayTaskList;

        public DelayTaskMgr()
        {
            this.mFrameInterval = 1;
            this.mTaskNumPerFrameInterval = 1;
            this.mCurTaskNum = 0;
            this.mPreFrame = 0;
            this.mCurFrame = 0;
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public virtual void onTick(float delta)
        {
            this.mCurFrame = (int)Ctx.mInstance.mSystemFrameData.getTotalFrameCount();

            if (this.mPreFrame + this.mFrameInterval <= this.mCurFrame)
            {
                this.mPreFrame = this.mCurFrame;
                this.mCurTaskNum = 0;

                this.execTask();
            }
        }

        public void addTask(IDelayTask task)
        {
            if (this.mCurTaskNum < this.mTaskNumPerFrameInterval)
            {
                ++this.mCurTaskNum;
                task.delayExec();
            }
            else
            {
                this.mDelayTaskList.push(task);
            }
        }

        public void execTask()
        {
            IDelayTask task = null;

            while (this.mCurTaskNum < this.mTaskNumPerFrameInterval)
            {
                if (this.mDelayTaskList.Count() > 0)
                {
                    task = this.mDelayTaskList[0];
                    this.mDelayTaskList.RemoveAt(0);
                    task.delayExec();
                }

                ++this.mCurTaskNum;
            }
        }
    }
}