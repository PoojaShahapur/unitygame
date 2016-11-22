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
        protected string mAtlasPath;       // 地图集的名字
        protected SOSpriteList mSoSpriteList;
        protected Dictionary<string, ImageItem> mPath2Image;

        public AtlasScriptRes()
        {
            this.mPath2Image = new Dictionary<string, ImageItem>();
        }

        public string atlasPath
        {
            set
            {
                this.mAtlasPath = value;
            }
        }

        public SOSpriteList soSpriteList
        {
            get
            {
                return this.mSoSpriteList;
            }
            set
            {
                this.mSoSpriteList = value;
            }
        }

        override protected void initImpl(ResItem res)
        {
            this.mAtlasPath = res.getLogicPath();
            this.mSoSpriteList = res.getObject(res.getPrefabName()) as SOSpriteList;

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
            foreach (string spriteName in this.mPath2Image.Keys)
            {
                if(this.mPath2Image.ContainsKey(spriteName))
                {
                    this.mPath2Image[spriteName].refCountResLoadResultNotify.loadResEventDispatch.clearEventHandle();
                    this.mPath2Image[spriteName].refCountResLoadResultNotify.refCount.reset();
                    this.mPath2Image[spriteName].unloadImage();
                }
            }

            this.mPath2Image.Clear();
            //this.mSoSpriteList.unload();

            base.unload();
        }

        protected void initImage()
        {
            foreach (ImageItem imageItem in this.mPath2Image.Values)
            {
                imageItem.init(this);
            }
        }

        protected void failedImage()
        {
            foreach (ImageItem imageItem in this.mPath2Image.Values)
            {
                imageItem.failed(this);
            }
        }

        public void loadImage(LoadParam param)
        {
            ImageItem retImage = null;
            if (!this.mPath2Image.ContainsKey(param.m_subPath))
            {
                retImage = createImage(param.m_subPath, refCountResLoadResultNotify.resLoadState);
                retImage.image = getSprite(param.m_subPath);
            }
            else
            {
                retImage = this.mPath2Image[param.m_subPath];
            }
            retImage.refCountResLoadResultNotify.resLoadState.setLoading();
            retImage.refCountResLoadResultNotify.refCount.incRef();

            if (m_refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.m_loadEventHandle != null)
                {
                    param.m_loadEventHandle(retImage);
                }
            }
            else if (m_refCountResLoadResultNotify.resLoadState.hasNotLoadOrLoading())
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
            if(this.mPath2Image.ContainsKey(spriteName))
            {
                this.mPath2Image[spriteName].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, loadEventHandle);
                this.mPath2Image[spriteName].refCountResLoadResultNotify.refCount.decRef();
                if(this.mPath2Image[spriteName].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    this.mPath2Image[spriteName].unloadImage();
                    this.mPath2Image.Remove(spriteName);
                }
            }
        }

        // 必然加载完成
        public ImageItem getImage(string spriteName)
        {
            if (this.mPath2Image.ContainsKey(spriteName))
            {
                return this.mPath2Image[spriteName];
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("地图集 {0} 中的图片 {0} 不能加载", this.mAtlasPath, spriteName));
                Ctx.mInstance.mLogSys.log("输出地图集中的图片列表");
                foreach (var key in this.mPath2Image.Keys)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("地图集 {0} 中的图片 {0}", this.mAtlasPath, key));
                }
                return null;
            }
        }

        // 通过索引获取图像
        public ImageItem getImage(int idx)
        {
            if (!this.mPath2Image.ContainsKey(this.mSoSpriteList.mObjList[idx].mPath))
            {
                addImage2Dic(this.mSoSpriteList.mObjList[idx].mPath);
            }
            return getImage(this.mSoSpriteList.mObjList[idx].mPath);
        }

        protected void addImage2Dic(string spriteName)
        {
            if (this.mSoSpriteList != null)
            {
                foreach (SOSpriteList.SerialObject obj in this.mSoSpriteList.mObjList)
                {
                    if (obj.mPath == spriteName)
                    {
                        createImage(spriteName, m_refCountResLoadResultNotify.resLoadState);
                        this.mPath2Image[obj.mPath].image = obj.mSprite;
                        break;
                    }
                }
            }
        }

        protected ImageItem createImage(string spriteName, ResLoadState resLoadState)
        {
            this.mPath2Image[spriteName] = new ImageItem();
            this.mPath2Image[spriteName].atlasScriptRes = this;
            this.mPath2Image[spriteName].spriteName = spriteName;
            this.mPath2Image[spriteName].refCountResLoadResultNotify.resLoadState.copyFrom(resLoadState);
            return this.mPath2Image[spriteName];
        }

        public Sprite getSprite(string spriteName)
        {
            if (this.mSoSpriteList != null)
            {
                foreach (SOSpriteList.SerialObject obj in this.mSoSpriteList.mObjList)
                {
                    if (obj.mPath == spriteName)
                    {
                        return obj.mSprite;
                    }
                }
            }

            return null;
        }

        public bool bHasRefImageItem()
        {
            return this.mPath2Image.Keys.Count > 0;
        }
    }
}