namespace SDK.Lib
{
    public class TextureMgr : InsResMgrBase
    {
        public TextureMgr()
        {

        }

        public TextureRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndSyncLoad<TextureRes>(path, handle, progressHandle);
        }

        public TextureRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndSyncLoad<TextureRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public TextureRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndAsyncLoad<TextureRes>(path, handle, progressHandle);
        }

        public TextureRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndAsyncLoad<TextureRes>(path, luaTable, luaFunction, progressLuaFunction);
        }
    }
}