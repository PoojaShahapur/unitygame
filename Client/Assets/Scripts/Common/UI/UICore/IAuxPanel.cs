namespace SDK.Common
{
    /**
     * @brief Panel 基类
     */
    public interface IAuxPanel
    {
        void findWidget();
        void addEventHandle();
        // 这个函数在构造函数、findWidget、addEventHandle 之后运行
        void init();
    }
}