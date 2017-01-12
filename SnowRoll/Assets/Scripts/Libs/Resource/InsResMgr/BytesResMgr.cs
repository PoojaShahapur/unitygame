namespace SDK.Lib
{
    public class BytesResMgr : InsResMgrBase
    {
        public BytesResMgr()
        {

        }

        public BytesRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return this.getAndSyncLoad<BytesRes>(path, handle);
        }

        public BytesRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null)
        {
            return this.getAndSyncLoad<BytesRes>(path, luaTable, luaFunction);
        }

        public BytesRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return this.getAndAsyncLoad<BytesRes>(path, handle);
        }

        public BytesRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null)
        {
            return this.getAndAsyncLoad<BytesRes>(path, luaTable, luaFunction);
        }
    }
}