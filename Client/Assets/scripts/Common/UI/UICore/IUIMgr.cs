namespace SDK.Common
{
    public interface IUIMgr
    {
        void showForm(UIFormID ID);
        void hideForm(UIFormID ID);
        void exitForm(UIFormID ID);
        void loadForm(UIFormID ID);
        void SetIUIFactory(IUIFactory value);
        void loadWidgetRes(UIFormID ID);
    }
}