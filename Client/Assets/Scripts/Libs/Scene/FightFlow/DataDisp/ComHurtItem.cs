using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace SDK.Lib
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
        override public void execHurt(BeingEntity being)
        {
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 开始执行普通被击 {0}", m_being.getDesc()));
            base.execHurt(being);
            being.behaviorControl.execHurt(this);
        }

        override public void onHurtExecEnd(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 当前普通被击执行结束 {0}", m_being.getDesc()));
            base.onHurtExecEnd(dispObj);
        }

        override public void initItemData(BeingEntity att, BeingEntity def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            // 普通被击伤血是对方的伤血值，不是血量的减少
            m_bDamage = true;
            if (m_bDamage)
            {
                m_damage = 10;
            }

            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 被击者掉血 {0}", m_damage));

            m_hurtEffectId = 7;         // 普通被击，根据攻击力播放不同的特效，并且播放掉血特效
        }
    }
}