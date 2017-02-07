namespace SDK.Lib
{
    public class DelayTaskMgr : ITickedObject, IDelayHandleItem
    {
	    protected int mFrameInterval;   // ֡���
        protected int mTaskNumPerFrameInterval; // ÿһ��֡���ִ����������
        protected int mCurTaskNum;              // ��ǰִ�е���������
        protected int mPreFrame;    // ֮ǰ֡
        protected int mCurFrame;	// ��ǰ֡

	    MList<IDelayTask> mDelayTaskList;

        public DelayTaskMgr()
        {
            this.mFrameInterval = 1;
            this.mTaskNumPerFrameInterval = 1;
            this.mCurTaskNum = 0;
            this.mPreFrame = 0;
            this.mCurFrame = 0;
            this.mDelayTaskList = new MList<IDelayTask>();
        }

        public void init()
        {

        }

        public void dispose()
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