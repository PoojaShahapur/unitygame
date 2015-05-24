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

        public AtlasScriptRes()
        {
            m_path2Image = new Dictionary<string, ImageItem>();
        }

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

        override public void init(ResItem res)
        {
            m_atlasPath = res.GetPath();
            m_soSpriteList = res.getObject(res.getPrefabName()) as SOSpriteList;

            initImage();
        }

        override public void failed(ResItem res)
        {
            failedImage();
            unload();
        }

        protected void initImage()
        {
            foreach (ImageItem imageItem in m_path2Image.Values)
            {
                imageItem.init(this);
            }
        }

        protected void failedImage()
        {
            foreach (ImageItem imageItem in m_path2Image.Values)
            {
                imageItem.failed(this);
            }
        }

        public void loadImage(LoadParam param)
        {
            ImageItem retImage = null;
            if (!m_path2Image.ContainsKey(param.m_subPath))
            {
                retImage = createImage(param.m_subPath, refCountResLoadResultNotify.resLoadState);
                retImage.image = getSprite(param.m_subPath);
            }
            else
            {
                retImage = m_path2Image[param.m_subPath];
            }
            retImage.refCountResLoadResultNotify.refCount.incRef();

            if (refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.m_loadEventHandle != null)
                {
                    param.m_loadEventHandle(retImage);
                }
            }
            else if (refCountResLoadResultNotify.resLoadState.hasLoading())
            {
                if (param.m_loadEventHandle != null)
                {
                    retImage.refCountResLoadResultNotify.loadEventDispatch.addEventHandle(param.m_loadEventHandle);
                }
            }
        }

        public override void unload()
        {
            
        }

        // 必然加载完成
        public ImageItem getImage(string spriteName)
        {
            if (!m_path2Image.ContainsKey(spriteName))
            {
                addImage2Dic(spriteName);
            }
            if (m_path2Image.ContainsKey(spriteName))
            {
                // 获取资源接口不增加引用计数
                //m_path2Image[spriteName].refCountResLoadResultNotify.refCount.incRef();
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

        protected void addImage2Dic(string spriteName)
        {
            if (m_soSpriteList != null)
            {
                foreach (SOSpriteList.SerialObject obj in m_soSpriteList.m_objList)
                {
                    if (obj.m_path == spriteName)
                    {
                        createImage(spriteName, refCountResLoadResultNotify.resLoadState);
                        m_path2Image[obj.m_path].image = obj.m_sprite;
                        break;
                    }
                }
            }
        }

        protected ImageItem createImage(string spriteName, ResLoadState resLoadState)
        {
            m_path2Image[spriteName] = new ImageItem();
            m_path2Image[spriteName].atlasScriptRes = this;
            m_path2Image[spriteName].spriteName = spriteName;
            m_path2Image[spriteName].refCountResLoadResultNotify.resLoadState.copyFrom(resLoadState);
            return m_path2Image[spriteName];
        }

        public Sprite getSprite(string spriteName)
        {
            if (m_soSpriteList != null)
            {
                foreach (SOSpriteList.SerialObject obj in m_soSpriteList.m_objList)
                {
                    if (obj.m_path == spriteName)
                    {
                        return obj.m_sprite;
                    }
                }
            }

            return null;
        }
    }
}