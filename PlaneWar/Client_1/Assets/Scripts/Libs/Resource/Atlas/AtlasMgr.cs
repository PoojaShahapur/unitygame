using System;

namespace SDK.Lib
{
    /**
     * @brief 地图集系统
     */
    public class AtlasMgr : InsResMgrBase
    {
        public AtlasMgr()
        { 
            
        }

        public void postInit()
        {

        }

        // 加载图像
        public void loadImage(LoadParam param)
        {
            if (!mPath2ResDic.ContainsKey(param.mResUniqueId))
            {
                // 保存加载事件处理，因为这个时候资源还没有加载，这次调用仅仅是想加载 AtlasScriptRes ，不想直接回调事件处理函数
                MAction<IDispatchObject> tmpLoadEventHandle = param.mLoadEventHandle;
                param.mLoadEventHandle = null;

                AtlasScriptRes atlasRes = createResItem<AtlasScriptRes>(param);

                param.mLoadEventHandle = tmpLoadEventHandle;
                tmpLoadEventHandle = null;

                atlasRes.loadImage(param);

                tmpLoadEventHandle = param.mLoadEventHandle;
                param.mLoadEventHandle = null;

                param.mLoadInsRes = atlasRes;
                load<AtlasScriptRes>(param);
            }
            else
            {
                (mPath2ResDic[param.mResUniqueId] as AtlasScriptRes).loadImage(param);
            }
        }

        public ImageItem getAndLoadImage(LoadParam param)
        {
            loadImage(param);
            return getImage(param.mResUniqueId, param.mSubPath);
        }

        public void syncLoadImage(string atlasName, string spriteName)
        {
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(atlasName);
            param.mSubPath = spriteName;
            param.mLoadNeedCoroutine = false;
            param.mResNeedCoroutine = false;
            loadImage(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        // 目前只实现同步加载，异步加载没有实现。syncLoad 同步加载资源不能喝异步加载资源的接口同时去加载一个资源，如果异步加载一个资源，这个时候资源还没有加载完成，然后又同步加载一个资源，这个时候获取的资源是没有加载完成的，由于同步加载资源没有回调，因此即使同步加载的资源加载完成，也不可能获取加载完成事件
        public ImageItem getAndSyncLoadImage(string atlasName, string spriteName)
        {
            syncLoadImage(atlasName, spriteName);
            return getImage(atlasName, spriteName);
        }

        /**
         * @brief  获取地图集中的一个 Sprite 
         * @param atlasName 地图集的名字，就是资源的路径，相对于 Resources ，例如 "Atlas/Common.prefab" 
         * @param spriteName 就是图片的目录，相对于打包的根目录，文件名字不加扩展名字，例如 denglu_srk ，而不是 denglu_srk.png 
         */
        public ImageItem getImage(string atlasName, string spriteName)
        {
            return (mPath2ResDic[atlasName] as AtlasScriptRes).getImage(spriteName);
        }

        // 暂时没有实现
        public void unloadImage(string atlasName, string spriteName, MAction<IDispatchObject> loadEventHandle)
        {
            if(mPath2ResDic.ContainsKey(atlasName))
            {
                (mPath2ResDic[atlasName] as AtlasScriptRes).unloadImage(spriteName, loadEventHandle);
                if (!(mPath2ResDic[atlasName] as AtlasScriptRes).bHasRefImageItem())        // 如果没有引用的 Image
                {
                    unload(atlasName, loadEventHandle);            // 卸载对应的资源
                }
            }
            else
            {
            }
        }

        // 暂时没有实现
        public void unloadImage(ImageItem imageItem, MAction<IDispatchObject> loadEventHandle)
        {
            if (imageItem != null && imageItem.atlasScriptRes != null)
            {
                unloadImage(imageItem.atlasScriptRes.getResUniqueId(), imageItem.spriteName, loadEventHandle);
            }
            else
            {
            }
        }
    }
}