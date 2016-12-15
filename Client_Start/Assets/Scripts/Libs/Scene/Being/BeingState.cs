namespace SDK.Lib
{
    /**
     * @brief Being 状态
     */
    public enum BeingState
    {
        BSIdle,         // 空闲状态
        BSWalk,         // 走状态
        BSRun,          // 跑状态
        BSSeparation,   // 分离状态
        BSBirth,        // 出生状态
    }

    /**
     * @brief Being 子状态
     */
    public enum BeingSubState
    {
        
    }

    /**
     * @brief 各种动作状态 Id ，通常一个状态可能对应多个动作。动作编码是这样编码的，三位数，从 100 - 999 ，如果是动作过渡，条件是这样的，就是源动作 Id * 100 + 目的动作 Id ，例如源动作 234 ，目的动作是 678 ，那么最终的转换条件是 234 * 100 + 678 = 234678 ，这个就是转换的条件，凡是单独的三位数，都是从 Default 状态转换到目标状态的，例如一个动作编号是 123 ，那么 123 这个条件就是直接到 123 这个动作的条件
     */
    public enum eBeingActId
    {
        ActIdle,
        ActWalk,
        ActRun
    }

    /**
     * @brief Being 状态过渡
     */
    public class BeingStateTransit
    {
        public eBeingActId convState2Act(BeingState state)
        {
            return eBeingActId.ActIdle;
        }
    }

    /**
     * @brief 移动方式
     */
    public enum MoveWay
    {
        eNone,          // 没有移动
        eAutoPathMove,  // 自动寻路
        eIOControlMove, // IO 控制移动
        eSeparateMove,  // 分离移动
        eBirthMove,     // 出生移动
    }
}