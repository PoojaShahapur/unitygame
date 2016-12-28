namespace SDK.Lib
{
    /**
     * @brief Lua 调用 UIMgr 的接口，每一个给 Lua 开口的 CS ，都要有一个 Lua 接口文件，方便以后改代码的时候，知道也需要修改 Lua 中的调用接口
     */
    public interface IUIMgrLua
    {
        void showForm(UIFormId ID);
    }
}