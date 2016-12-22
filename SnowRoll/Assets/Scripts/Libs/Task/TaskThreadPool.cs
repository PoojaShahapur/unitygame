using System.Collections.Generic;

namespace SDK.Lib
{
    public class TaskThreadPool
    {
        protected List<TaskThread> mList;

        public TaskThreadPool()
        {

        }

        public void initThreadPool(int numThread, TaskQueue taskQueue)
        {
            mList = new List<TaskThread>(numThread);
            int idx = 0;
            for(idx = 0; idx < numThread; ++idx)
            {
                mList.Add(new TaskThread(string.Format("TaskThread{0}", idx), taskQueue));
                mList[idx].start();
            }
        }

        public void notifyIdleThread()
        {
            foreach(var item in mList)
            {
                if(item.notifySelf())       // 如果唤醒某个线程就退出，如果一个都没有唤醒，说明当前线程都比较忙，需要等待
                {
                    break;
                }
            }
        }
    }
}