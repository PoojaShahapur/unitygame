using SDK.Common;
using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 普通卡牌渲染器，这个是图鉴中卡牌、随从卡、法术卡基类
     */
    public class CanOutCardRender : ExceptBlackCardRender
    {
        protected int m_subModeCount;               // 子模型数量

        public CanOutCardRender(SceneEntityBase entity_, int subModelCount_) :
            base(entity_)
        {
            m_subModeCount = subModelCount_;
            // 这个是获取图鉴的数据，其它的场景卡牌在单独的地方设置
            m_uiPrefabPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/CommonCardUI.prefab");
            m_boxModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/CommonCardBox.prefab");

            m_subTex = new CardSubPart[m_subModeCount];
            for (int idx = 0; idx < m_subModeCount; ++idx)
            {
                m_subTex[idx] = new CardSubPart();
            }
        }

        override public void dispose()
        {
            for (int idx = 0; idx < m_subModeCount; ++idx)
            {
                m_subTex[idx].dispose();
            }

            Ctx.m_instance.m_uiPrefabMgr.unload(m_uiPrefabRes.GetPath(), null);
            m_uiPrefabRes = null;
            Ctx.m_instance.m_modelMgr.unload(m_boxModel.GetPath(), null);
            m_boxModel = null;

            base.dispose();
        }

        override protected void updateLeftAttr(TableCardItemBody tableBody)
        {
            // 根据表中的数据更新，如果服务器有数据，还会根据服务器的数据更新一次，因为图鉴的数据是根据表中更新的，场景中的牌是根据服务器的数据更新的
            UtilLogic.updateCardDataNoChangeByTable(tableBody, m_model.selfGo);
            UtilLogic.updateCardDataChangeByTable(tableBody, m_model.selfGo);

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

            if (tableBody.m_type == (int)CardType.CARDTYPE_MAGIC)//法术牌
            {
                GameObject raceGO = UtilApi.TransFindChildByPObjAndPath(m_model.selfGo, "gongji_kapai");
                UtilApi.SetActive(raceGO, false);
                AuxLabel attText = new AuxLabel(m_model.selfGo, "UIRoot/AttText");
                attText.text = "";

                GameObject raceGO2 = UtilApi.TransFindChildByPObjAndPath(m_model.selfGo, "xue_kapai");
                UtilApi.SetActive(raceGO2, false);
                AuxLabel hpText = new AuxLabel(m_model.selfGo, "UIRoot/HpText");
                hpText.text = "";
            }
            else if(tableBody.m_type == (int)CardType.CARDTYPE_EQUIP)//武器牌
            {
                GameObject raceGO2 = UtilApi.TransFindChildByPObjAndPath(m_model.selfGo, "xue_kapai");
                UtilApi.SetActive(raceGO2, false);
                AuxLabel hpText = new AuxLabel(m_model.selfGo, "UIRoot/HpText");
                hpText.text = "";
            }
        }

        // 修改卡牌纹理
        override protected void modifyTex(GameObject go_, TableCardItemBody tableBody)
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
                path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], UtilLogic.getImageByPinZhi(tableBody.m_quality));
                m_subTex[(int)CardSubPartType.EQuality].tex.texPath = path;
                m_subTex[(int)CardSubPartType.EQuality].tex.syncUpdateTex();
            }
        }
    }
}