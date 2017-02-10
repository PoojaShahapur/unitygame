namespace SDK.Lib
{
    public class DelayTaskMgr : ITickedObject, IDelayHandleItem
    {
	    protected int mFrameInterval;   // ֡���
        protected int mTaskNumPerFrameInterval; // ÿһ��֡���ִ����������
        protected int mCurTaskNum;              // ��ǰִ�е���������
        protected int mPreFrame;    // ֮ǰ֡
        protected int mCurFrame;	// ��ǰ֡

	    protected MList<IDelayTask> mDelayTaskList;
        protected bool mCanExec;    // �Ƿ����ִ������

        public DelayTaskMgr()
        {
            this.mFrameInterval = 1;
            this.mTaskNumPerFrameInterval = 1;
            this.mCurTaskNum = 0;
            this.mPreFrame = 0;
            this.mCurFrame = 0;
            this.mDelayTaskList = new MList<IDelayTask>();
            this.mCanExec = false;
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
                this.mCanExec = true;

                this.execTask();
            }
            else
            {
                this.mCanExec = false;
            }
        }

        public void addTask(IDelayTask task)
        {
            this.mDelayTaskList.push(task);

            this.execTask();
        }

        public void removeTask(IDelayTask task)
        {
            if(-1 != this.mDelayTaskList.IndexOf(task))
            {
                this.mDelayTaskList.Remove(task);
            }
        }

        public void execTask()
        {
            if (this.mCanExec &&
                this.mCurTaskNum < this.mTaskNumPerFrameInterval)
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
}