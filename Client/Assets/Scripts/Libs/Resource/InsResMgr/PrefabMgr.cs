using System;

namespace SDK.Lib
{
    /**
     * @brief 主要管理各种 Prefab 元素
     */
    public class PrefabMgr : ResMgrBase
    {
        public PrefabRes getAndSyncLoadRes(string path)
        {
            path = MFileSys.convResourcesPath2AssetBundlesPath(path);
            path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndSyncLoad<PrefabRes>(path);
        }

        public PrefabRes getAndAsyncLoadRes(string path, Action<IDispatchObject> handle)
        {
            path = MFileSys.convResourcesPath2AssetBundlesPath(path);
            path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndAsyncLoad<PrefabRes>(path, handle);
        }
    }
}