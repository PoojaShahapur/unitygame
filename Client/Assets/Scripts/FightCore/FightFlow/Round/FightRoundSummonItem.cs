using Game.Msg;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 召唤，在场上召唤牌
     */
    public class FightRoundSummonItem : FightRoundItemBase
    {
        protected stNotifyBattleCardPropertyUserCmd m_attCmd;       // 攻击数据

        public FightRoundSummonItem(SceneDZData data) :
            base(data)
        {

        }

        override public void psstNotifyBattleCardPropertyUserCmd(stNotifyBattleCardPropertyUserCmd msg)
        {
            m_attCmd = msg;
        }

        override public void processOneAttack()
        {

        }

        protected void onOneAttackAndHurtEndHandle(IDispatchObject dispObj)
        {
            m_OneAttackAndHurtEndDisp.dispatchEvent(this);
        }
    }
}