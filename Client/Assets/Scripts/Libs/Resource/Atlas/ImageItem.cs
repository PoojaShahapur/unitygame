using UnityEngine.UI;
namespace SDK.Lib
{
    /**
     * @brief 一项地图项
     */
    public class ImageItem
    {
        protected Image m_image;

        public Image image
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
    }
}