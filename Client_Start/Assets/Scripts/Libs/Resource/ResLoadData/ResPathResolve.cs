using UnityEngine;

namespace SDK.Lib
{
    public class ResPathResolve
    {
        static public string msAssetBundlesPrefixPath = "Assets/Resources/";
        static public string[] msLoadRootPathList;

        static public void initABRootPath()
        {
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
                ret = msAssetBundlesPrefixPath + resPath;
                ret = ret.ToLower();
            }

            return ret;
        }

        static public string convAssetBundlesPath2ResourcesPath(string assetBundlesPath)
        {
            if (MacroDef.ASSETBUNDLES_LOAD)
            {
                int idx = assetBundlesPath.IndexOf(msAssetBundlesPrefixPath);
                // 如果有前缀
                if (-1 != idx)
                {
                    return assetBundlesPath.Substring(msAssetBundlesPrefixPath.Length);
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
                origPath = msAssetBundlesPrefixPath + origPath;
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