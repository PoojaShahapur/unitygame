using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景卡牌资源，主要是显示卡牌使用的各种资源
     */
    public class CardPlayerRender : CardRenderBase
    {
        protected CardModelItem m_cardModelItem;        // 异步加载的时候，使用 path 字段
        // 这些是为了卸载资源使用
        protected AuxDynModel m_model;          // 模型资源
        protected CardSubPart[] m_subTex;       // 子模型

        public CardPlayerRender()
        {
            m_model = new AuxDynModel();
            m_subTex = new CardSubPart[(int)CardSubPartType.eTotal];
            for (int idx = 0; idx < (int)CardSubPartType.eTotal; ++idx)
            {
                m_subTex[idx] = new CardSubPart();
            }
        }

        public AuxDynModel model
        {
            get
            {
                return m_model;
            }
        }

        public void onEntityClick(GameObject go)
        {
            m_clickEntityDisp.dispatchEvent(this);
        }

        override public void dispose()
        {
            if (m_model != null)
            {
                m_model.dispose();
                m_model = null;
            }

            for (int idx = 0; idx < (int)CardSubPartType.eTotal; ++idx)
            {
                m_subTex[idx].dispose();
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

        virtual public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            TableCardItemBody tableBody = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, objId).m_itemBody as TableCardItemBody;
            setTableItemAndPnt(tableBody, pntGo_);
        }

        public void setTableItemAndPnt(TableCardItemBody tableBody, GameObject pntGo_)
        {
            m_cardModelItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[tableBody.m_type];

            m_model.pntGo = pntGo_;
            m_model.modelResPath = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[tableBody.m_type].m_path;
            m_model.syncUpdateModel();

            UtilApi.updateCardDataNoChange(tableBody, m_model.selfGo);
            UtilApi.updateCardDataChange(tableBody, m_model.selfGo);

            AuxLabel raceText = new AuxLabel(m_model.selfGo, "UIRoot/RaceText");
            // 更新种族
            if (tableBody.m_race > 0)
            {
                TableRaceItemBody raceTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_RACE, (uint)tableBody.m_race).m_itemBody as TableRaceItemBody;
                raceText.text = raceTableItem.m_raceName;
            }
            else
            {
                GameObject raceGO = UtilApi.TransFindChildByPObjAndPath(m_model.selfGo, m_cardModelItem.m_raceSubModel);
                UtilApi.SetActive(raceGO, false);
                raceText.text = "";
            }

            modifyTex(m_model.selfGo, tableBody);

            UtilApi.addEventHandle(gameObject(), onEntityClick);
        }

        // 修改卡牌纹理
        protected void modifyTex(GameObject go_, TableCardItemBody tableBody)
        {
            // 头像是每一个卡牌一个配置
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], tableBody.m_cardHeader);
            m_subTex[(int)CardSubPartType.eHeader].tex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_headerSubModel);
            m_subTex[(int)CardSubPartType.eHeader].tex.texPath = path;
            m_subTex[(int)CardSubPartType.eHeader].tex.syncUpdateTex();

            TableJobItemBody jobTable;
            // 边框
            jobTable = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)(tableBody.m_career)).m_itemBody as TableJobItemBody;
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], jobTable.m_frameImage);
            m_subTex[(int)CardSubPartType.eFrame].tex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_frameSubModel);
            m_subTex[(int)CardSubPartType.eFrame].tex.texPath = path;
            m_subTex[(int)CardSubPartType.eFrame].tex.syncUpdateTex();

            // 腰带
            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], jobTable.m_yaoDaiImage);
            m_subTex[(int)CardSubPartType.eBelt].tex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_yaoDaiSubModel);
            m_subTex[(int)CardSubPartType.eBelt].tex.texPath = path;
            m_subTex[(int)CardSubPartType.eBelt].tex.syncUpdateTex();

            // 品质
            m_subTex[(int)CardSubPartType.EQuality].tex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_pinZhiSubModel);
            if (tableBody.m_quality == 0)        // 品质 0 ，不显示
            {
                UtilApi.SetActive(m_subTex[(int)CardSubPartType.EQuality].tex.selfGo, false);
            }
            else
            {
                path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], UtilApi.getImageByPinZhi(tableBody.m_quality));
                m_subTex[(int)CardSubPartType.EQuality].tex.texPath = path;
                m_subTex[(int)CardSubPartType.EQuality].tex.syncUpdateTex();
            }
        }
    }
}