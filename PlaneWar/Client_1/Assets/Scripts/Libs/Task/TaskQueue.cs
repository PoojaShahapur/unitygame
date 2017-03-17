namespace SDK.Lib
{
    public class TaskQueue : LockQueue<ITask>
    {
        public TaskThreadPool mTaskThreadPool;

        public TaskQueue(string name)
            : base(name)
        {

        }

        public new void push(ITask item)
        {
            base.push(item);

            // 检查是否有线程空闲，如果有就唤醒
            mTaskThreadPool.notifyIdleThread();
        }
    }
}