namespace SDK.Common
{
    public interface IUISceneMgr
    {
        ISceneForm showSceneForm(UISceneFormID ID);
        ISceneForm loadSceneForm(UISceneFormID ID);
        ISceneForm getSceneUI(UISceneFormID ID);
        void SetIUISceneFactory(IUISceneFactory value);
    }
}