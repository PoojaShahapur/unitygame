using Game.Msg;

namespace FightCore
{
    public class ComHurtItem : HurtItemBase
    {
        protected int m_hurtEffectId;   // 攻击受伤 Id

        public ComHurtItem(EHurtType hurtType) :
            base(hurtType)
        {

        }

        public int hurtEffectId
        {
            get
            {
                return m_hurtEffectId;
            }
            set
            {
                m_hurtEffectId = value;
            }
        }

        // 执行当前的受伤操作
        override public void execHurt(SceneCardBase card)
        {
            card.behaviorControl.execHurt(this);
        }

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_hurtEffectId = 4;         // 普通被击，根据攻击力播放不同的特效，并且播放掉血特效
        }
    }
}