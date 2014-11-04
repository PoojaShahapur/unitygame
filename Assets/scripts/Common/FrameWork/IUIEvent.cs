using System;

namespace SDK.Common
{
    public interface IUIEvent
    {
        Action<IForm> getLoadedFunc(UIFormID ID);
    }
}