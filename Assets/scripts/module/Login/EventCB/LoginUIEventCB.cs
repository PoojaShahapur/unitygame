using SDK.Common;
using System;

namespace Game.Login
{
    public class LoginUIEventCB : IUIEvent
    {
        public Action<IForm> getLoadedFunc(UIFormID ID)
        {
            return OnFormLoaded;
        }

        public void OnFormLoaded(IForm form)
        {

        }
    }
}