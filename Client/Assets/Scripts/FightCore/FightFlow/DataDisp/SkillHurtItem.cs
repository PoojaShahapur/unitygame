using Game.Msg;
using SDK.Common;

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
        }

        // 执行当前的受伤操作
        override public void execHurt(SceneCardBase card)
        {
            card.behaviorControl.execHurt(this);
        }
    }
}