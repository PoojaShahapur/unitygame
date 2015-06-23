using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 技能被击，仅仅是播放掉血特效，没有被击特效
     */
    public class SkillHurtItem : HurtItemBase
    {
        protected uint m_skillId;
        protected TableSkillItemBody m_skillTableItem;

        public SkillHurtItem(EHurtType hurtType) : 
            base(hurtType)
        {

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

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_svrCard = def.sceneCardItem.svrCard;  // 保存这次被击的属性，可能这个会被后面的给改掉

            skillId = msg.dwMagicType;
            this.delayTime = m_skillTableItem.m_effectMoveTime;

            // 技能被击伤血是血量值差
            m_bDamage = hasDamageHp(msg.m_origDefObject, def.sceneCardItem.svrCard);
            if (m_bDamage)
            {
                m_damage = (int)(msg.m_origDefObject.hp - def.sceneCardItem.svrCard.hp);
            }

            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 被击者掉血 {0}", m_damage));
        }

        // 执行当前的受伤操作
        override public void execHurt(SceneCardBase card)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 开始执行技能被击");
            base.execHurt(card);
            card.behaviorControl.execHurt(this);
        }

        override public void onHurtExecEnd(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 当前技能被击执行结束");
            base.onHurtExecEnd(dispObj);
        }

        // 计算是否是伤血
        protected bool hasDamageHp(t_Card src, t_Card dest)
        {
            if (src.hp > dest.hp)        // HP 减少
            {
                if (src.maxhp == dest.maxhp)         // 不是由于技能导致的将这两个值减少并且设置成同样的值，就是伤血
                {
                    return true;
                }
            }

            return false;
        }
    }
}