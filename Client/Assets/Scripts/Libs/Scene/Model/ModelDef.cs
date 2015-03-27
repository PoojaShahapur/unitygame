namespace SDK.Lib
{
    /**
     * @brief 模型定义，目前玩家分这几种， npc 怪物只有一个模型 
     */
    public enum PlayerModelDef
    {
        eModelHead = 0,         // 头
        eModelChest = 1,        // 胸
        eModelWaist = 2,        // 腰
        eModelLeg = 3,          // 腿

        eModelFoot = 4,         // 脚
        eModelArm = 5,          // 胳膊
        eModelHand = 6,         // 手
        eModelTotal
    }

    public enum NpcModelDef
    {
        eModelBody = 0,
        eModelTotal
    }

    public enum MonstersModelDef
    {
        eModelBody = 0,
        eModelTotal
    }
}