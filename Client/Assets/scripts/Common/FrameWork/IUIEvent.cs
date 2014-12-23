using System;

namespace SDK.Common
{
    public interface IUIEvent
    {
        void onCodeFormLoaded(IForm form);
        void onWidgetLoaded(IForm form);
    }
}