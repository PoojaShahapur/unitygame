namespace SDK.Lib
{
    /**
     * @brief 触碰集合
     */
    public class MultiTouchSet : IDispatchObject
    {
        protected MList<MTouch> mTouchList;

        public MultiTouchSet()
        {
            this.mTouchList = new MList<MTouch>();
        }

        public void reset()
        {
            this.mTouchList.Clear();
        }

        public void addTouch(MTouch touch)
        {
            this.mTouchList.Add(touch);
        }
    }
}