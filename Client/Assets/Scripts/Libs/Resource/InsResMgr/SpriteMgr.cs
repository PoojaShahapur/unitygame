using LuaInterface;
using System;

namespace SDK.Lib
{
    public class SpriteMgr : ResMgrBase
    {
        public SpriteAtlasRes getAndSyncLoadRes(string path)
        {
            path = MFileSys.convResourcesPath2AssetBundlesPath(path);
            path = path + UtilApi.DOTPNG;
            return getAndSyncLoad<SpriteAtlasRes>(path, true);
        }

        public SpriteAtlasRes getAndAsyncLoadRes(string path, Action<IDispatchObject> handle)
        {
            path = MFileSys.convResourcesPath2AssetBundlesPath(path);
            path = path + UtilApi.DOTPNG;
            return getAndAsyncLoad<SpriteAtlasRes>(path, handle, true);
        }

        public SpriteAtlasRes getAndAsyncLoadRes(string path, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            path = MFileSys.convResourcesPath2AssetBundlesPath(path);
            path = path + UtilApi.DOTPNG;
            return getAndAsyncLoad<SpriteAtlasRes>(path, luaTable, luaFunction, true);
        }
    }
}