using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 出手的场牌，出牌区域的卡牌渲染器
     */
    public class ChangCardRender : CanOutCardRender
    {
        public ChangCardRender(SceneEntityBase entity_) :
            base(entity_, 1)
        {
            m_uiPrefabPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/ChangCardUI.prefab");
            m_boxModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/ChangCardBox.prefab");
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            TableCardItemBody tableBody = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, objId).m_itemBody as TableCardItemBody;
            m_modelPath = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[tableBody.m_type].m_outModelPath;
            setTableItemAndPnt(tableBody, pntGo_);
        }

        override protected void updateLeftAttr(TableCardItemBody tableBody)
        {
            AuxLabel text = new AuxLabel(m_model.selfGo, "UIRoot/AttText");
            text.text = tableBody.m_attack.ToString();
            text = new AuxLabel(m_model.selfGo, "UIRoot/HpText");
            text.text = tableBody.m_hp.ToString();
        }

        override protected void modifyTex(GameObject go_, TableCardItemBody tableBody)
        {
            // 头像是每一个卡牌一个配置
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], tableBody.m_dzCardHeader);
            m_subTex[(int)CardSubPartType.eHeader].tex.selfGo = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_outHeaderSubModel);
            m_subTex[(int)CardSubPartType.eHeader].tex.texPath = path;
            m_subTex[(int)CardSubPartType.eHeader].tex.syncUpdateTex();
        }

        override protected void addHandle()
        {
            UtilApi.addEventHandle(gameObject(), onEntityClick);
        }
    }
}