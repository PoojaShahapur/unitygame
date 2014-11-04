using SDK.Common;
using System;

namespace Game.Game
{
    public class GameUIEventCB : IUIEvent
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