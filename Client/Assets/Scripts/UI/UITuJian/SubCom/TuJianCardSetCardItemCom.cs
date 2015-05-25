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