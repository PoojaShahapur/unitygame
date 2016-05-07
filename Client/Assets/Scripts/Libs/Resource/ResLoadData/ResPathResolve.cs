using UnityEngine;

namespace SDK.Lib
{
    public class ResPathResolve
    {
        static public string kAssetBundlesPath = "/AssetBundles/";
        static public string BaseDownloadingURL;
        static public string AssetBundlesPrefixPath = "Assets/Resources/";

        /*
        public static void modifyLoadParam(string resPath, LoadParam param)
        {
            param.m_origPath = resPath;             // 记录原始的资源名字

            if (MacroDef.PKG_RES_LOAD)
            {
                string retPath = resPath;

                if ("Module/AutoUpdate.prefab" == resPath)       // 自动更新模块更新还没有实现
                {
                    param.m_resLoadType = ResLoadType.eStreamingAssets;
                }
                else
                {
                    // 获取包的名字
                    if (Ctx.m_instance.m_pPakSys.path2PakDic.ContainsKey(resPath))
                    {
                        retPath = Ctx.m_instance.m_pPakSys.path2PakDic[resPath].m_pakName;
                    }

                    if (param != null)
                    {
                        Ctx.m_instance.m_fileSys.getAbsPathByRelPath(ref retPath, ref param.m_resLoadType);
                    }
                    else
                    {
                        ResLoadType tmp = ResLoadType.eStreamingAssets;
                        Ctx.m_instance.m_fileSys.getAbsPathByRelPath(ref retPath, ref tmp);
                    }
                }
                param.m_path = retPath;
                param.m_pakPath = param.m_path;
            }
            else if (MacroDef.UNPKG_RES_LOAD)
            {
                if (param != null)
                {
                    param.m_resLoadType = ResLoadType.eStreamingAssets;
                }
                param.m_path = resPath;
            }
            else if (MacroDef.ASSETBUNDLES_LOAD)
            {
                //param.m_path = AssetBundlesPrefixPath + resPath;
                //param.m_path = param.m_path.ToLower();
                param.m_path = resPath;
            }
            else
            {
                param.m_path = resPath;
            }
        }
        */

        static public void initABRootPath()
        {
            string relativePath = "";
            if (MacroDef.ASSETBUNDLES_LOAD)
            {
                relativePath = Application.streamingAssetsPath;
            }
            else
            {
                relativePath = Application.streamingAssetsPath;
            }
            string platformFolderForAssetBundles = UtilApi.GetPlatformFolderForAssetBundles(Application.platform);
            BaseDownloadingURL = relativePath + kAssetBundlesPath + platformFolderForAssetBundles;
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


    }
}