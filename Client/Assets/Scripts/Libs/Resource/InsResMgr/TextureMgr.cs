using LuaInterface;
using System;

namespace SDK.Lib
{
    public class TextureMgr : ResMgrBase
    {
        public TextureMgr()
        {

        }

        public TextureRes getAndSyncLoadRes(string path)
        {
            //path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndSyncLoad<TextureRes>(path);
        }

        public TextureRes getAndAsyncLoadRes(string path, Action<IDispatchObject> handle)
        {
            //path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndAsyncLoad<TextureRes>(path, handle);
        }

        public TextureRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            //path = path + UtilApi.PREFAB_DOT_EXT;
            return getAndAsyncLoad<TextureRes>(path, luaTable, luaFunction);
        }
    }
}