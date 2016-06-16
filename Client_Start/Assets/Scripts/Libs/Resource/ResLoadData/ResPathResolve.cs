namespace SDK.Lib
{
    public class ResPathResolve
    {
        // WWW 加载加载 File 系统时候的目录
        static public string[] msFileLoadRootPathList;
        // AssetBundles::CreateFromFile 直接从 StreamingAssetsPath 加载时候的目录
        static public string[] msABLoadRootPathList;

        static public void initRootPath()
        {
            // 初始化 WWW 加载目录
            msFileLoadRootPathList = new string[(int)ResLoadType.eLoadTotal];

            msFileLoadRootPathList[(int)ResLoadType.eLoadResource] = "";
            msFileLoadRootPathList[(int)ResLoadType.eLoadStreamingAssets] = MFileSys.msStreamingAssetsPath;
            msFileLoadRootPathList[(int)ResLoadType.eLoadLocalPersistentData] = MFileSys.msPersistentDataPath;

            msFileLoadRootPathList[(int)ResLoadType.eLoadWeb] = "http://127.0.0.1/GameWebServer/" + PlatformDefine.PlatformFolder;

            // 初始化 AssetBundles 加载目录
            msABLoadRootPathList = new string[(int)ResLoadType.eLoadTotal];

            msABLoadRootPathList[(int)ResLoadType.eLoadResource] = "";
            msABLoadRootPathList[(int)ResLoadType.eLoadStreamingAssets] = MFileSys.msStreamingAssetsPath;
            msABLoadRootPathList[(int)ResLoadType.eLoadLocalPersistentData] = MFileSys.msPersistentDataPath;

            msABLoadRootPathList[(int)ResLoadType.eLoadWeb] = "http://127.0.0.1/GameWebServer/" + PlatformDefine.PlatformFolder;
        }
    }
}