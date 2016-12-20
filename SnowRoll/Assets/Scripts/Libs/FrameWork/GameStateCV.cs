namespace SDK.Lib
{
    /**
     * @brief 游戏状态常量
     */
    public enum GameStateCV
    {
        eGSStart = 0,                   // 启动状态
        eGSFirstLoadingScene = 1,       // 第一次加载场景
        eGSLoadingScene = 2,            // 非第一次加载场景
        eGSRun = 3,                     // 运行状态
        eGSQuit = 4,                    // 退出状态
    }
}