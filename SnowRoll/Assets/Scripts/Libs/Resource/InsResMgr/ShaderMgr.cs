using LuaInterface;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class ShaderMgr : InsResMgrBase
    {
        public Dictionary<string, Shader> mId2ShaderDic;

        public ShaderMgr()
        {
            mId2ShaderDic = new Dictionary<string, Shader>();
        }

        public ShaderRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<ShaderRes>(path);
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