namespace Game.UI
{
    public class CVSceneDZPath
    {
        public const string TurnBtn = "dz/turn";
        public const string SelfHero = "ZhanChang/yingxiong_zhanchang";
        public const string EnemyHero = "ZhanChang/yingxiong_zhanchang001";
        public const string LuckyCoin = "luckycoin";
        public const string SelfTurnTip = "youturntip";
        public const string SelfCardFullTip = "SelfCardFullTip";
        public const string SelfCardFullTipText = "Canvas/Text";
        public const string MyCardDeap = "ZhanChang/paizu_zhanchang";
        public const string EnemyCardDeap = "ZhanChang/paizu_zhanchang";

        public const string CenterGO = "dzban";         // 中心位置，所有的牌的相对位置都是相对这个位置

        public const string SelfStartCardCenterGO = "dzban/SelfStartCard";
        public const string EnemyStartCardCenterGO = "dzban/EnemyStartCard";
        public const string SelfOutCardCenterGO = "dzban/SelfOutCardCenter";
        public const string EnemyOutCardCenterGO = "dzban/EnemyOutCardCenter";
        public const string SelfCardCenterGO = "dzban/SelfCardCenter";
        public const string EnemyCardCenterGO = "dzban/EnemyCardCenter";
        public const string SelfEquipGO = "dzban/SelfEquip";
        public const string EnemyEquipGO = "dzban/EnemyEquip";
        public const string SelfSkillGO = "dzban/SelfSkill";
        public const string EnemySkillGO = "dzban/EnemySkill";

        public const string ArrowStartPosGO = "dzban/ArrowStartPos";
        public const string ArrowListGO = "dzban/ArrowStartPos/ArrowList";

        public const string StartGO = "dz/StartBtn";
        public const string EndBtn = "EndBtn";

        public const string SelfMpText = "dz/UIGo/Canvas/SelfMPText";
        public const string EnemyMpText = "dz/UIGo/Canvas/EnemyMPText";
        public const string SelfMpList = "dz/SelfGo/MpListGo";
        public const string EnemyMpList = "dz/EnemyGo/MpListGo";
        public const string HistoryGo = "HistoryGo";
        public const string TimerGo = "TimerGo";

        public const string CollideBG = "dz/di/di";
    }

    public enum EnSceneDZText
    {
        eSelfMp,    // 自己的 MP 
        eEnemyMp,    // 自己的 MP 

        eTotal
    }

    public class SceneDZCV
    {
        public const int OUT_CARD_TOTAL = 5;        // 出牌区域最多牌的数量
    }
}