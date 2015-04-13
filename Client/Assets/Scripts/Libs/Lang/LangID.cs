namespace SDK.Lib
{
    public enum LangID
    {
        zh_CN           // 中文
    }

    public enum LangTypeId
    {
        eLTLog,                 // 所有的日志信息
        eMsgRoute,              // 客户端自己的消息 Route
        eSelectHero,            // 角色选择
        eLogin,                 // 登陆
        eDZ,                    // 对战场景提示
        eLTTotal
    }

    // 对应的日志项，如果 xml 文件中删除一个，这里面也要删除一个，否则会对应不起来
    public enum LangLogID
    {
        eItem0,
        eItem1,
        eItem2,
        eItem3,
        eItem4,
        eItem5,
        eItem6,
        eItem7,
        eItem8,
        eItem9,
        eItem10,
        eItem11,
        eItem12,
        eItem13,
        eItem14,
        eItem15,
        eItem16,
        eItem17,
        eItem18,
        eItem19,
        eItem20,
        eItem21,
        eItem22,

        eLLogTotal
    }

    public enum LangMsgRouteID
    {
        eItem0,
        eItem1,
    }

    public enum LangSelectHero
    {
        eItem0,
        eItem1,
    }

    public enum LangLogin
    {
        eItem0,
        eItem1,
        eItem2,
        eItem3,
    }

    public enum LangDZ
    {
        eItem0,
        eItem1,
        eItem2,
        eItem3,
    }
}