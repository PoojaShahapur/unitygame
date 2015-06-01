namespace Game.UI
{
    public enum CardSceneState
    {
        eInplace,           // 在原地

        eInplace2DestStart, // 开始向目标移动
        eInplace2Desting,   // 原地到目标中
        eInplace2Dested,    // 移动到目标

        eAttackStart,       // 攻击开始
        eAttacking,         // 攻击中
        eAttacked,          // 攻击结束

        eDest2InplaceStart,    // 目的地会原地开始
        eDest2Inplaceing,      // 目的地会原地中
        eDest2Inplaced,      // 目的地会原地结束

        eHurtStart,       // 受伤开始
        eHurting,         // 受伤中
        eHurted,          // 受伤结束
    }
}