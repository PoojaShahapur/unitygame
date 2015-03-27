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

        eNoneType       // 默认类型
    }
}