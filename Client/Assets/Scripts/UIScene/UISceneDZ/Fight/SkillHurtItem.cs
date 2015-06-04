using Game.Msg;

namespace Game.UI
{
    /**
     * @brief 技能被击，仅仅是播放掉血特效，没有被击特效
     */
    public class SkillHurtItem : HurtItemBase
    {
        protected bool m_bDamage;       // 是否是伤血，可能是回血

        public bool bDamage
        {
            get
            {
                return m_bDamage;
            }
            set
            {
                m_bDamage = value;
            }
        }

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {

        }

        // 执行当前的受伤操作
        override public void execHurt(SceneCardBase card)
        {
            card.behaviorControl.execHurt(this);
        }
    }
}