namespace SDK.Lib
{
	/**
	 * 界面ID定义
	 */
	public enum UIFormId
	{
        // 第二层
        eUITest,            // 测试界面
        eUITerrainEdit,     // 地形编辑器窗口
        eUILogin,           // 登陆窗口
        eUISelectRole,      // 选择角色窗口
        eUIPack,            // 背包界面
        eUIJoyStick,        // 摇杆

        // Lua 界面 Id 定义开始
        eUIStartGame_Lua = 10001,       // Lua中的界面
    }

    public enum WinIdCnt
    {
        eIdCnt = 500,           // 每一个窗口中可以占用的 ID 数量
    }
}