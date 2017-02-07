namespace SDK.Lib
{
    public class SpriteMgr : InsResMgrBase
    {
        public SpriteAtlasRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndSyncLoad<SpriteAtlasRes>(path, handle, progressHandle, true);
        }

        public SpriteAtlasRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndSyncLoad<SpriteAtlasRes>(path, luaTable, luaFunction, progressLuaFunction, true);
        }

        public SpriteAtlasRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndAsyncLoad<SpriteAtlasRes>(path, handle, progressHandle, true);
        }

        public SpriteAtlasRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndAsyncLoad<SpriteAtlasRes>(path, luaTable, luaFunction, progressLuaFunction, true);
        }
    }
}