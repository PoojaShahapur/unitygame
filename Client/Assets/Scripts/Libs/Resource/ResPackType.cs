namespace SDK.Lib
{
    /**
     * @brief 资源包的类型
     */
    public enum ResPackType
    {
        eLevelType,     // 关卡，可以从本地或者服务器加载
        eBundleType,    // 资源包，注意默认的打包 Resources 不包括在这里面，自己真正的单独打包，才在这里面，可以从本地或者服务器加载
        eResourcesType, // 注意默认的打包 Resources ，只能从本地加载
        eDataType,      // 纯数据资源，主要是从 web 服务器更新数据
        eUnPakType,     // 没有打包的 unity3d 数据
        eUnPakLevelType,// 没有打包的 Level unity3d 数据

        ePakType,     // 没有打包的 unity3d 数据
        ePakLevelType,// 没有打包的 Level unity3d 数据

        eNoneType       // 默认类型
    }
}