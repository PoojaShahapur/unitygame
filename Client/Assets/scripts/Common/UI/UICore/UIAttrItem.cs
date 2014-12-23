namespace SDK.Common
{
    public class UIAttrItem
    {
        public UILayerID m_LayerID;

        public string m_codePath;          // 逻辑代码 path 
        public string m_widgetPath;            // 拖放的控件 path 
        public string m_codePrefabName;         // bundle 中逻辑的名字
        public string m_widgetPrefabName;         // bundle 中资源的名字
    }
}