using LuaInterface;

namespace SDK.Lib
{
    public class TextResMgr : InsResMgrBase
    {
        public TextResMgr()
        {

        }

        public TextRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndSyncLoad<TextRes>(path, handle);
        }

        public TextRes getAndSyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndSyncLoad<TextRes>(path, luaTable, luaFunction);
        }

        public TextRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<TextRes>(path, handle);
        }

        public TextRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndAsyncLoad<TextRes>(path, luaTable, luaFunction);
        }
    }
}