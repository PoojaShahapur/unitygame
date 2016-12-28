namespace SDK.Lib
{
    public class UIAttrItem
    {
        public string mCodePath;               // 逻辑代码 path
        public string mWidgetPath;             // 拖放的控件 path
        public string mScriptTypeName;         // 脚本代码的名字空间和名字

        public UICanvasID mCanvasID;           // 在哪个 Canvas
        public UILayerId mLayerID;             // 所在的 Layer
        public byte mUiSceneType;              // 场景类型列表

        public bool mIsNeedLua;
        public string mLuaScriptPath;
        public string mLuaScriptTableName;

        public void addUISceneType(UISceneType sceneType)
        {
            UtilMath.setState((int)sceneType, ref mUiSceneType);
        }

        public bool canUnloadUIBySceneType(UISceneType unloadSceneType, UISceneType loadSceneTpe)
        {
            // 在卸载 UI 场景类型中，但是不在加载场景类型中
            if (UtilMath.checkState((int)unloadSceneType, mUiSceneType) &&
               !UtilMath.checkState((int)loadSceneTpe, mUiSceneType))
            {
                return true;
            }

            return false;
        }
    }
}