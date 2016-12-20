using LuaInterface;

namespace SDK.Lib
{
    public class SpriteMgr : InsResMgrBase
    {
        public SpriteAtlasRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<SpriteAtlasRes>(path, true);
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