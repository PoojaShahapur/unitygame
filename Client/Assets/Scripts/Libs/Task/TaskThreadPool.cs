using System.Collections.Generic;

namespace SDK.Lib
{
    public class TaskThreadPool
    {
        protected List<TaskThread> m_list;

        public TaskThreadPool(int numThread, TaskQueue taskQueue)
        {
            m_list = new List<TaskThread>(numThread);
            int idx = 0;
            for(idx = 0; idx < numThread; ++idx)
            {
                m_list[idx] = new TaskThread(string.Format("TaskThread{0}", idx), taskQueue);
                m_list[idx].start();
            }
        }

        public void notifyIdleThread()
        {
            foreach(var item in m_list)
            {
                if(item.notifySelf())       // 如果唤醒某个线程就退出，如果一个都没有唤醒，说明当前线程都比较忙，需要等待
                {
                    break;
                }
            }
        }
    }
}