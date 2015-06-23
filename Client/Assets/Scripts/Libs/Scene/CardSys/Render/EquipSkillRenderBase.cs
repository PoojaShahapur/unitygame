﻿using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 装备卡、技能卡，渲染器
     */
    public class EquipSkillRenderBase : ExceptBlackCardRender
    {
        public EquipSkillRenderBase(SceneEntityBase entity_) :
            base(entity_)
        {
            m_subTex = new CardSubPart[1];
            m_subTex[0] = new CardSubPart();
        }

        override public void dispose()
        {
            m_subTex[0].dispose();

            base.dispose();
        }

        override protected void updateLeftAttr(TableCardItemBody tableBody)
        {

        }

        // 修改卡牌纹理
        override protected void modifyTex(GameObject go_, TableCardItemBody tableBody)
        {
            // 头像是每一个卡牌一个配置
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], tableBody.m_dzCardHeader);
            m_subTex[0].tex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_headerSubModel);          // 场上 Hero 的纹理贴图
            m_subTex[0].tex.texPath = path;
            m_subTex[0].tex.syncUpdateTex();
        }

        // 技能装备没有显示的属性
        override protected void addUIAndBox()
        {
            m_boxModel = Ctx.m_instance.m_modelMgr.getAndSyncLoad<ModelRes>(m_boxModelPath);
            UtilApi.copyBoxCollider(m_boxModel.getObject() as GameObject, gameObject());
        }
    }
}