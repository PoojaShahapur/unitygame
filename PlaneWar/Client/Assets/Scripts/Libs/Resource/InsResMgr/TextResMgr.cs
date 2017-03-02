namespace SDK.Lib
{
    public class TextResMgr : InsResMgrBase
    {
        public TextResMgr()
        {

        }

        public TextRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndSyncLoad<TextRes>(path, handle, progressHandle);
        }

        public TextRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndSyncLoad<TextRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public TextRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndAsyncLoad<TextRes>(path, handle, progressHandle);
        }

        public TextRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndAsyncLoad<TextRes>(path, luaTable, luaFunction, progressLuaFunction);
        }
    }
}