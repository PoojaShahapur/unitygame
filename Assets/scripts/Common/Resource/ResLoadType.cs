namespace SDK.Common
{
    /**
     * @brief 资源加载类型
     */
    public enum ResLoadType
    {
        eLoadResource,  // Resource 缺省打进程序包里的AssetBundle里加载资源
        eLoadDisc,      // 从本地磁盘加载 AssetBundle
        eLoadWeb,       // 从 Web 加载

        eLoadNum
    }
}
