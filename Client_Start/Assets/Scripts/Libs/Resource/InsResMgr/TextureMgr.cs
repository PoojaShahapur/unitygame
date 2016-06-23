using LuaInterface;

namespace SDK.Lib
{
    public class TextureMgr : InsResMgrBase
    {
        public TextureMgr()
        {

        }

        public TextureRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<TextureRes>(path);
        }

        public TextureRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<TextureRes>(path, handle);
        }

        public TextureRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndAsyncLoad<TextureRes>(path, luaTable, luaFunction);
        }
    }
}