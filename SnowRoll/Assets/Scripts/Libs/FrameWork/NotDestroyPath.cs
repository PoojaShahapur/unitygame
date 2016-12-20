namespace SDK.Lib
{
    /**
     * @brief 不释放的路径
     */
    public class NotDestroyPath
    {
        public const string ND_CV_Root = "NoDestroy";

        public const string ND_CV_App = "AppGo";       // 注意这个地方不是 "App" ，实例化的一定要加 (Clone)，目前将名字改成了 App 了，直接 App 就能获取，目前在 Start 模块直接修改成 App 了，因此使用 App 
        public const string ND_CV_Game = "Game";

        public const string ND_CV_UIFirstCanvas = "UIFirstCanvas";            // 这个是 UI ，需要屏幕自适应的
        public const string ND_CV_UISecondCanvas = "UISecondCanvas";          // 这个是 UI ，需要屏幕自适应的
        public const string ND_CV_UICamera = "UICamera";

        // 界面层，层越小越在最后面显示
        public const string ND_CV_UIBtmLayer_FirstCanvas = "UIFirstCanvas/UIBtmLayer";         // 界面最底层
        public const string ND_CV_UIFirstLayer_FirstCanvas = "UIFirstCanvas/UIFirstLayer";     // 界面第一层
        public const string ND_CV_UISecondLayer_FirstCanvas = "UIFirstCanvas/UISecondLayer";   // 界面第二层
        public const string ND_CV_UIThirdLayer_FirstCanvas = "UIFirstCanvas/UIThirdLayer";     // 界面第三层
        public const string ND_CV_UIForthLayer_FirstCanvas = "UIFirstCanvas/UIForthLayer";     // 界面第四层
        public const string ND_CV_UITopLayer_FirstCanvas = "UIFirstCanvas/UITopLayer";         // 界面最顶层

        public const string ND_CV_UIBtmLayer_SecondCanvas = "UISecondCanvas/UIBtmLayer";         // 界面最底层
        public const string ND_CV_UIFirstLayer_SecondCanvas = "UISecondCanvas/UIFirstLayer";     // 界面第一层
        public const string ND_CV_UISecondLayer_SecondCanvas = "UISecondCanvas/UISecondLayer";   // 界面第二层
        public const string ND_CV_UIThirdLayer_SecondCanvas = "UISecondCanvas/UIThirdLayer";     // 界面第三层
        public const string ND_CV_UIForthLayer_SecondCanvas = "UISecondCanvas/UIForthLayer";     // 界面第四层
        public const string ND_CV_UITopLayer_SecondCanvas = "UISecondCanvas/UITopLayer";         // 界面最顶层

        public const string ND_CV_EventSystem = "EventSystem";
    }
}