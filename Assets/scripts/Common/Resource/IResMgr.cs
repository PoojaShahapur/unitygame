using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface IResMgr
    {
        IRes load(LoadParam param);
        LoadParam getLoadParam();
    }
}
