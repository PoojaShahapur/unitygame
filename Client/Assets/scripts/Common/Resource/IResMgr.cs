using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface IResMgr
    {
        IResItem loadBundle(LoadParam param);
        IResItem loadLevel(LoadParam param);
        IResItem loadResources(LoadParam param);
        IResItem load(LoadParam param);
        void unload(string path);
        IResItem getResource(string path);
        LoadParam getLoadParam();
    }
}