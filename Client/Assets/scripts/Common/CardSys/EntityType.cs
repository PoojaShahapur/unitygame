namespace SDK.Common
{
    public enum EntityType
    {
        eETBtn,         // 点击商店的按钮
        eETShop,        // 商店
        eETShopSelectPack,        // 商店中的选择扩展包按钮
        eETShopClose,        // 商店关闭按钮
        eETOpen,
        eETMcam,
        eETwdscjm,
        eETdzmoshibtn,
    }

    public enum EntityTag   // 每一个类型中，有很多标签，表明不同的实例化
    {
        eETagShop,              // 商店
        eETagExtPack,           // 扩展包
        eETaggoback,            // 关闭扩展背包
        eETagwdscbtn,           // 打开我的收藏
        eETagdzmoshibtn,        // 点击对战模式
        eETaggoldbuy,           // 商城购买
    }
}