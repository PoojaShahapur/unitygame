namespace Game.UI
{
    public enum TuJianBtnEnum
    {
        eBtnBack,
        eBtnTotal
    }

    public enum TuJianCardNumPerPage            // 每一页卡牌的数量
    {
        eNum = 6,
        eRow = 2,
        eCol = 3,
    }

    public enum TuJianNumPerPage            // 总共套牌数量
    {
        eNum = 10,
    }

    public class TuJianPath
    {
        public const string GoTopEditCardPos = "UIRootNode/MidLay/TopEditCardPosGo";
        public const string BtnPrePage = "UIRootNode/TopLay/PrePageBtn";               // 前一页
        public const string BtnNextPage = "UIRootNode/TopLay/NextPageBtn";               // 后一页
        public const string TextPageNum = "UIRootNode/TopLay/PageBgImage/pagenum";               // 第一页

        public const string BtnJob0f = "UIRootNode/MidLay/LeftBtnList/JobBtn_0";           // 职业
        public const string BtnJob1f = "UIRootNode/MidLay/LeftBtnList/JobBtn_1";           // 职业
        public const string BtnJob2f = "UIRootNode/MidLay/LeftBtnList/JobBtn_2";           // 职业
        public const string BtnJob3f = "UIRootNode/MidLay/LeftBtnList/JobBtn_3";           // 职业

        public const string BtnFilter = "UIRootNode/MidLay/LeftBtnList/FilterBtn";        // 过滤
        public const string BtnShouCang = "UIRootNode/MidLay/LeftBtnList/ShouCangBtn";      // 收藏

        public const string PnlJob = "UIRootNode/MidLay/TopJobList";      // 职业面板
        public const string Job0f = "UIRootNode/MidLay/TopJobList/JobBtn_0";
        public const string Job1f = "UIRootNode/MidLay/TopJobList/JobBtn_1";
        public const string Job2f = "UIRootNode/MidLay/TopJobList/JobBtn_2";
        public const string Job3f = "UIRootNode/MidLay/TopJobList/JobBtn_3";

        public const string PnlFilter = "UIRootNode/MidLay/FilterPnl";      // 过滤面板
        public const string Filter0f = "UIRootNode/MidLay/FilterPnl/Btn_0";
        public const string Filter1f = "UIRootNode/MidLay/FilterPnl/Btn_1";
        public const string Filter2f = "UIRootNode/MidLay/FilterPnl/Btn_2";
        public const string Filter3f = "UIRootNode/MidLay/FilterPnl/Btn_3";
        public const string Filter4f = "UIRootNode/MidLay/FilterPnl/Btn_4";
        public const string Filter5f = "UIRootNode/MidLay/FilterPnl/Btn_5";
        public const string Filter6f = "UIRootNode/MidLay/FilterPnl/Btn_6";
        public const string Filter7f = "UIRootNode/MidLay/FilterPnl/Btn_7";
        public const string Filter8f = "UIRootNode/MidLay/FilterPnl/Btn_8";

        public const string BtnNewCardSet = "UIRootNode/MidLay/RightPnl/NewCardSetBtn";
        public const string BtnRet = "UIRootNode/MidLay/RightPnl/RetBtn";
        public const string TextCardSetCardCnt = "UIRootNode/MidLay/RightPnl/CardSetCardCntText";

        public const string CardSetListParent = "UIRootNode/MidLay/RightPnl/CardSetListGo";
        public const string CardSetListCont = "UIRootNode/MidLay/RightPnl/CardSetListGo/ScrollRect/Content";
        public const string CardSetCardListParent = "UIRootNode/MidLay/RightPnl/CardSetCardListGo";
        public const string CardSetCardListCon = "UIRootNode/MidLay/RightPnl/CardSetCardListGo/ScrollRect/Content";

        public const string CardSetPrefabPath = "UI/UITuJian/CardSet.prefab";
        public const string CardSetCardPrefabPath = "UI/UITuJian/CardSetCard.prefab";
    }
}