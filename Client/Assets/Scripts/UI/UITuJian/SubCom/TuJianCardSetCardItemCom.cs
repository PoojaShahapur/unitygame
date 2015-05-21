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
        protected GameObject m_sceneGo;
        protected TableCardItemBody m_cardItem; // 卡牌基本数据
        protected ImageItem m_imageItem;
        protected UIPrefabRes m_uiPrefabRes;

        public TuJianCardSetCardItemCom()
        {
            
        }

        public TableCardItemBody cardItem
        {
            set
            {
                m_cardItem = value;
            }
        }

        public GameObject sceneGo
        {
            get
            {
                return m_sceneGo;
            }
        }

        public void dispose()
        {
            Ctx.m_instance.m_uiPrefabMgr.unload(m_uiPrefabRes.GetPath());
            Ctx.m_instance.m_atlasMgr.unloadImage(m_imageItem);
            UtilApi.Destroy(m_sceneGo);
        }

        public void createSceneGo()
        {
            m_uiPrefabRes = Ctx.m_instance.m_uiPrefabMgr.syncGet<UIPrefabRes>(TuJianPath.CardSetCardPrefabPath);
            m_sceneGo = m_uiPrefabRes.InstantiateObject(TuJianPath.CardSetCardPrefabPath);
        }

        public void updateImage()
        {
            if (string.IsNullOrEmpty(m_cardItem.m_cardHeader))
            {
                m_cardItem.m_cardHeader = "emei_taopai";
            }
            m_imageItem = Ctx.m_instance.m_atlasMgr.getAndAsyncLoadImage("Atlas/TuJianDyn.asset", m_cardItem.m_cardHeader);
            m_imageItem.setGoImage(m_sceneGo);
        }

        public void add2Layout(AuxLayoutV cardLayoutV)
        {
            cardLayoutV.addElem(m_sceneGo, true);
        }
    }
}