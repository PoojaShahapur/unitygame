namespace FightCore
{
    /**
     * @brief 战斗常亮
     */
    public enum EAttackType
    {
        eCommon,    // 普通攻击
        eSkill,     // 技能攻击
    }

    public enum EAttackRangeType
    {
        eSingle,        // 单攻
        eMul,           // 多功
    }

    public enum EHurtType
    {
        eCommon,
        eSkill,
    }

    public enum EHurtItemState
    {
        eDisable,
        eEnable,            // 处于这个状态的才能计算
    }

    public enum EHurtExecState
    {
        eNone,              // 没有执行
        eExec,              // 这个在执行中
        eEnd,               // 执行结束
    }
}