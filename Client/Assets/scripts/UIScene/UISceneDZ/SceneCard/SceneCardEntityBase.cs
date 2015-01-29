using SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 场景中卡牌基类
     */
    public class SceneCardEntityBase : LSBehaviour
    {
        protected SceneCardItem m_sceneCardItem;

        //public void setCardData(SceneCardItem dataitem)
        //{
        //    m_sceneCardItem = dataitem;
        //    updateCardData();
        //}

        public SceneCardItem sceneCardItem
        {
            get
            {
                return m_sceneCardItem;
            }
            set
            {
                m_sceneCardItem = value;
                if (m_sceneCardItem != null)
                {
                    updateCardDataChange();
                    updateCardDataNoChange();
                }
            }
        }

        public void destroy()
        {
            UtilApi.Destroy(gameObject);
            m_sceneCardItem = null;
        }

        // 更新卡牌属性，这个主要更改卡牌经常改变的属性
        public virtual void updateCardDataChange()
        {
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_COMMON || m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_HAND)
                {
                    Text text;
                    text = UtilApi.getComByP<Text>(gameObject, "attack/Canvas/Text");       // 攻击
                    text.text = m_sceneCardItem.m_svrCard.damage.ToString();
                    text = UtilApi.getComByP<Text>(gameObject, "cost/Canvas/Text");         // Magic
                    text.text = m_sceneCardItem.m_svrCard.mpcost.ToString();
                    text = UtilApi.getComByP<Text>(gameObject, "health/Canvas/Text");       // HP
                    text.text = m_sceneCardItem.m_svrCard.hp.ToString();
                }
            }
        }

        // 这个主要是更新卡牌不经常改变的属性
        public virtual void updateCardDataNoChange()
        {
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.m_cardArea != CardArea.CARDCELLTYPE_HERO)
                {
                    Text text;
                    text = UtilApi.getComByP<Text>(gameObject, "name/Canvas/Text");         // 名字
                    text.text = m_sceneCardItem.m_cardTableItem.m_name;

                    text = UtilApi.getComByP<Text>(gameObject, "description/Canvas/Text");  // 描述
                    string desc = "";
                    if (m_sceneCardItem.m_cardTableItem.m_chaoFeng == 1)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.ChaoFeng;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_chongFeng == 1)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.ChongFeng;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_fengNu == 1)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.FengNu;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_qianXing == 1)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.QianXing;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_shengDun == 1)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.ShengDun;
                    }

                    if (m_sceneCardItem.m_cardTableItem.m_magicConsume > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.MoFaXiaoHao;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_attack > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.GongJiLi;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_hp > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.Xueliang;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_Durable > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.NaiJiu;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_mpAdded > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.FaShuShangHai;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_guoZai > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        desc += TableCardAttrName.GuoZai;
                    }

                    TableSkillItemBody tableSkillItem;
                    // 技能
                    if (m_sceneCardItem.m_cardTableItem.m_faShu > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        tableSkillItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, (uint)m_sceneCardItem.m_cardTableItem.m_faShu).m_itemBody as TableSkillItemBody;
                        desc += tableSkillItem.m_desc;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_zhanHou > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        tableSkillItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, (uint)m_sceneCardItem.m_cardTableItem.m_zhanHou).m_itemBody as TableSkillItemBody;
                        desc += tableSkillItem.m_desc;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_wangYu > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        tableSkillItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, (uint)m_sceneCardItem.m_cardTableItem.m_wangYu).m_itemBody as TableSkillItemBody;
                        desc += tableSkillItem.m_desc;
                    }
                    if (m_sceneCardItem.m_cardTableItem.m_jiNu > 0)
                    {
                        if (desc.Length > 0)
                        {
                            desc += "\n";
                        }
                        tableSkillItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, (uint)m_sceneCardItem.m_cardTableItem.m_jiNu).m_itemBody as TableSkillItemBody;
                        desc += tableSkillItem.m_desc;
                    }

                    text.text = desc;
                }
            }
        }
    }
}