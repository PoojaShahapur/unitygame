namespace SDK.Lib
{
    /**
     * @brief 逻辑心跳管理器
     */
    public class LogicTickMgr : TickMgr
    {
        protected TimeInterval mTimeInterval;

        public LogicTickMgr()
        {
            this.mTimeInterval = new TimeInterval();
        }

        override protected void onExecAdvance(float delta, TickMode tickMode)
        {
            if(this.mTimeInterval.canExec(delta))
            {
                base.onExecAdvance(delta, tickMode);
            }
        }
    }
}