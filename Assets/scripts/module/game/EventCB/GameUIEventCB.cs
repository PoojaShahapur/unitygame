using SDK.Common;
using System;

namespace Game.Game
{
    public class GameUIEventCB : IUIEvent
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