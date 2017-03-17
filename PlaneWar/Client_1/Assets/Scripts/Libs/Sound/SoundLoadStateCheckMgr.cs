namespace SDK.Lib
{
    /**
     * @brief 声音加载状态检查管理器
     */
    public class SoundLoadStateCheckMgr : TickObjectNoPriorityMgr
    {
        public SoundLoadStateCheckMgr()
        {

        }

        override public void init()
        {

        }

        override public void dispose()
        {

        }

        public void addSound(SoundItem sound)
        {
            this.addObject(sound);
        }

        public void removeSound(SoundItem sound)
        {
            this.removeObject(sound);
        }
    }
}