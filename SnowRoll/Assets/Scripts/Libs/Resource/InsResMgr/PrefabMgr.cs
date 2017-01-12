using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief 主要管理各种 Prefab 元素
     */
    public class PrefabMgr : InsResMgrBase
    {
        public PrefabRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndSyncLoad<PrefabRes>(path, handle);
        }

        public PrefabRes getAndSyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndSyncLoad<PrefabRes>(path, luaTable, luaFunction);
        }

        public PrefabRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<PrefabRes>(path, handle);
        }

        public PrefabRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndAsyncLoad<PrefabRes>(path, luaTable, luaFunction);
        }
    }
}