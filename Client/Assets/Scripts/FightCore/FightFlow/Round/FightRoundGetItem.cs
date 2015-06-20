using Game.Msg;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 获取一张手牌
     */
    public class FightRoundGetItem : FightRoundItemBase
    {
        protected stNotifyBattleCardPropertyUserCmd m_attCmd;       // 攻击数据

        public FightRoundGetItem(SceneDZData data) :
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