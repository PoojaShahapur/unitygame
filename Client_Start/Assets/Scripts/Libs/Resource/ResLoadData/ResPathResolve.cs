namespace SDK.Lib
{
    public class ResPathResolve
    {
        static public string msAssetBundlesPrefixPath = "Assets/Resources/";
        static public string[] msLoadRootPathList;

        static public void initRootPath()
        {
            msLoadRootPathList = new string[(int)ResLoadType.eLoadTotal];

            msLoadRootPathList[(int)ResLoadType.eLoadResource] = "";
            msLoadRootPathList[(int)ResLoadType.eLoadStreamingAssets] = MFileSys.msStreamingAssetsPath;
            msLoadRootPathList[(int)ResLoadType.eLoadLocalPersistentData] = MFileSys.msPersistentDataPath;
            msLoadRootPathList[(int)ResLoadType.eLoadWeb] = "http://127.0.0.1/Monster";
        }
    }
}