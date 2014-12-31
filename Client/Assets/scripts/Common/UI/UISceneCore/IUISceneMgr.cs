namespace SDK.Common
{
    public interface IUISceneMgr
    {
        void showSceneForm(UISceneFormID ID);
        void loadSceneForm(UISceneFormID ID);
        ISceneForm getSceneUI(UISceneFormID ID);
        void SetIUISceneFactory(IUISceneFactory value);
    }
}