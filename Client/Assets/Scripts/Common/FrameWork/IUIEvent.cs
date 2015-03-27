using System;

namespace SDK.Common
{
    public interface IUIEvent
    {
        void onCodeFormLoaded(Form form);
        void onWidgetLoaded(Form form);
    }
}