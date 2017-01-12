using LuaInterface;

namespace SDK.Lib
{
    public class MaterialMgr : InsResMgrBase
    {
        //public Dictionary<MaterialId, Material> mId2MatDic = new Dictionary<MaterialId, Material>();

        public MaterialMgr()
        {

        }

        public MatRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndSyncLoad<MatRes>(path, handle);
        }

        public MatRes getAndSyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndSyncLoad<MatRes>(path, luaTable, luaFunction);
        }

        public MatRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<MatRes>(path, handle);
        }

        public MatRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndAsyncLoad<MatRes>(path, luaTable, luaFunction, true);
        }
    }
}