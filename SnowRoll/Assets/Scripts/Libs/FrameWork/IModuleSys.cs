namespace SDK.Lib
{
    /**
     * @brief 模块系统
     */
    public interface IModuleSys
    {
        void loadModule(ModuleId moduleID);
        void unloadModule(ModuleId moduleID);
    }
}
