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
        eUITuJianTop,        // 图鉴上层
        eUIMain,             // 主界面
        eUIHero,             // 英雄
        eUIOpenPack,         // 开包
        eUIShop,         //商店

        // 第四层
        eUIInfo,             // 信息窗口

        // 顶层
        eUILogicTest,        // 逻辑测试窗口
	}

    public enum WinIDCnt
    {
        eIDCnt = 500,           // 每一个窗口中可以占用的 ID 数量
    }
}