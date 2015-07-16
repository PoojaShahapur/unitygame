namespace SDK.Lib
{
    /**
     * @brief 战斗常亮
     */
    public enum EImmeAttackType
    {
        eCommon,    // 普通攻击
        eSkill,     // 技能攻击
    }

    public enum EImmeAttackRangeType
    {
        eSingle,        // 单攻
        eMul,           // 多功
    }

    public enum EImmeHurtType
    {
        eCommon,        // 普通被击
        eSkill,         // 技能被击
        eDie,           // 死亡
    }

    public enum EImmeHurtItemState
    {
        eDisable,
        eEnable,            // 处于这个状态的才能计算
    }

    public enum EImmeHurtExecState
    {
        eNone,              // 没有执行
        eStartExec,         // 将要执行
        eExecing,           // 这个在执行中
        eEnd,               // 执行结束
    }
}