using SDK.Lib;
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
        protected RefCountResLoadResultNotify m_refCountResLoadResultNotify;

        public ImageItem()
        {
            m_refCountResLoadResultNotify = new RefCountResLoadResultNotify();
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
                return m_refCountResLoadResultNotify;
            }
            set
            {
                m_refCountResLoadResultNotify = value;
            }
        }

        public void init(AtlasScriptRes atlasScriptRes)
        {
            m_image = atlasScriptRes.getSprite(m_spriteName);
            m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public void failed(AtlasScriptRes atlasScriptRes)
        {
            m_refCountResLoadResultNotify.resLoadState.setFailed();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
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