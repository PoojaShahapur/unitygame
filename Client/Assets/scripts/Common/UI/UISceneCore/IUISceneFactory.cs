namespace SDK.Common
{
    /**
     * @brief UI 创建
     */
    public interface IUISceneFactory
    {
        // 创建 UI
        ISceneForm CreateSceneForm(UISceneFormID id);
    }
}