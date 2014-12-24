namespace SDK.Common
{
    /**
     * @brief 不释放的路径
     */
    public class NotDestroyPath
    {
        public const string ND_CV_Root = "NoDestroy";

        public const string ND_CV_App = "App";       // 注意这个地方不是 "App" ，实例化的一定要加 (Clone)，目前将名字改成了 App 了，直接 App 就能获取，目前在 Start 模块直接修改成 App 了，因此使用 App 
        public const string ND_CV_Game = "Game";

        public const string ND_CV_Canvas = "Canvas";
        public const string ND_CV_UICamera = "Canvas/UICamera";
        public const string ND_CV_UIFirstLayer = "Canvas/UIFirstLayer";
        public const string ND_CV_EventSystem = "EventSystem";
    }
}