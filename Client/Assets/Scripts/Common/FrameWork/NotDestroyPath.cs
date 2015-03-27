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

        // 界面层，层越小越在最后面显示
        public const string ND_CV_UIBtmLayer = "Canvas/UIBtmLayer";         // 界面最底层
        public const string ND_CV_UIFirstLayer = "Canvas/UIFirstLayer";     // 界面第一层
        public const string ND_CV_UISecondLayer = "Canvas/UISecondLayer";   // 界面第二层
        public const string ND_CV_UIThirdLayer = "Canvas/UIThirdLayer";     // 界面第三层
        public const string ND_CV_UIForthLayer = "Canvas/UIForthLayer";     // 界面第四层
        public const string ND_CV_UITopLayer = "Canvas/UITopLayer";         // 界面最顶层

        public const string ND_CV_EventSystem = "EventSystem";
    }
}