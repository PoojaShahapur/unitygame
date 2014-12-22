using SDK.Common;
using System;

namespace Game.Login
{
    public class LoginUIEventCB : IUIEvent
    {
        public void onCodeFormLoaded(IForm form)
        {
            form.init();
        }

        public void onWidgetLoaded(IForm form)
        {
            form.show();
        }
    }
}