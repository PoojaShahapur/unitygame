namespace SDK.Lib
{
    /**
     * @brief 资源加载类型，主要是从哪里加载
     */
    public enum ResLoadType
    {
        eLoadResource,              // Resources 缺省打进程序包里的AssetBundle里加载资源
        eLoadStreamingAssets,       // 从 StreamingAssets 目录加载
        eLoadLocalPersistentData,   // 从 persistentDataPath 本地目录加载
        eLoadWeb,                   // 从 Web 加载
        eLoadNum
    }
}