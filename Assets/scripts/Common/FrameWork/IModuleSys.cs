namespace SDK.Common
{
    /**
     * @brief 模块系统
     */
    public interface IModuleSys
    {
        void loadModule(string name);
        void unloadModule(string name);
    }
}
