namespace SDK.Common
{
    /**
     * @brief 玩家属性常量定义
     */

    // 玩家职业
    public enum EnPlayerCareer
    {
        HERO_OCCUPATION_NONE = 0,   //NONE  
        HERO_OCCUPATION_1 = 1,      //职业1 
        HERO_OCCUPATION_2 = 2,      //职业2
        HERO_OCCUPATION_3 = 3,      //职业3

        //HERO_OCCUPATION_MAX,        //职业MAX
        //ePCOne,
        ePCTotal
    }

    // 场景卡牌类型，主要是显示资源基本数据
    public enum EnSceneCardType
    {
        eScene_minion, 
        eScene_ability, 
        eScene_weapon,
        eScene_Total
    }
}