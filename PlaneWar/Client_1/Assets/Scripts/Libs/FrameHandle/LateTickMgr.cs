namespace SDK.Lib
{
    /**
     * @brief 对应事件 LateUpdate
     */
    public class LateTickMgr : TickMgr
    {
        public LateTickMgr()
        {

        }

        override protected void onExecAdvance(float delta, TickMode tickMode)
        {
            base.onExecAdvance(delta, tickMode);
        }
    }
}