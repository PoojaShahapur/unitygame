using System.Collections.Generic;

namespace SDK.Common
{
    public class UIAttrItem
    {
        public string m_codePath;               // 逻辑代码 path
        public string m_widgetPath;             // 拖放的控件 path
        public string m_scriptTypeName;         // 脚本代码的名字空间和名字

        public UICanvasID m_canvasID;           // 在哪个 Canvas
        public UILayerID m_LayerID;             // 所在的 Layer
        public byte m_uiSceneType;              // 场景类型列表

        public void addUISceneType(UISceneType sceneType)
        {
            UtilMath.setState((int)sceneType, ref m_uiSceneType);
        }

        public bool canUnloadUIBySceneType(UISceneType unloadSceneType, UISceneType loadSceneTpe)
        {
            // 在卸载 UI 场景类型中，但是不在加载场景类型中
            if (UtilMath.checkState((int)unloadSceneType, m_uiSceneType) &&
               !UtilMath.checkState((int)loadSceneTpe, m_uiSceneType))
            {
                return true;
            }

            return false;
        }
    }
}