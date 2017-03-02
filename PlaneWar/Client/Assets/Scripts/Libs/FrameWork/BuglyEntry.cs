namespace SDK.Lib
{
    /**
     * @brief Bugly 接入
     */
    public class BuglyEntry
    {
#if UNITY_IPHONE || UNITY_IOS
        public static string msAppId = "d319aa5377";
#elif UNITY_ANDROID
        public static string msAppId = "3d591c63d5";
#else
        public static string msAppId = "";
#endif

        static public void testException()
        {
            throw new System.Exception("Test Exception");
        }

        // 初始化 Bugly
        static public void init()
        {
            // 开启SDK的日志打印，发布版本请务必关闭
            BuglyAgent.ConfigDebugMode(true);
            // 注册日志回调，替换使用 'Application.RegisterLogCallback(Application.LogCallback)'注册日志回调的方式
            // BuglyAgent.RegisterLogCallback (CallbackDelegate.Instance.OnApplicationLogCallbackHandler);

#if UNITY_IPHONE || UNITY_IOS
            BuglyAgent.InitWithAppId (BuglyEntry.msAppId);
#elif UNITY_ANDROID
            BuglyAgent.InitWithAppId(BuglyEntry.msAppId);
#endif

            // 如果你确认已在对应的iOS工程或Android工程中初始化SDK，那么在脚本中只需启动C#异常捕获上报功能即可
            BuglyAgent.EnableExceptionHandler();
        }
    }
}