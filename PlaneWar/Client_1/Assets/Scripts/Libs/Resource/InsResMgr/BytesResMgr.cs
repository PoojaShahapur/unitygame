namespace SDK.Lib
{
    public class BytesResMgr : InsResMgrBase
    {
        public BytesResMgr()
        {

        }

        public BytesRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return this.getAndSyncLoad<BytesRes>(path, handle, progressHandle);
        }

        public BytesRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return this.getAndSyncLoad<BytesRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public BytesRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return this.getAndAsyncLoad<BytesRes>(path, handle, progressHandle);
        }

        public BytesRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return this.getAndAsyncLoad<BytesRes>(path, luaTable, luaFunction, progressLuaFunction);
        }
    }
}