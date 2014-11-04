using SDK.Common;
using System;

namespace Game.Game
{
    public class UIEvent : IUIEvent
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