using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace SDK.Lib
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

        override public void initDieItemData(BeingEntity dieBeing, stRetRemoveBattleCardUserCmd msg)
        {
            base.initDieItemData(dieBeing, msg);

            m_being = dieBeing;
            m_dieEffectId = 14;         // 普通死亡
        }


        // 执行当前的受伤操作
        override public void execHurt(BeingEntity being)
        {
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 开始执行死亡 {0}", m_being.getDesc()));
            base.execHurt(being);
            being.behaviorControl.execHurt(this);
        }

        override public void onHurtExecEnd(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 当前死亡执行结束 {0}", m_being.getDesc()));
            base.onHurtExecEnd(dispObj);
        }
    }
}