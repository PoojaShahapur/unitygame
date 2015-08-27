using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 一项地图集加载资源
     */
    public class AtlasGoRes : InsResBase
    {
        public GameObject m_go;
        protected GameObject m_subGo;

        protected Dictionary<string, ImageItem> m_path2Image = new Dictionary<string,ImageItem>();

        public AtlasGoRes()
        {

        }

        public override void unload()
        {
            m_go = null;
        }

        public ImageItem getImage(string spriteName)
        {
            if(!m_path2Image.ContainsKey(spriteName))
            {
                m_subGo = UtilApi.TransFindChildByPObjAndPath(m_go, spriteName);
                Image image = UtilApi.getComByP<Image>(m_subGo);
                ImageItem item = new ImageItem();
                item.image = image.sprite;
                m_path2Image[spriteName] = item;
            }
            else
            {
                m_path2Image[spriteName].refCountResLoadResultNotify.refCount.incRef();
            }

            return m_path2Image[spriteName];
        }
    }
}