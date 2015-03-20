namespace SDK.Common
{
    public interface IUISceneMgr
    {
        ISceneForm showSceneForm(UISceneFormID ID);
        void hideSceneForm(UISceneFormID ID);
        ISceneForm loadSceneForm(UISceneFormID ID);
        ISceneForm getSceneUI(UISceneFormID ID);
        void SetIUISceneFactory(IUISceneFactory value);
        void readySceneForm(UISceneFormID ID);
        ISceneForm loadAndShowForm(UISceneFormID ID);
        void exitSceneForm(UISceneFormID ID, bool bremoved = true);
        void unloadAll();
    }
}