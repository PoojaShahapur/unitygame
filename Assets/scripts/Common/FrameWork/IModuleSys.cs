namespace SDK.Common
{
    /**
     * @brief 模块系统
     */
    public interface IModuleSys
    {
        void loadModule(ModuleID moduleID);
        void unloadModule(ModuleID moduleID);
    }
}
