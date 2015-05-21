using SDK.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 一项地图集加载资源
     */
    public class AtlasScriptRes : InsResBase
    {
        protected SOSpriteList m_soSpriteList;
        protected Dictionary<string, ImageItem> m_path2Image;

        public SOSpriteList soSpriteList
        {
            get
            {
                return m_soSpriteList;
            }
            set
            {
                m_soSpriteList = value;
            }
        }

        public override void unload()
        {
            
        }

        public ImageItem getImage(string spriteName)
        {
            if (m_path2Image == null)
            {
                buildDic();
            }
            else
            {
                m_path2Image[spriteName].incRef();
            }

            return m_path2Image[spriteName];
        }

        protected void buildDic()
        {
            m_path2Image = new Dictionary<string, ImageItem>();
            foreach(SOSpriteList.SerialObject obj in m_soSpriteList.m_objList)
            {
                m_path2Image[obj.m_path] = new ImageItem();
                m_path2Image[obj.m_path].image = obj.m_sprite;
            }

            m_soSpriteList.m_objList.Clear();
            m_soSpriteList = null;
        }
    }
}