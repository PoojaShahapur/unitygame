using LuaInterface;
using System.Collections.Generic;
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

        public ShaderRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndSyncLoad<ShaderRes>(path, handle);
        }

        public ShaderRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<ShaderRes>(path, handle);
        }

        public ShaderRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            return getAndAsyncLoad<ShaderRes>(path, luaTable, luaFunction, true);
        }
    }
}