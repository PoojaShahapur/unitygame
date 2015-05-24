using SDK.Common;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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

        public ImageItem loadImage(LoadParam param)
        {
            if(!m_path2ResDic.ContainsKey(param.m_path))
            {
                // 保存加载事件处理，因为这个时候资源还没有加载，这次调用仅仅是想加载 AtlasScriptRes ，不想直接回调事件处理函数
                Action<IDispatchObject> tmpLoadEventHandle = param.m_loadEventHandle;
                param.m_loadEventHandle = null;

                load<AtlasScriptRes>(param);

                param.m_loadEventHandle = tmpLoadEventHandle;
                tmpLoadEventHandle = null;
            }

            return (m_path2ResDic[param.m_path] as AtlasScriptRes).loadImage(param);
        }

        // 目前只实现同步加载，异步加载没有实现
        public ImageItem getAndAsyncLoadImage(string atlasName, string spriteName)
        {
            if(!m_path2ResDic.ContainsKey(atlasName))
            {
                AtlasScriptRes res = syncGet<AtlasScriptRes>(atlasName);
            }

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
        public void unloadImage(string atlasName, string spriteName)
        {
            
        }

        // 暂时没有实现
        public void unloadImage(ImageItem imageItem)
        {

        }
    }
}