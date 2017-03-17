namespace SDK.Lib
{
    public class MaterialMgr : InsResMgrBase
    {
        //public Dictionary<MaterialId, Material> mId2MatDic = new Dictionary<MaterialId, Material>();

        public MaterialMgr()
        {

        }

        public MatRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndSyncLoad<MatRes>(path, handle, progressHandle);
        }

        public MatRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndSyncLoad<MatRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public MatRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndAsyncLoad<MatRes>(path, handle, progressHandle);
        }

        public MatRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndAsyncLoad<MatRes>(path, luaTable, luaFunction, progressLuaFunction, true);
        }
    }
}