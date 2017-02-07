namespace SDK.Lib
{
    public class SkelAniMgr : InsResMgrBase
    {
        public SkelAniMgr()
        {

        }

        public SkelAnimRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndSyncLoad<SkelAnimRes>(path, handle, progressHandle);
        }

        public SkelAnimRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndSyncLoad<SkelAnimRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public SkelAnimRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndAsyncLoad<SkelAnimRes>(path, handle, progressHandle);
        }

        public SkelAnimRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndAsyncLoad<SkelAnimRes>(path, luaTable, luaFunction, progressLuaFunction, true);
        }
    }
}