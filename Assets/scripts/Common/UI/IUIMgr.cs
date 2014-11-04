namespace SDK.Common
{
    public interface IUIMgr
    {
        void destroyForm(UIFormID ID);
        void loadForm(UIFormID ID);
        void SetIUIFactory(IUIFactory value);
    }
}