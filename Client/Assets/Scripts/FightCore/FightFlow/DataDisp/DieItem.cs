using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 死亡项
     */
    public class DieItem : HurtItemBase
    {
        protected int m_dieEffectId;   // 死亡特效 Id

        public DieItem(EHurtType hurtType) : 
            base(hurtType)
        {

        }

        public int dieEffectId
        {
            get
            {
                return m_dieEffectId;
            }
            set
            {
                m_dieEffectId = value;
            }
        }

        override public void initDieItemData(SceneCardBase dieCard, stRetRemoveBattleCardUserCmd msg)
        {
            base.initDieItemData(dieCard, msg);

            m_dieEffectId = 14;         // 普通死亡
        }


        // 执行当前的受伤操作
        override public void execHurt(SceneCardBase card)
        {
            Ctx.m_instance.m_logSys.log("[Fight] 开始执行死亡");
            base.execHurt(card);
            card.behaviorControl.execHurt(this);
        }

        override public void onHurtExecEnd(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.log("[Fight] 当前死亡执行结束");
            base.onHurtExecEnd(dispObj);
        }
    }
}