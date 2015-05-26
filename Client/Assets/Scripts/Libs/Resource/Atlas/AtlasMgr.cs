using SDK.Common;
using System;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 地图集系统
     */
    public class AtlasMgr : ResMgrBase
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
            if (!m_path2ResDic.ContainsKey(param.m_path))
            {
                // 保存加载事件处理，因为这个时候资源还没有加载，这次调用仅仅是想加载 AtlasScriptRes ，不想直接回调事件处理函数
                Action<IDispatchObject> tmpLoadEventHandle = param.m_loadEventHandle;
                param.m_loadEventHandle = null;

                AtlasScriptRes atlasRes = createResItem<AtlasScriptRes>(param);

                param.m_loadEventHandle = tmpLoadEventHandle;
                tmpLoadEventHandle = null;

                atlasRes.loadImage(param);

                tmpLoadEventHandle = param.m_loadEventHandle;
                param.m_loadEventHandle = null;

                param.m_loadInsRes = atlasRes;
                load<AtlasScriptRes>(param);
            }
            else
            {
                (m_path2ResDic[param.m_path] as AtlasScriptRes).loadImage(param);
            }
        }

        public ImageItem getAndLoadImage(LoadParam param)
        {
            loadImage(param);
            return getImage(param.m_path, param.m_subPath);
        }

        public void syncLoadImage(string atlasName, string spriteName)
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(atlasName, param);
            param.m_subPath = spriteName;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            loadImage(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
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
            return (m_path2ResDic[atlasName] as AtlasScriptRes).getImage(spriteName);
        }

        // 暂时没有实现
        public void unloadImage(string atlasName, string spriteName, Action<IDispatchObject> loadEventHandle)
        {
            if(m_path2ResDic.ContainsKey(atlasName))
            {
                (m_path2ResDic[atlasName] as AtlasScriptRes).unloadImage(spriteName, loadEventHandle);
                if (!(m_path2ResDic[atlasName] as AtlasScriptRes).bHasRefImageItem())        // 如果没有引用的 Image
                {
                    unload(atlasName, loadEventHandle);            // 卸载对应的资源
                }
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("Unload Atlas {0} failed", atlasName));
            }
        }

        // 暂时没有实现
        public void unloadImage(ImageItem imageItem, Action<IDispatchObject> loadEventHandle)
        {
            if (imageItem != null && imageItem.atlasScriptRes != null)
            {
                unloadImage(imageItem.atlasScriptRes.GetPath(), imageItem.spriteName, loadEventHandle);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("Unload Null ImageItem");
            }
        }
    }
}