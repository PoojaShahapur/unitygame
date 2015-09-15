using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 左边卡牌列表中的一个卡牌
     */
    public class TuJianCardSetCardItemCom
    {
        protected TableCardItemBody m_cardItem; // 卡牌基本数据
        protected AuxDynTexDynGOButton m_auxDynTexDynGOButton;
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
            if (m_auxDynTexDynGOButton != null)
            {
                m_auxDynTexDynGOButton.dispose();
            }
        }

        public void createSceneGo()
        {
            if (m_auxDynTexDynGOButton == null)
            {
                m_auxDynTexDynGOButton = new AuxDynTexDynGOButton();
            }

            m_auxDynTexDynGOButton.prefabPath = TuJianPath.CardSetCardPrefabPath;
            m_auxDynTexDynGOButton.texPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], m_cardItem.m_cardSetCardHeader);
            m_auxDynTexDynGOButton.addEventHandle(onImageLoaded);
            m_auxDynTexDynGOButton.syncUpdateCom();

            m_mpNumText = new AuxLabel(m_auxDynTexDynGOButton.selfGo, TuJianPath.MpNumText);
            m_nameText = new AuxLabel(m_auxDynTexDynGOButton.selfGo, TuJianPath.NameText);
            m_numImage = new AuxStaticImage(m_auxDynTexDynGOButton.selfGo, TuJianPath.NumImage);

            m_mpNumText.text = m_cardItem.m_magicConsume.ToString();
            m_nameText.text = m_cardItem.m_name;
            m_numImage.hide();
        }

        public void add2Layout(AuxLayoutV cardLayoutV)
        {
            cardLayoutV.addElem(m_auxDynTexDynGOButton.selfGo, true);
        }

        public void removeFromLayout(AuxLayoutV cardLayoutV)
        {
            cardLayoutV.removeElem(m_auxDynTexDynGOButton.selfGo, true);
        }

        protected void onImageLoaded(IDispatchObject dispObj)
        {

        }
    }
}