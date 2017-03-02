namespace SDK.Lib
{
    // 加载进度
    public class LoadProgressMgr : TickObjectMgrBase
    {
        public LoadProgressMgr()
        {

        }

        public void addProgress(LoadItem item)
        {
            this.addObject(item);
        }

        public void removeProgress(LoadItem item)
        {
            this.removeObject(item);
        }
    }
}