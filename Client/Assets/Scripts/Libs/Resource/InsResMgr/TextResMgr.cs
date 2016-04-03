using System;

namespace SDK.Lib
{
    public class TextResMgr : ResMgrBase
    {
        public TextResMgr()
        {

        }

        public TextRes getAndSyncLoadRes(string path)
        {
            path = MFileSys.convResourcesPath2AssetBundlesPath(path);
            path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndSyncLoad<TextRes>(path);
        }

        public TextRes getAndAsyncLoadRes(string path, Action<IDispatchObject> handle)
        {
            path = MFileSys.convResourcesPath2AssetBundlesPath(path);
            path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndAsyncLoad<TextRes>(path, handle);
        }
    }
}