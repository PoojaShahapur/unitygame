using LuaInterface;
using System;

namespace SDK.Lib
{
    /**
     * @brief 主要管理各种 Prefab 元素
     */
    public class PrefabMgr : InsResMgrBase
    {
        public PrefabRes getAndSyncLoadRes(string path)
        {
            //path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndSyncLoad<PrefabRes>(path);
        }

        public PrefabRes getAndAsyncLoadRes(string path, Action<IDispatchObject> handle)
        {
            //path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndAsyncLoad<PrefabRes>(path, handle);
        }

        public PrefabRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            //path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndAsyncLoad<PrefabRes>(path, luaTable, luaFunction);
        }
    }
}