﻿using SDK.Common;
using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 普通卡牌渲染器，这个是图鉴中卡牌、随从卡、法术卡基类
     */
    public class CommonCardRender : CardPlayerRender
    {
        protected UIPrefabRes m_uiPrefabRes;        // 这个是 UI 资源
        protected ModelRes m_boxModel;              // 这个是碰撞盒子模型

        public CommonCardRender(SceneEntityBase entity_) :
            base(entity_)
        {
            m_subTex = new CardSubPart[(int)CardSubPartType.eTotal];
            for (int idx = 0; idx < (int)CardSubPartType.eTotal; ++idx)
            {
                m_subTex[idx] = new CardSubPart();
            }
        }

        override public void dispose()
        {
            for (int idx = 0; idx < (int)CardSubPartType.eTotal; ++idx)
            {
                m_subTex[idx].dispose();
            }

            Ctx.m_instance.m_uiPrefabMgr.unload(m_uiPrefabRes.GetPath(), null);
            m_uiPrefabRes = null;
            Ctx.m_instance.m_modelMgr.unload(m_boxModel.GetPath(), null);
            m_boxModel = null;

            base.dispose();
        }

        override public void setTableItemAndPnt(TableCardItemBody tableBody, GameObject pntGo_)
        {
            updateModel(tableBody, pntGo_);
            addUIAndBox();      // 继续添加 UI 和碰撞

            updateLeftAttr(tableBody);      // 更新剩余的属性
            modifyTex(m_model.selfGo, tableBody);

            addHandle();
        }

        override protected void addHandle()
        {
            
        }

        protected void addUIAndBox()
        {
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/CommonCardUI.prefab");
            m_uiPrefabRes = Ctx.m_instance.m_uiPrefabMgr.getAndSyncLoad<UIPrefabRes>(path);
            GameObject _go = m_uiPrefabRes.InstantiateObject(path);
            _go.name = "UIRoot";
            UtilApi.SetParent(_go, gameObject(), false);

            path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/CommonCardBox.prefab");
            m_boxModel = Ctx.m_instance.m_modelMgr.getAndSyncLoad<ModelRes>(path);
            _go = m_boxModel.InstantiateObject(path);
            _go.name = "CommonCardBox";
            UtilApi.SetParent(_go, gameObject(), false);
            UtilApi.addEventHandle(_go, onEntityClick);
        }

        override protected void updateLeftAttr(TableCardItemBody tableBody)
        {
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
                path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], UtilApi.getImageByPinZhi(tableBody.m_quality));
                m_subTex[(int)CardSubPartType.EQuality].tex.texPath = path;
                m_subTex[(int)CardSubPartType.EQuality].tex.syncUpdateTex();
            }
        }
    }
}