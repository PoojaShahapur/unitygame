﻿using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景卡牌资源，主要是显示卡牌使用的各种资源，图鉴卡牌、英雄卡、装备卡、技能卡、随从卡、法术卡基类
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

        virtual public void setTableItemAndPnt(TableCardItemBody tableBody, GameObject pntGo_)
        {
            updateModel(tableBody, pntGo_);
            updateLeftAttr(tableBody);      // 更新剩余的属性
            modifyTex(m_model.selfGo, tableBody);

            addHandle();
        }

        public void updateModel(TableCardItemBody tableBody, GameObject pntGo_)
        {
            m_cardModelItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[tableBody.m_type];

            m_model.pntGo = pntGo_;
            m_model.modelResPath = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[tableBody.m_type].m_path;
            m_model.syncUpdateModel();
        }

        virtual protected void addHandle()
        {
            UtilApi.addEventHandle(gameObject(), onEntityClick);
        }

        virtual protected void updateLeftAttr(TableCardItemBody tableBody)
        {

        }

        // 修改卡牌纹理
        virtual protected void modifyTex(GameObject go_, TableCardItemBody tableBody)
        {
            
        }
    }
}