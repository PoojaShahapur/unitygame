namespace Game.UI
{
    public enum PackBtnNum
    {
        eBtnPack1,//卡包1
        eBtnPack5,//卡包5
        eBtnPack10,//卡包10
        eBtnPack20,//卡包20

        ePackBtnTotal
    }
    public enum ShopBtnNum
    {
        eBtnPack1XZ,//卡包1选中
        eBtnPack5XZ,//卡包5选中
        eBtnPack10XZ,//卡包10选中
        eBtnPack20XZ,//卡包20选中

        eBtnBack,//返回
        eBtnBuy,//购买
        eBtnOk,//确定充值
        eBtnCancel,//取消充值

        eBtnTotal
    }

    public enum ShopTxtPriceNum
    {
        eTxtPrice1,//卡包1
        eTxtPrice5,//卡包5
        eTxtPrice10,//卡包10
        eTxtPrice20,//卡包20

        eTxtTotal
    }

    public class ShopComPath
    {
        public const string BtnBack = "BtnBack";
        public const string BtnBuy = "BtnBuy";
        public const string BtnPack1 = "CardPack/BtnPack1";
        public const string BtnPack5 = "CardPack/BtnPack5";
        public const string BtnPack10 = "CardPack/BtnPack10";
        public const string BtnPack20 = "CardPack/BtnPack20";
        public const string BtnPack1XZ = "CardPack/BtnPack1XZ";
        public const string BtnPack5XZ = "CardPack/BtnPack5XZ";
        public const string BtnPack10XZ = "CardPack/BtnPack10XZ";
        public const string BtnPack20XZ = "CardPack/BtnPack20XZ";
        public const string TextGoldNum = "GoldNum";
        public const string TxtPrice1 = "PriceText/Price1";
        public const string TxtPrice5 = "PriceText/Price5";
        public const string TxtPrice10 = "PriceText/Price10";
        public const string TxtPrice20 = "PriceText/Price20";
        public const string BtnOk = "NoGoldTip/BtnOk";
        public const string BtnCancel = "NoGoldTip/BtnCancel";
    }
}
