using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主角渲染器
     */
    public class HeroRender : CardRenderBase
    {
        protected CardModelItem m_cardModelItem;        // 异步加载的时候，使用 path 字段
        // 这些是为了卸载资源使用
        protected AuxDynModel m_model;          // 模型资源
        protected CardSubPart m_subTex;         // 子模型
        protected EventDispatch m_clickEntityDisp;  // 点击事件分发

        public HeroRender()
        {
            m_model = new AuxDynModel();
            m_subTex = new CardSubPart();
            m_clickEntityDisp = new EventDispatch();
        }

        public AuxDynModel model
        {
            get
            {
                return m_model;
            }
        }

        public EventDispatch clickEntityDisp
        {
            get
            {
                return m_clickEntityDisp;
            }
        }

        virtual public void createCard(CardItemBase cardItem, GameObject pntGo_)
        {
            m_cardModelItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[(int)cardItem.m_tableItemCard.m_type];

            m_model.pntGo = pntGo_;
            m_model.modelResPath = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[(int)cardItem.m_tableItemCard.m_type].m_path;
            m_model.syncUpdateModel();

            UtilApi.updateCardDataNoChange(cardItem.m_tableItemCard, m_model.selfGo);
            UtilApi.updateCardDataChange(cardItem.m_tableItemCard, m_model.selfGo);

            AuxLabel numText = null;
            numText = new AuxLabel(m_model.selfGo, "UIRoot/NumText");       // 卡牌数量
            numText.text = cardItem.m_tujian.num.ToString();

            AuxLabel raceText = new AuxLabel(m_model.selfGo, "UIRoot/RaceText");
            // 更新种族
            if (cardItem.m_tableItemCard.m_race > 0)
            {
                TableRaceItemBody raceTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_RACE, (uint)cardItem.m_tableItemCard.m_race).m_itemBody as TableRaceItemBody;
                raceText.text = raceTableItem.m_raceName;
            }
            else
            {
                GameObject raceGO = UtilApi.TransFindChildByPObjAndPath(m_model.selfGo, m_cardModelItem.m_raceSubModel);
                UtilApi.SetActive(raceGO, false);
                raceText.text = "";
            }

            modifyTex(m_model.selfGo, cardItem);

            UtilApi.addEventHandle(gameObject(), onEntityClick);
        }

        public void onEntityClick(GameObject go)
        {
            m_clickEntityDisp.dispatchEvent(this);
        }

        // 修改卡牌纹理
        protected void modifyTex(GameObject go_, CardItemBase cardItem)
        {
            // 头像是每一个卡牌一个配置
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], cardItem.m_tableItemCard.m_cardHeader);
            m_subTex.tex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_headerSubModel);
            m_subTex.tex.texPath = path;
            m_subTex.tex.syncUpdateTex();
        }

        override public void dispose()
        {
            if (m_model != null)
            {
                m_model.dispose();
                m_model = null;
            }

            if (m_subTex != null)
            {
                m_subTex.dispose();
                m_subTex = null;
            }
            m_clickEntityDisp.clearEventHandle();
        }

        override public GameObject gameObject()
        {
            return m_model.selfGo;
        }

        override public Transform transform()
        {
            return m_model.selfGo.transform;
        }

        override public void setGameObject(GameObject rhv)
        {
            model.selfGo = rhv;
        }
    }
}