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
        protected string m_atlasPath;       // 地图集的名字
        protected SOSpriteList m_soSpriteList;
        protected Dictionary<string, ImageItem> m_path2Image;

        public string atlasPath
        {
            set
            {
                m_atlasPath = value;
            }
        }

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

        public void init(IResItem res)
        {
            m_atlasPath = res.GetPath();
            m_soSpriteList = res.getObject(res.getPrefabName()) as SOSpriteList;
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
            if (m_path2Image.ContainsKey(spriteName))
            {
                m_path2Image[spriteName].incRef();
                return m_path2Image[spriteName];
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("地图集 {0} 中的图片 {0} 不能加载", m_atlasPath, spriteName));
                Ctx.m_instance.m_logSys.log("输出地图集中的图片列表");
                foreach (var key in m_path2Image.Keys)
                {
                    Ctx.m_instance.m_logSys.log(string.Format("地图集 {0} 中的图片 {0}", m_atlasPath, key));
                }
                return null;
            }
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