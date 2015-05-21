using SDK.Common;
using UnityEngine;
using UnityEngine.UI;
namespace SDK.Lib
{
    /**
     * @brief 一项地图项
     */
    public class ImageItem : RefCount
    {
        protected Sprite m_image;

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