namespace SDK.Common
{
	/**
	 * 界面ID定义
	 */
	public enum UIFormID
	{
        // 第二层
		eUIPack,             // 背包界面
        eUILogin,            // 登陆界面
        eUIHeroSelect,       // 角色选择界面界面
        eUIBlurBg,           // 模糊背景界面
        eUITest,             // 测试界面
        eUIDZ,               // 对战界面
        eUIExtraOp,          // 操作界面
        eUIChat,             // 聊天窗口
        
        eUIJobSelect,        // 职业选择界面
        eUITuJian,           // 图鉴选择界面
        eUIMain,             // 主界面
        eUIHero,             // 英雄
        eUIOpenPack,         // 开包

        // 第四层
        eUIInfo,             // 信息窗口

        // 顶层
        eUILogicTest,        // 逻辑测试窗口
	}

    public enum UILayerID
    {
        BtmLayer,                   // 最低层啊，一般不放东西，以备不时之需，目前放模糊的界面
        FirstLayer,                 // 第一层，聊天之类的主界面窗口
        SecondLayer,                // 第二层，主要是功能性的界面，弹出需要关闭的界面
        ThirdLayer,                 // 第三层，新手引导层
        ForthLayer,                 // 第四层，提示窗口层
        TopLayer,                   // 最顶层，一般不放东西，以备不时之需

		MaxLayer
    }

    public enum WinIDCnt
    {
        eIDCnt = 500,           // 每一个窗口中可以占用的 ID 数量
    }
}