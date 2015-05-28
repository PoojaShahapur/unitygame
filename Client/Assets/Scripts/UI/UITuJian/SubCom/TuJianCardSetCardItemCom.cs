using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 左边卡牌列表中的一个卡牌
     */
    public class TuJianCardSetCardItemCom
    {
        protected TableCardItemBody m_cardItem; // 卡牌基本数据
        protected AuxDynImageDynGoButton m_auxDynImageDynGoButton;
        protected AuxLabel m_mpNumText;
        protected AuxLabel m_nameText;
        protected AuxStaticImage m_numImage;

        public TuJianCardSetCardItemCom()
        {
            
        }

        public TableCardItemBody cardItem
        {
            set
            {
                m_cardItem = value;
                createSceneGo();
            }
        }

        public AuxStaticImage numImage
        {
            get
            {
                return m_numImage;
            }
        }

        public void dispose()
        {
            if (m_auxDynImageDynGoButton != null)
            {
                m_auxDynImageDynGoButton.dispose();
            }
        }

        public void createSceneGo()
        {
            if (m_auxDynImageDynGoButton == null)
            {
                m_auxDynImageDynGoButton = new AuxDynImageDynGoButton();
            }

            m_auxDynImageDynGoButton.auxDynImageDynGOImage.prefabPath = TuJianPath.CardSetCardPrefabPath;
            m_auxDynImageDynGoButton.auxDynImageDynGOImage.setImageInfo(CVAtlasName.TuJianDyn, m_cardItem.m_cardHeader);
            m_auxDynImageDynGoButton.auxDynImageDynGOImage.imageLoadedDisp.addEventHandle(onImageLoaded);
            m_auxDynImageDynGoButton.auxDynImageDynGOImage.syncUpdateCom();

            m_mpNumText = new AuxLabel(m_auxDynImageDynGoButton.selfGo, TuJianPath.MpNumText);
            m_nameText = new AuxLabel(m_auxDynImageDynGoButton.selfGo, TuJianPath.NameText);
            m_numImage = new AuxStaticImage(m_auxDynImageDynGoButton.selfGo, TuJianPath.NumImage);

            m_mpNumText.text = m_cardItem.m_magicConsume.ToString();
            m_nameText.text = m_cardItem.m_name;
            m_numImage.hide();
        }

        public void add2Layout(AuxLayoutV cardLayoutV)
        {
            cardLayoutV.addElem(m_auxDynImageDynGoButton.selfGo, true);
        }

        public void removeFromLayout(AuxLayoutV cardLayoutV)
        {
            cardLayoutV.removeElem(m_auxDynImageDynGoButton.selfGo, true);
        }

        protected void onImageLoaded(IDispatchObject dispObj)
        {

        }
    }
}