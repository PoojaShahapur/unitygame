using UnityEngine;

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

        public const string SelfCardHandRadiusGO = "PlaceHolder/SelfCardHandRadiusGO";
        public const string EnemyCardHandRadiusGO = "PlaceHolder/EnemyCardHandRadiusGO";
        public const string SelfCardCommonRadiusGO = "PlaceHolder/SelfCardCommonRadiusGO";
        public const string EnemyCardCommonRadiusGO = "PlaceHolder/EnemyCardCommonRadiusGO";

        public const string ArrowStartPosGO = "PlaceHolder/ArrowStartPos";
        public const string ArrowListGO = "PlaceHolder/ArrowStartPos/ArrowList";

        public const string StartGO = "dz/StartBtn";
        public const string EndBtn = "EndBtn";

        public const string SelfMpText = "dz/UIGo/Canvas/SelfMPText";
        public const string EnemyMpText = "dz/UIGo/Canvas/EnemyMPText";
        public const string SelfMpList = "dz/SelfGo/MpListGo";
        public const string EnemyMpList = "dz/EnemyGo/MpListGo";
        public const string HistoryGo = "HistoryGo";
        public const string TimerGo = "TimerGo";

        public const string CollideBG = "bujian_zhanchang";
        //public const string FirstInitCardGO = "PlaceHolder/FirstInitCardGO";         // 第一个场景卡牌位置
        //public const string SecondInitCardGO = "PlaceHolder/SecondInitCardGO";        // 第二个场景卡牌位置
        //public const string ThirdInitCardGO = "PlaceHolder/ThirdInitCardGO";         // 第三个场景卡牌位置
        //public const string ForthInitCardGO = "PlaceHolder/ForthInitCardGO";         // 第四个场景卡牌位置
    }

    public enum EnSceneDZText
    {
        eSelfMp,    // 自己的 MP 
        eEnemyMp,    // 自己的 MP 

        eTotal
    }

    public class SceneDZCV
    {
        public const int OUT_CARD_TOTAL = 5;                        // 出牌区域最多牌的数量
        public const float HAND_CARD_WIDTH = 2.0f;                  // 手牌宽度
        public const float COMMON_CARD_WIDTH = 2.0f;                // 场牌宽度
        public const float HAND_YDELTA = 0.1f;                      // 手牌的时候 Y Delta 值
        public const float DRAG_YDELTA = HAND_YDELTA * 10;                      // 拖动 Y Delta 值

        public static Vector3 SMALLFACT = new Vector3(0.5f, 0.5f, 0.5f);    // 小牌时的缩放因子
        public static Vector3 BIGFACT = new Vector3(1.2f, 1.2f, 1.2f);      // 大牌时候的因子
        public const uint WHITE_CARDID = uint.MaxValue - 1;      // 白色的占位卡牌 ID
        public const uint BLACK_CARD_ID = uint.MaxValue;        // 敌人背面卡 ID
    }
}