using SDK.Common;

namespace BehaviorLibrary
{
    /**
     * @brief 空闲条件
     */
    public class ConditionIdle : Condition
    {
        public ConditionIdle()
            : base(null)
        {
            boolFunc = onExecBoolFunc;
        }

        protected bool onExecBoolFunc()
        {
            // 如果是 Idle 状态就需要
            //if (m_behaviorTree.inputParam.beingEntity.aiLocalState.behaviorState == BehaviorState.BSIdle)
            //{
                // 目前测试，直接进入徘徊装填
                //m_behaviorTree.inputParam.beingEntity.aiLocalState.behaviorState = BehaviorState.BSWander;
            //}

            return true;
        }
    }
}