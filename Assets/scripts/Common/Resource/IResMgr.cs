using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface IResMgr
    {
        IRes loadBundle(LoadParam param);
        IRes loadLevel(LoadParam param);
        IRes loadResources(LoadParam param);
        IRes load(LoadParam param);
        void unload(string path);
        IRes getResource(string path);
        LoadParam getLoadParam();
    }
}