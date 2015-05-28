using SDK.Lib;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 场景卡牌资源，主要是显示卡牌使用的各种资源
     */
    public class SceneCardModel
    {
        protected CardModelItem m_cardModelItem;        // 异步加载的时候，使用 path 字段
        // 这些是为了卸载资源使用
        protected AuxDynModel m_model;          // 模型资源
        protected StaticModelSynTex m_headerModelTex;       // 头像资源
        protected StaticModelSynTex m_frameModelTex;        // 边框资源
        protected StaticModelSynTex m_yaoDaiModelTex;       // 腰带资源
        protected StaticModelSynTex m_pinZhiModelTex;       // 品质资源

        public SceneCardModel()
        {
            m_model = new AuxDynModel();
            m_headerModelTex = new StaticModelSynTex();
            m_frameModelTex = new StaticModelSynTex();
            m_yaoDaiModelTex = new StaticModelSynTex();
            m_pinZhiModelTex = new StaticModelSynTex();
        }

        public AuxDynModel model
        {
            get
            {
                return m_model;
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
        }

        // 修改卡牌纹理
        protected void modifyTex(GameObject go_, CardItemBase cardItem)
        {
            // 头像是每一个卡牌一个配置
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], cardItem.m_tableItemCard.m_cardHeader);
            m_headerModelTex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_headerSubModel);
            m_headerModelTex.texPath = path;
            m_headerModelTex.syncUpdateTex();

            TableJobItemBody jobTable;
            // 边框
            jobTable = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)(cardItem.m_tableItemCard.m_career)).m_itemBody as TableJobItemBody;
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], jobTable.m_frameImage);
            m_frameModelTex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_frameSubModel);
            m_frameModelTex.texPath = path;
            m_headerModelTex.syncUpdateTex();

            // 腰带
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], jobTable.m_yaoDaiImage);
            m_yaoDaiModelTex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_yaoDaiSubModel);
            m_yaoDaiModelTex.texPath = path;
            m_yaoDaiModelTex.syncUpdateTex();

            // 品质
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], UtilApi.getImageByPinZhi(cardItem.m_tableItemCard.m_quality));
            m_pinZhiModelTex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_pinZhiSubModel);
            m_pinZhiModelTex.texPath = path;
            m_pinZhiModelTex.syncUpdateTex();
        }

        public void dispose()
        {
            if (m_model != null)
            {
                m_model.dispose();
                m_model = null;
            }
            if (m_headerModelTex != null)
            {
                m_headerModelTex.dispose();
                m_headerModelTex = null;
            }
            if (m_frameModelTex != null)
            {
                m_frameModelTex.dispose();
                m_frameModelTex = null;
            }
            if (m_yaoDaiModelTex != null)
            {
                m_yaoDaiModelTex.dispose();
                m_yaoDaiModelTex = null;
            }
            if (m_pinZhiModelTex != null)
            {
                m_pinZhiModelTex.dispose();
                m_pinZhiModelTex = null;
            }
        }
    }
}