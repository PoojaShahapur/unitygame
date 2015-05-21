namespace Game.UI
{
    public enum OpenPackGo
    {
        eCardPackLayer,
        eOpenPackLayer,

        eTotal
    }

    public enum OpenPackBtnEnum
    {
        eBtnBack,           // 返回
		eBtnShop,           //商店

        eBtnTotal
    }

    public enum CardBtnEnum
    {
        ePackBtn_0,
        ePackBtn_1,
        ePackBtn_2,

        eOpenedPackBtn_0,
        eOpenedPackBtn_1,
        eOpenedPackBtn_2,
        eOpenedPackBtn_3,
        eOpenedPackBtn_4,

        eCardBtnTotal
    }

    public class OpenPackPath
    {
        public const string CardPackLayer = "CardPackLayer";
        public const string OpenPackLayer = "OpenPackLayer";

        public const string RetBtn = "CardPackLayer/RetBtn";
        public const string ShopBtn = "CardPackLayer/ShopBtn";

        public const string PackBtn_0 = "CardPackLayer/PackBtn_0";
        public const string PackBtn_1 = "CardPackLayer/PackBtn_1";
        public const string PackBtn_2 = "CardPackLayer/PackBtn_2";

        public const string OpenedPackBtn_0 = "OpenPackLayer/OpenedPackBtn_0";
        public const string OpenedPackBtn_1 = "OpenPackLayer/OpenedPackBtn_1";
        public const string OpenedPackBtn_2 = "OpenPackLayer/OpenedPackBtn_2";
        public const string OpenedPackBtn_3 = "OpenPackLayer/OpenedPackBtn_3";
        public const string OpenedPackBtn_4 = "OpenPackLayer/OpenedPackBtn_4";
    }
}