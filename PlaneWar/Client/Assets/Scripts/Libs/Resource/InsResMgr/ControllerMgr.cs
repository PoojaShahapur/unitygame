namespace SDK.Lib
{
    /**
     * @brief 主要是 Animator 中 Controller 管理器
     */
    public class ControllerMgr : InsResMgrBase
    {
        public ControllerRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return this.getAndSyncLoad<ControllerRes>(path, handle, progressHandle);
        }

        public ControllerRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return this.getAndSyncLoad<ControllerRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public ControllerRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return this.getAndAsyncLoad<ControllerRes>(path, handle, progressHandle);
        }

        public ControllerRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return this.getAndAsyncLoad<ControllerRes>(path, luaTable, luaFunction, progressLuaFunction);
        }
    }
}