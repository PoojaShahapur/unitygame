namespace FightCore
{
    public class CVSceneDZPath
    {
        public const string TurnBtn = "jieshu_zhanchang";
        //public const string LuckyCoin = "luckycoin";
        public const string SelfTurnTip = "youturntip";
        public const string SelfCardFullTip = "SelfCardFullTip";
        public const string SelfCardFullTipText = "Canvas/Text";
        public const string MyCardDeap = "paiku_zhanchang";
        public const string EnemyCardDeap = "paiku_zhanchang";

        public const string CenterGO = "dzban";         // 中心位置，所有的牌的相对位置都是相对这个位置

        public const string SelfStartCardCenterGO = "PlaceHolder/SelfStartCard";
        public const string EnemyStartCardCenterGO = "PlaceHolder/EnemyStartCard";
        public const string SelfOutCardCenterGO = "PlaceHolder/SelfOutCardCenter";
        public const string EnemyOutCardCenterGO = "PlaceHolder/EnemyOutCardCenter";
        public const string SelfCardCenterGO = "PlaceHolder/SelfCardCenter";
        public const string EnemyCardCenterGO = "PlaceHolder/EnemyCardCenter";
        public const string SelfEquipGO = "PlaceHolder/SelfEquip";
        public const string EnemyEquipGO = "PlaceHolder/EnemyEquip";
        public const string SelfSkillGO = "PlaceHolder/SelfSkill";
        public const string EnemySkillGO = "PlaceHolder/EnemySkill";
        public const string SelfHeroGO = "PlaceHolder/SelfHero";
        public const string EnemyHeroGO = "PlaceHolder/EnemyHero";

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