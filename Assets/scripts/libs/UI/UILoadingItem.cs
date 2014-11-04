using SDK.Common;
namespace SDK.Lib
{
    /**
     * @brief 正在加载的 Item ，主要是防止重复加载
     */
    public class UILoadingItem
    {
        public UIFormID m_ID;               // Form ID
        public string m_logicPath;          // 逻辑代码 path 
        public string m_resPath;            // 拖放的控件 path 

        public bool m_logicLoaded = false;      // 逻辑代码是否加载完成
        public bool m_resLoaded = false;        // 拖拽的资源是否加载完成

        public bool IsLoaded()
        {
            return m_logicLoaded && m_resLoaded;
        }
    }
}