namespace Game.UI
{
    public enum SceneWDSCBtnEnum
    {
        eBtnBack,
        eBtnTotal
    }

    public enum SCCardNumPerPage            // 每一页卡牌的数量
    {
        eNum = 8,
    }

    public enum SCTPNumPerPage            // 总共套牌数量
    {
        eNum = 10,
    }

    public class SceneSCPath
    {
        public const string WDSCGO = "wdscjm";      // 我的收藏界面根结点
        public const string WDSCSceneUI = "wdscjm/SceneUIRoot";      // 我的收藏界面 UI 根结点
        public const string WDSCPage = "wdscjm/page";      // 我的收藏界面页
        public const string BtnClose = "wdscjm/wdscfh/btn";                         // 关闭按钮

        public const string SetBtn = "wdscjm/setbtn";               // 右边根节点
        public const string BtnAddCardSet = "wdscjm/setbtn/newSetBtn";               // 新增套牌
        public const string BtnAddCardSetList = "wdscjm/setbtn/CardGroupList";               // 卡组列表
        public const string BtnAddCardList = "wdscjm/setbtn/LeftCardList";               // 左边卡牌列表

        public const string BtnPrePage = "TopLay/PrePageBtn";               // 前一页
        public const string BtnNextPage = "TopLay/NextPageBtn";               // 后一页
        public const string TextPageNum = "TopLay/PageBgImage/pagenum";               // 第一页

        public const string BtnJob0f = "MidLay/LeftBtnList/JobBtn_0";           // 职业
        public const string BtnJob1f = "MidLay/LeftBtnList/JobBtn_1";           // 职业
        public const string BtnJob2f = "MidLay/LeftBtnList/JobBtn_2";           // 职业
        public const string BtnJob3f = "MidLay/LeftBtnList/JobBtn_3";           // 职业
        public const string BtnJob4f = "MidLay/LeftBtnList/JobBtn_4";           // 职业
        public const string BtnFilter = "MidLay/LeftBtnList/FilterBtn";        // 过滤
        public const string BtnShouCang = "MidLay/LeftBtnList/ShouCangBtn";      // 收藏

        public const string PnlJob = "MidLay/TopJobList";      // 职业面板
        public const string Job1f = "MidLay/TopJobList/JobBtn_1";
        public const string Job2f = "MidLay/TopJobList/JobBtn_2";
        public const string Job3f = "MidLay/TopJobList/JobBtn_3";
        public const string Job4f = "MidLay/TopJobList/JobBtn_4";

        public const string PnlFilter = "MidLay/FilterPnl";      // 过滤面板
        public const string Filter0f = "MidLay/FilterPnl/Btn_0";
        public const string Filter1f = "MidLay/FilterPnl/Btn_1";
        public const string Filter2f = "MidLay/FilterPnl/Btn_2";
        public const string Filter3f = "MidLay/FilterPnl/Btn_3";
        public const string Filter4f = "MidLay/FilterPnl/Btn_4";
        public const string Filter5f = "MidLay/FilterPnl/Btn_5";
        public const string Filter6f = "MidLay/FilterPnl/Btn_6";
        public const string Filter7f = "MidLay/FilterPnl/Btn_7";
        public const string Filter8f = "MidLay/FilterPnl/Btn_8";

        public const string BtnNewCardSet = "MidLay/RightPnl/NewCardSetBtn";
        public const string BtnRet = "MidLay/RightPnl/RetBtn";
        public const string TextCardSetCardCnt = "MidLay/RightPnl/CardSetCardCntText";

        public const string GoTopEditCardPos = "wdscjm/setbtn/TopEditCardPosGo";

        // SceneUI 加载进来设置的名字
        public const string SceneUIName = "SceneUI";
    }
}