namespace SDK.Lib
{
    public class ResPathResolve
    {
        // MDataStream 加载 File 系统时候的目录
        static public string[] msDataStreamLoadRootPathList;
        // AssetBundles::CreateFromFile 加载时候的目录
        static public string[] msABLoadRootPathList;

        static public void initRootPath()
        {
            // 初始化 WWW 加载目录
            msDataStreamLoadRootPathList = new string[(int)ResLoadType.eLoadTotal];

            msDataStreamLoadRootPathList[(int)ResLoadType.eLoadResource] = MFileSys.msDataStreamResourcesPath;
            msDataStreamLoadRootPathList[(int)ResLoadType.eLoadStreamingAssets] = MFileSys.msDataStreamStreamingAssetsPath;
            msDataStreamLoadRootPathList[(int)ResLoadType.eLoadLocalPersistentData] = MFileSys.msDataStreamPersistentDataPath;

            //msDataStreamLoadRootPathList[(int)ResLoadType.eLoadWeb] = "http://127.0.0.1/GameWebServer/" + PlatformDefine.PlatformFolder;
            msDataStreamLoadRootPathList[(int)ResLoadType.eLoadWeb] = "https://gif5.club/";

            // 初始化 AssetBundles 加载目录
            msABLoadRootPathList = new string[(int)ResLoadType.eLoadTotal];

            msABLoadRootPathList[(int)ResLoadType.eLoadResource] = "";
            msABLoadRootPathList[(int)ResLoadType.eLoadStreamingAssets] = MFileSys.msAssetBundlesStreamingAssetsPath;
            msABLoadRootPathList[(int)ResLoadType.eLoadLocalPersistentData] = MFileSys.msAssetBundlesPersistentDataPath;

            msABLoadRootPathList[(int)ResLoadType.eLoadWeb] = "http://127.0.0.1/GameWebServer/" + PlatformDefine.PlatformFolder;
        }
    }
}