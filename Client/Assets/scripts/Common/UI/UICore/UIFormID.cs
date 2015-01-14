namespace SDK.Common
{
	/**
	 * 界面ID定义
	 */
	public enum UIFormID
	{
		UIPack,             // 背包界面
        UILogin,            // 登陆界面
        UIHeroSelect,       // 角色选择界面界面
        UIBlurBg,           // 模糊背景界面
        UITest,             // 测试界面
	}

    public enum UILayerID
    {
        BtmLayer,                   // 最低层啊，一般不放东西，以备不时之需，目前放模糊的界面
        FirstLayer,                 // 第一层，聊天之类的主界面窗口
        SecondLayer,                // 第二层，主要是功能性的界面，弹出需要关闭的界面
        ThirdLayer,                 // 第三层，提示窗口层
        ForthLayer,                 // 第四层，新手引导层
        TopLayer,                   // 最顶层，一般不放东西，以备不时之需

		MaxLayer
    }

    public enum WinIDCnt
    {
        eIDCnt = 500,           // 每一个窗口中可以占用的 ID 数量
    }
}