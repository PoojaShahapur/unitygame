using SDK.Common;
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
        protected string m_modelPath;           // 模型目录

        public CardPlayerRender(SceneEntityBase entity_) :
            base(entity_)
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

        override public void show()
        {
            UtilApi.SetActive(m_model.selfGo, true);
        }

        override public void hide()
        {
            UtilApi.SetActive(m_model.selfGo, false);
        }

        override public bool IsVisible()
        {
            return UtilApi.IsActive(m_model.selfGo);
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
            m_modelPath = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[tableBody.m_type].m_handleModelPath;
            setTableItemAndPnt(tableBody, pntGo_);
        }

        virtual public void setTableItemAndPnt(TableCardItemBody tableBody, GameObject pntGo_)
        {
            updateModel(tableBody, pntGo_);
            updateLeftAttr(tableBody);      // 更新剩余的属性
            modifyTex(m_model.selfGo, tableBody);

            addHandle();
        }

        virtual public void updateModel(TableCardItemBody tableBody, GameObject pntGo_)
        {
            m_cardModelItem = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[tableBody.m_type];

            m_model.pntGo = pntGo_;
            m_model.modelResPath = m_modelPath;
            m_model.syncUpdateModel();
        }

        virtual protected void addHandle()
        {
            UtilApi.addEventHandle(gameObject(), onEntityClick);
            UtilApi.addPressHandle(gameObject(), onEntityDownUp);
            UtilApi.addDragOverHandle(gameObject(), onEntityDragOver);
            UtilApi.addDragOutHandle(gameObject(), onEntityDragOut);
        }

        virtual protected void updateLeftAttr(TableCardItemBody tableBody)
        {

        }

        // 修改卡牌纹理
        virtual protected void modifyTex(GameObject go_, TableCardItemBody tableBody)
        {
            
        }

        override public GameObject getPnt()
        {
            return m_model.pntGo;
        }
    }
}