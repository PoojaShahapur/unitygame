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

        public const string BtnJob = "UIRootNode/MidLay/LeftBtnList/JobBtn";           // 职业

        public const string BtnFilter = "UIRootNode/MidLay/LeftBtnList/FilterBtn";        // 过滤
        public const string BtnShouCang = "UIRootNode/MidLay/LeftBtnList/ShouCangBtn";      // 收藏

        public const string PnlJob = "UIRootNode/MidLay/TopJobList";      // 职业面板

        public const string PnlFilter = "UIRootNode/MidLay/FilterPnl";      // 过滤面板

        public const string BtnNewCardSet = "UIRootNode/MidLay/RightPnl/NewCardSetBtn";
        public const string BtnRet = "UIRootNode/MidLay/RightPnl/RetBtn";
        public const string TextCardSetCardCnt = "UIRootNode/MidLay/RightPnl/CardSetCardCntText";

        public const string CardSetListParent = "UIRootNode/MidLay/RightPnl/CardSetListGo";
        public const string CardSetListCont = "UIRootNode/MidLay/RightPnl/CardSetListGo/ScrollRect/Content";
        public const string CardSetCardListParent = "UIRootNode/MidLay/RightPnl/CardSetCardListGo";
        public const string CardSetCardListCon = "UIRootNode/MidLay/RightPnl/CardSetCardListGo/ScrollRect/Content";

        public const string CardSetNameText = "NameText";

        public const string CardSetPrefabPath = "UI/UITuJian/CardSet.prefab";
        public const string CardSetCardPrefabPath = "UI/UITuJian/CardSetCard.prefab";

        public const string NumImage = "NumImage";
        public const string MpNumText = "MpNumText";
        public const string NameText = "NameText";
    }
}