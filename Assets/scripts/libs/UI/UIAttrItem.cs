using SDK.Common;

namespace SDK.Lib
{
    public class UIAttrItem
    {
        public UILayerID m_LayerID;

        public string m_logicPath;          // 逻辑代码 path 
        public string m_resPath;            // 拖放的控件 path 
        public string m_prefabName;         // bundle 中资源的名字
    }
}