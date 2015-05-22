using SDK.Common;
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

        public override void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();

            // 获取资源单独保存
            //(m_path2ResDic[path] as AtlasGoRes).m_go = res.getObject(res.getPrefabName()) as GameObject;
            (m_path2ResDic[path] as AtlasScriptRes).init(res);
            if (m_path2ListenItemDic[path].m_loaded != null)
            {
                m_path2ListenItemDic[path].m_loaded(m_path2ResDic[path]);
            }

            base.onLoaded(resEvt);
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