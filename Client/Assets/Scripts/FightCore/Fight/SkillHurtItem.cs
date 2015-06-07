using Game.Msg;

namespace FightCore
{
    /**
     * @brief 技能被击，仅仅是播放掉血特效，没有被击特效
     */
    public class SkillHurtItem : HurtItemBase
    {
        public SkillHurtItem(EHurtType hurtType) : 
            base(hurtType)
        {

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