namespace SDK.Lib
{
    /**
     * @brief 主要管理各种 Prefab 元素
     */
    public class PrefabMgr : InsResMgrBase
    {
        public PrefabRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndSyncLoad<PrefabRes>(path, handle, progressHandle);
        }

        public PrefabRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndSyncLoad<PrefabRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public PrefabRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndAsyncLoad<PrefabRes>(path, handle, progressHandle);
        }

        public PrefabRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndAsyncLoad<PrefabRes>(path, luaTable, luaFunction, progressLuaFunction);
        }
    }
}