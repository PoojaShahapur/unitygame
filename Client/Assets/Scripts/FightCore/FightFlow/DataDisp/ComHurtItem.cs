using Game.Msg;
using SDK.Common;
using SDK.Lib;

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
            Ctx.m_instance.m_logSys.log("[Fight] 开始执行普通被击");
            base.execHurt(card);
            card.behaviorControl.execHurt(this);
        }

        override public void onHurtExecEnd(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.log("[Fight] 当前普通被击执行结束");
            base.onHurtExecEnd(dispObj);
        }

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_hurtEffectId = 7;         // 普通被击，根据攻击力播放不同的特效，并且播放掉血特效
        }
    }
}