namespace SDK.Lib
{
    /**
     * @brief 全局委托
     */
    public class GlobalDelegate
    {
        // PlayerMainChild 的质量发生改变
        public AddOnceEventDispatch mMainChildMassChangedDispatch;
        // Camera 相机方向或者位置发生改变
        public AddOnceEventDispatch mCameraOrientChanged;

        public GlobalDelegate()
        {
            this.mMainChildMassChangedDispatch = new AddOnceEventDispatch();
            this.mCameraOrientChanged = new AddOnceEventDispatch();
        }

        public void init()
        {

        }

        public void dispose()
        {

        }
    }
}