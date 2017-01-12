namespace SDK.Lib
{
    /**
     * @brief 主要是 Animator 中 Controller 管理器
     */
    public class ControllerMgr : InsResMgrBase
    {
        public ControllerRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return this.getAndSyncLoad<ControllerRes>(path, handle);
        }

        public ControllerRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null)
        {
            return this.getAndSyncLoad<ControllerRes>(path, luaTable, luaFunction);
        }

        public ControllerRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return this.getAndAsyncLoad<ControllerRes>(path, handle);
        }

        public ControllerRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null)
        {
            return this.getAndAsyncLoad<ControllerRes>(path, luaTable, luaFunction);
        }
    }
}