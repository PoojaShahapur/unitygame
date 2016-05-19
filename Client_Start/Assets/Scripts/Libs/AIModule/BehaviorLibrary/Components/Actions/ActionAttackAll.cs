using FightCore;
using Game.UI;

namespace BehaviorLibrary
{
    /**
     * @brief 所有的攻击暂时都在这里面
     */
    public class ActionAttackAll : Action
    {
        public ActionAttackAll()
            : base(null)
        {
            base.actionFunc = onExecAction;
        }

        protected BehaviorReturnCode onExecAction()
        {
            SceneCardBase attackCard = behaviorTree.blackboardData.GetData(BlackboardKey.PSCARD) as SceneCardBase;
            if (attackCard != null)
            {
                attackCard.behaviorControl.updateAttack();      // 更新攻击
                attackCard.behaviorControl.updateHurt();        // 更新受伤
            }

            return BehaviorReturnCode.Success;
        }
    }
}