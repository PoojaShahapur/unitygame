using UnityEngine;

namespace SDK.Lib
{
    public class ResPathResolve
    {
        static public string kAssetBundlesPath = "/AssetBundles/";
        static public string BaseDownloadingURL;
        static public string AssetBundlesPrefixPath = "Assets/Resources/";
        static public string[] msLoadRootPathList;

        static public void initABRootPath()
        {
            string relativePath = "";
            //relativePath = Application.persistentDataPath;
            relativePath = Application.streamingAssetsPath;
            //string platformFolderForAssetBundles = UtilApi.GetPlatformFolderForAssetBundles(Application.platform);
            //BaseDownloadingURL = relativePath + kAssetBundlesPath + platformFolderForAssetBundles;
            BaseDownloadingURL = relativePath;

            msLoadRootPathList[(int)ResLoadType.eLoadResource] = "";
            msLoadRootPathList[(int)ResLoadType.eLoadStreamingAssets] = MFileSys.msStreamingAssetsPath;
            msLoadRootPathList[(int)ResLoadType.eLoadLocalPersistentData] = MFileSys.msPersistentDataPath;
            msLoadRootPathList[(int)ResLoadType.eLoadWeb] = "http://127.0.0.1/Monster";
        }

        // 转换 Resources 中的目录到 AssetBundles 中的目录
        static public string convResourcesPath2AssetBundlesPath(string resPath)
        {
            string ret = resPath;
            if (MacroDef.ASSETBUNDLES_LOAD)
            {
                ret = AssetBundlesPrefixPath + resPath;
                ret = ret.ToLower();
            }

            return ret;
        }

        static public string convAssetBundlesPath2ResourcesPath(string assetBundlesPath)
        {
            if (MacroDef.ASSETBUNDLES_LOAD)
            {
                int idx = assetBundlesPath.IndexOf(AssetBundlesPrefixPath);
                // 如果有前缀
                if (-1 != idx)
                {
                    return assetBundlesPath.Substring(AssetBundlesPrefixPath.Length);
                }
            }

            return assetBundlesPath;
        }

        // 转换原始的资源加载目录到资源唯一Id
        static public string convOrigPathToUniqueId(string origPath)
        {
            string uniqueId = "";
            if (MacroDef.ASSETBUNDLES_LOAD)
            {
                origPath = AssetBundlesPrefixPath + origPath;
                origPath = origPath.ToLower();
            }

            int dotIdx = origPath.IndexOf(".");
            if (-1 == dotIdx)
            {
                uniqueId = origPath;
            }
            else
            {
                uniqueId = origPath.Substring(0, dotIdx);
            }

            return uniqueId;
        }

        static  public string convLoadPathToUniqueId(string loadPath)
        {
            string uniqueId = "";

            int dotIdx = loadPath.IndexOf(".");
            if (-1 == dotIdx)
            {
                uniqueId = loadPath;
            }
            else
            {
                uniqueId = loadPath.Substring(0, dotIdx);
            }

            return uniqueId;
        }
    }
}