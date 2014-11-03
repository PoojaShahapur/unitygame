namespace SDK.Common
{
    /**
     * @brief 不释放的路径
     */
    public class NotDestroyPath
    {
        public const string ND_CV_App = "App(Clone)";       // 注意这个地方不是 "App" ，实例化的一定要加 (Clone)
        public const string ND_CV_RootLayer = "RootLayer";   // 注意不是 "App(Clone)/RootLayer"
        public const string ND_CV_SceneLayer = "RootLayer/SceneLayer";
        public const string ND_CV_GameLayer = "RootLayer/SceneLayer/GameLayer";

        public const string ND_CV_UILayer = "RootLayer/UILayer";
        public const string ND_CV_UIRoot = "RootLayer/UILayer/UIRoot";
        public const string ND_CV_Camera = "RootLayer/UILayer/UIRoot/Camera";
        public const string ND_CV_UIFirstLayer = "RootLayer/UILayer/UIRoot/UIFirstLayer";
        public const string ND_CV_Game = "RootLayer/SceneLayer/GameLayer/Game";
    }
}
