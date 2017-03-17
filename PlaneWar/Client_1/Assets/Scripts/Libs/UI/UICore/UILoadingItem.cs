namespace SDK.Lib
{
    /**
     * @brief 正在加载的 Item ，主要是防止重复加载
     */
    public class UILoadingItem
    {
        public UIFormId mId;               // Form ID
        public string mResPath;            // 拖放的控件 path 
    }
}