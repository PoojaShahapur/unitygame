using System;
using System.Collections.Generic;
using UnityEngine;

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

        override protected void initImpl(ResItem res)
        {
            m_atlasPath = res.getLogicPath();
            m_soSpriteList = res.getObject(res.getPrefabName()) as SOSpriteList;

            initImage();
        }

        override public void failed(ResItem res)
        {
            failedImage();
            unload();
        }

        override public void unload()
        {
            // 卸载所有图像
            foreach (string spriteName in m_path2Image.Keys)
            {
                if(m_path2Image.ContainsKey(spriteName))
                {
                    m_path2Image[spriteName].refCountResLoadResultNotify.loadResEventDispatch.clearEventHandle();
                    m_path2Image[spriteName].refCountResLoadResultNotify.refCount.reset();
                    m_path2Image[spriteName].unloadImage();
                }
            }

            m_path2Image.Clear();
            //m_soSpriteList.unload();

            base.unload();
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
            retImage.refCountResLoadResultNotify.resLoadState.setLoading();
            retImage.refCountResLoadResultNotify.refCount.incRef();

            if (refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.m_loadEventHandle != null)
                {
                    param.m_loadEventHandle(retImage);
                }
            }
            else if (refCountResLoadResultNotify.resLoadState.hasNotLoadOrLoading())
            {
                if (param.m_loadEventHandle != null)
                {
                    retImage.refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, param.m_loadEventHandle);
                }
            }
        }

        // 卸载一个 Image
        public void unloadImage(string spriteName, MAction<IDispatchObject> loadEventHandle)
        {
            if(m_path2Image.ContainsKey(spriteName))
            {
                m_path2Image[spriteName].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, loadEventHandle);
                m_path2Image[spriteName].refCountResLoadResultNotify.refCount.decRef();
                if(m_path2Image[spriteName].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    m_path2Image[spriteName].unloadImage();
                    m_path2Image.Remove(spriteName);
                }
            }
        }

        // 必然加载完成
        public ImageItem getImage(string spriteName)
        {
            if (m_path2Image.ContainsKey(spriteName))
            {
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

        // 通过索引获取图像
        public ImageItem getImage(int idx)
        {
            if (!m_path2Image.ContainsKey(m_soSpriteList.m_objList[idx].m_path))
            {
                addImage2Dic(m_soSpriteList.m_objList[idx].m_path);
            }
            return getImage(m_soSpriteList.m_objList[idx].m_path);
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

        public bool bHasRefImageItem()
        {
            return m_path2Image.Keys.Count > 0;
        }
    }
}