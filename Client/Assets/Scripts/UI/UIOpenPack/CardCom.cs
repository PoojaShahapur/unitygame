using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    public class CardCom
    {
        protected int m_tag;
        protected AuxDynImageStaticGoButton m_auxDynImageStaticGoButton;

        public CardCom(int idx)
        {
            m_tag = idx;
        }

        public AuxDynImageStaticGoButton auxDynImageStaticGoButton
        {
            get
            {
                return m_auxDynImageStaticGoButton;
            }
            set
            {
                m_auxDynImageStaticGoButton = value;
            }
        }

        public void createBtn(GameObject btnRoot, string btnPath)
        {
            m_auxDynImageStaticGoButton = new AuxDynImageStaticGoButton(btnRoot, btnPath);
        }

        public void load()
        {
            if (m_tag < 3)
            {
                m_auxDynImageStaticGoButton.auxDynImageStaticGOImage.setImageInfo(CVAtlasName.ShopDyn, "pdxt_kb1");
                m_auxDynImageStaticGoButton.auxDynImageStaticGOImage.syncUpdateCom();
            }
            else
            {
                m_auxDynImageStaticGoButton.auxDynImageStaticGOImage.setImageInfo(CVAtlasName.ShopDyn, "pdxt_kbd");
                m_auxDynImageStaticGoButton.auxDynImageStaticGOImage.syncUpdateCom();
            }
        }

        public void dispose()
        {
            m_auxDynImageStaticGoButton.dispose();
        }

        public void loadShop()
        {
            if (m_tag < 4)
            {
                m_auxDynImageStaticGoButton.auxDynImageStaticGOImage.setImageInfo(CVAtlasName.ShopDyn, string.Format("pdxt_kb{0}", m_tag + 1));
                m_auxDynImageStaticGoButton.auxDynImageStaticGOImage.syncUpdateCom();
            }
            else
            {
                m_auxDynImageStaticGoButton.auxDynImageStaticGOImage.setImageInfo(CVAtlasName.ShopDyn, "pdxt_kbd");
                m_auxDynImageStaticGoButton.auxDynImageStaticGOImage.syncUpdateCom();
            }
        }
    }
}