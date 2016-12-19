namespace SDK.Lib
{
	/**
	 * 界面ID定义
	 */
	public enum UIFormID
	{
        // 第二层
        eUITest,            // 测试界面
        eUITerrainEdit,     // 地形编辑器窗口
        eUILogin,           // 登陆窗口
        eUISelectRole,      // 选择角色窗口

        // Lua 界面 Id 定义开始
        eUIStartGame = 10001,       // Lua中的界面
    }

    public enum WinIDCnt
    {
        eIDCnt = 500,           // 每一个窗口中可以占用的 ID 数量
    }
}