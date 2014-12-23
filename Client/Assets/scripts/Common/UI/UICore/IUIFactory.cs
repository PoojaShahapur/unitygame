namespace SDK.Common
{
    /**
     * @brief UI 创建
     */
    public interface IUIFactory
    {
        // 创建 UI
        IForm CreateForm(UIFormID id);
    }
}