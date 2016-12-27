using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief AssetBundles 资源依赖系统，主要是加载依赖系统使用的
     */
    public class DepResMgr
    {
        protected AssetBundleManifest m_AssetBundleManifest;
        protected string[] m_Variants = { };
        protected MDictionary<string, string[]> m_Dependencies;
        protected AuxAssetBundleManifestLoader mAuxAssetBundleManifestLoader;

        public DepResMgr()
        {
            m_AssetBundleManifest = null;
            m_Dependencies = new MDictionary<string, string[]>();
        }

        public AssetBundleManifest AssetBundleManifestObject
        {
            set
            {
                m_AssetBundleManifest = value;
            }
        }

        public string[] Variants
        {
            get
            {
                return m_Variants;
            }
            set
            {
                m_Variants = value;
            }
        }

        public void init()
        {
            string platformFolderForAssetBundles = UtilApi.getManifestName();
            // AssetBundleManifest 必须同步加载，加载完成这个以后再加载其它资源
            mAuxAssetBundleManifestLoader = new AuxAssetBundleManifestLoader();
            mAuxAssetBundleManifestLoader.syncLoad(platformFolderForAssetBundles, onLoadEventHandle);
        }

        public string[] getDep(string assetBundleName)
        {
            if (m_Dependencies.ContainsKey(assetBundleName))
            {
                return m_Dependencies[assetBundleName];
            }

            return null;
        }

        public void loadDep(string assetBundleName)
        {
            if (!m_Dependencies.ContainsKey(assetBundleName))
            {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
                if (dependencies.Length == 0)
                {
                    return;
                }

                for (int i = 0; i < dependencies.Length; i++)
                {
                    dependencies[i] = RemapVariantName(dependencies[i]);
                }

                m_Dependencies.Add(assetBundleName, dependencies);
            }
        }

        protected string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

            if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
            {
                return assetBundleName;
            }

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;

            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                {
                    continue;
                }

                int found = System.Array.IndexOf(m_Variants, curSplit[1]);
                if (found != -1 && found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFitIndex != -1)
            {
                return bundlesWithVariant[bestFitIndex];
            }
            else
            {
                return assetBundleName;
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            AuxAssetBundleManifestLoader mAuxAssetBundleManifestLoader = dispObj as AuxAssetBundleManifestLoader;

            if (mAuxAssetBundleManifestLoader.hasSuccessLoaded())
            {
                // 从 AssetBundle 中获取名字 AssetBundleManifest
                m_AssetBundleManifest = mAuxAssetBundleManifestLoader.getAssetBundleManifest();
            }
            else if (mAuxAssetBundleManifestLoader.hasFailed())
            {
                
            }
        }

        // 只检查是否有依赖资源，如果有依赖的资源，就算是有依赖的资源
        public bool hasDep(string assetBundleName)
        {
            if (m_AssetBundleManifest == null)
            {                
                return false;
            }

            loadDep(assetBundleName);
            if (!m_Dependencies.ContainsKey(assetBundleName))
            {
                return false;
            }

            return true;
        }

        // 检查是否所有的依赖都加载完成，加载失败也算是加载完成
        public bool checkIfAllDepLoaded(string[] depList)
        {
            foreach(string depName in depList)
            {
                if(!Ctx.mInstance.mResLoadMgr.isResLoaded(depName))
                {
                    return false;
                }
            }

            return true;    // 所有的依赖都已经加载完成了
        }

        // 是否所有的依赖的资源都加载完成
        public bool isDepResLoaded(string assetBundleName)
        {
            if (m_AssetBundleManifest == null)
            {
                return true;
            }

            loadDep(assetBundleName);
            if (!m_Dependencies.ContainsKey(assetBundleName))
            {
                return true;
            }

            return checkIfAllDepLoaded(m_Dependencies[assetBundleName]);
        }
    }
}