namespace SDK.Common
{
    public interface IUIMgr
    {
        void showForm(UIFormID ID);
        void exitForm(UIFormID ID);
        void loadForm(UIFormID ID);
        Form getForm(UIFormID ID);
        void SetIUIFactory(IUIFactory value);
        void loadWidgetRes(UIFormID ID);
        void getLayerGameObject();
        void exitAllWin();
    }
}