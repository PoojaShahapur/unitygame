using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface IRes : IEventDispatch
    {
        bool HasLoaded();
    }
}
