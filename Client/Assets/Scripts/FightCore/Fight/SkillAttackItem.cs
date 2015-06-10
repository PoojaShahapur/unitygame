﻿using Game.Msg;
using SDK.Common;

namespace FightCore
{
    /**
     * @brief 技能攻击不会发生移动过去的情况，仅仅是将法术卡拖到场景中，然后释放，就出发技能，可能会有功能准备特效和攻击特效
     */
    public class SkillAttackItem : AttackItemBase
    {
        protected uint m_skillId;
        protected TableSkillItemBody m_skillTableItem;
        protected MList<uint> m_hurtIdList;     // 被击者 this id 列表

        public SkillAttackItem(EAttackType attackType) :
            base(attackType)
        {
            m_hurtIdList = new MList<uint>();
        }

        public MList<uint> hurtIdList
        {
            get
            {
                return m_hurtIdList;
            }
            set
            {
                m_hurtIdList = value;
            }
        }

        public uint skillId
        {
            get
            {
                return m_skillId;
            }
            set
            {
                m_skillId = value;
                m_skillTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, m_skillId).m_itemBody as TableSkillItemBody;
            }
        }

        public TableSkillItemBody skillTableItem
        {
            get
            {
                return m_skillTableItem;
            }
            set
            {
                m_skillTableItem = value;
            }
        }

        override public void execAttack(SceneCardBase card)
        {
            card.behaviorControl.execAttack(this);
        }

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            skillId = msg.dwMagicType;
            foreach(var item in msg.defList)
            {
                m_hurtIdList.Add(item.qwThisID);
            }
        }
    }
}