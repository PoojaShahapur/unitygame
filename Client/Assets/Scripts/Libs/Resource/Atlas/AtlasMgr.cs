using SDK.Common;
using UnityEngine;
using UnityEngine.UI;
namespace SDK.Lib
{
    /**
     * @brief 地图集系统
     */
    public class AtlasMgr
    {
        public AtlasMgr()
        { 
            
        }

        public void postInit()
        {

        }

        /**
         * @brief  获取地图集中的一个 Sprite 
         * @param atlasName 地图集的名字，就是资源的路径，相对于 Resources ，例如 "Atlas/Common.prefab" 
         * @param spriteName 就是图片的目录，相对于打包的根目录，文件名字不加扩展名字，例如 Common/denglu_srk ，而不是 Common/denglu_srk.png 
         */
        public ImageItem getImage(string atlasName, string spriteName)
        {
            UIPrefabRes res = Ctx.m_instance.m_uiPrefabMgr.syncGet<UIPrefabRes>("Atlas/Common.prefab");
            Image image = res.getComByP<Image>(spriteName);
            ImageItem item = new ImageItem();
            item.image = image;
            return item;
        }
    }
}