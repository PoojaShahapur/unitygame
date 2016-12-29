using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 一项地图项
     */
    public class ImageItem : IDispatchObject
    {
        protected Sprite m_image;
        protected AtlasScriptRes m_atlasScriptRes;
        protected string m_spriteName;
        protected RefCountResLoadResultNotify mRefCountResLoadResultNotify;

        public ImageItem()
        {
            mRefCountResLoadResultNotify = new RefCountResLoadResultNotify();
        }

        public Sprite image
        {
            get
            {
                return m_image;
            }
            set
            {
                m_image = value;
            }
        }

        public AtlasScriptRes atlasScriptRes
        {
            get
            {
                return m_atlasScriptRes;
            }
            set
            {
                m_atlasScriptRes = value;
            }
        }

        public string spriteName
        {
            get
            {
                return m_spriteName;
            }
            set
            {
                m_spriteName = value;
            }
        }

        public RefCountResLoadResultNotify refCountResLoadResultNotify
        {
            get
            {
                return mRefCountResLoadResultNotify;
            }
            set
            {
                mRefCountResLoadResultNotify = value;
            }
        }

        public void init(AtlasScriptRes atlasScriptRes)
        {
            m_image = atlasScriptRes.getSprite(m_spriteName);
            mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public void failed(AtlasScriptRes atlasScriptRes)
        {
            mRefCountResLoadResultNotify.resLoadState.setFailed();
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public void unloadImage()
        {
            
        }

        public void setGoImage(GameObject go_)
        {
            Image _image = UtilApi.getComByP<Image>(go_);
            _image.sprite = m_image;
        }

        public void setImageImage(Image image_)
        {
            image_.sprite = m_image;
        }
    }
}