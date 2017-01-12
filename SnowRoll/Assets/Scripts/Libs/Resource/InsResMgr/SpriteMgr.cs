using LuaInterface;

namespace SDK.Lib
{
    public class SpriteMgr : InsResMgrBase
    {
        public SpriteAtlasRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndSyncLoad<SpriteAtlasRes>(path, handle, true);
        }

        public SpriteAtlasRes getAndSyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndSyncLoad<SpriteAtlasRes>(path, luaTable, luaFunction, true);
        }

        public SpriteAtlasRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<SpriteAtlasRes>(path, handle, true);
        }

        public SpriteAtlasRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndAsyncLoad<SpriteAtlasRes>(path, luaTable, luaFunction, true);
        }
    }
}