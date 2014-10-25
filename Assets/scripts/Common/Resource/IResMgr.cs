using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface IResMgr
    {
        IRes load(LoadParam param);
        void unload(string path);
        IRes getResource(string path);
        LoadParam getLoadParam();
    }
}
