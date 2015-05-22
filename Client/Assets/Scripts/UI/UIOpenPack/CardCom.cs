using SDK.Common;
using SDK.Lib;
using UnityEngine.UI;

namespace Game.UI
{
    public class CardCom
    {
        protected int m_tag;
        protected Button m_uiCardBtn;
        protected ImageItem m_imageItem;

        public CardCom(int idx)
        {
            m_tag = idx;
        }

        public Button uiCardBtn
        {
            get
            {
                return m_uiCardBtn;
            }
            set
            {
                m_uiCardBtn = value;
            }
        }

        public ImageItem imageItem
        {
            get
            {
                return m_imageItem;
            }
            set
            {
                m_imageItem = value;
            }
        }

        public void load()
        {
            if (m_tag < 3)
            {
                m_imageItem = Ctx.m_instance.m_atlasMgr.getAndAsyncLoadImage("Atlas/ShopDyn.asset", "pdxt_kb1");
            }
            else
            {
                m_imageItem = Ctx.m_instance.m_atlasMgr.getAndAsyncLoadImage("Atlas/ShopDyn.asset", "pdxt_kbd");
            }

            m_imageItem.setGoImage(m_uiCardBtn.gameObject);
        }

        public void dispose()
        {
            Ctx.m_instance.m_atlasMgr.unloadImage(m_imageItem);
        }

        public void loadShop()
        {
            if (m_tag < 4)
            {
                m_imageItem = Ctx.m_instance.m_atlasMgr.getAndAsyncLoadImage("Atlas/ShopDyn.asset", string.Format("pdxt_kb{0}", m_tag+1));
            }
            else
            {
                m_imageItem = Ctx.m_instance.m_atlasMgr.getAndAsyncLoadImage("Atlas/ShopDyn.asset", "pdxt_kbd");
            }

            Image srcImage = UtilApi.getComByP<Image>(m_uiCardBtn.gameObject);
            srcImage.sprite = imageItem.image;
        }
    }
}