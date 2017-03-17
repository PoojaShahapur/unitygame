namespace SDK.Lib
{
    /**
     * @brief 心跳管理器
     */
    public class TickMgr : TickObjectPriorityMgr
    {
        public TickMgr()
        {
            
        }

        override public void init()
        {
            base.init();
        }

        override public void dispose()
        {
            base.dispose();
        }

        public void addTick(ITickedObject tickObj, float priority = 0.0f)
        {
            this.addObject(tickObj as IDelayHandleItem, priority);
        }

        public void removeTick(ITickedObject tickObj)
        {
            this.removeObject(tickObj as IDelayHandleItem);
        }
    }
}