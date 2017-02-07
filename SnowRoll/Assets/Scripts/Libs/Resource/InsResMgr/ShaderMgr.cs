using UnityEngine;

namespace SDK.Lib
{
    public class ShaderMgr : InsResMgrBase
    {
        public MDictionary<string, Shader> mId2ShaderDic;

        public ShaderMgr()
        {
            mId2ShaderDic = new MDictionary<string, Shader>();
        }

        public ShaderRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndSyncLoad<ShaderRes>(path, handle, progressHandle);
        }

        public ShaderRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndSyncLoad<ShaderRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public ShaderRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndAsyncLoad<ShaderRes>(path, handle, progressHandle);
        }

        public ShaderRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndAsyncLoad<ShaderRes>(path, luaTable, luaFunction, progressLuaFunction, true);
        }
    }
}