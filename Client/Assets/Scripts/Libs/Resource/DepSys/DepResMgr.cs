﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR	
using UnityEditor;
#endif

namespace SDK.Lib
{
    /**
     * @brief AssetBundles 资源依赖系统，主要是加载依赖系统使用的
     */
    public class DepResMgr
    {
        protected AssetBundleManifest m_AssetBundleManifest;
        protected string[] m_Variants = { };
        protected Dictionary<string, string[]> m_Dependencies;
        protected Dictionary<string, ResAndDepItem> m_resAndDepItemDic;

        public DepResMgr()
        {
            m_resAndDepItemDic = new Dictionary<string, ResAndDepItem>();
            m_AssetBundleManifest = null;
            m_Dependencies = new Dictionary<string, string[]>();
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

        public void initialize()
        {
            MFileSys.initABRootPath();

            string platformFolderForAssetBundles =
			UtilApi.GetPlatformFolderForAssetBundles(Application.platform);
            // 必须同步加载
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = platformFolderForAssetBundles;
            param.m_loadEventHandle = onLoadEventHandle;
            param.m_loadNeedCoroutine = true;
            param.m_resNeedCoroutine = true;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        protected string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

            if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
                return assetBundleName;

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;

            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                    continue;

                int found = System.Array.IndexOf(m_Variants, curSplit[1]);
                if (found != -1 && found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFitIndex != -1)
                return bundlesWithVariant[bestFitIndex];
            else
                return assetBundleName;
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;

            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                // 从 AssetBundle 中获取名字 AssetBundleManifest
                m_AssetBundleManifest = res.getObject("AssetBundleManifest") as AssetBundleManifest;
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                
            }

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), onLoadEventHandle);
        }

        public bool hasDep(string assetBundleName)
        {
            if (m_AssetBundleManifest == null)
            {
                Debug.LogError("Please initialize AssetBundleManifest");
                return false;
            }

            if (!m_Dependencies.ContainsKey(assetBundleName))
            {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
                if (dependencies.Length == 0)
                    return false;

                for (int i = 0; i < dependencies.Length; i++)
                    dependencies[i] = RemapVariantName(dependencies[i]);

                m_Dependencies.Add(assetBundleName, dependencies);
            }

            return true;
        }

        public bool checkIfAllDepLoaded(string[] depList)
        {
            foreach(string depName in depList)
            {
                ResItem res = Ctx.m_instance.m_resLoadMgr.getResource(depName);
                if(res == null)
                {
                    return false;
                }
                else if(res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded() ||
                    res.refCountResLoadResultNotify.resLoadState.hasFailed())
                {
                    return false;
                }
            }

            return true;    // 所有的依赖都加载完成
        }

        public bool isDepResLoaded(string assetBundleName)
        {
            if (m_AssetBundleManifest == null)
            {
                Debug.LogError("Please initialize AssetBundleManifest");
                return true;
            }

            if(!m_Dependencies.ContainsKey(assetBundleName))
            {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
                if (dependencies.Length == 0)
                    return true;

                for (int i = 0; i < dependencies.Length; i++)
                    dependencies[i] = RemapVariantName(dependencies[i]);

                m_Dependencies.Add(assetBundleName, dependencies);
            }

            return checkIfAllDepLoaded(m_Dependencies[assetBundleName]);
        }

        public void loadRes(LoadParam loadParam)
        {
            m_resAndDepItemDic[loadParam.m_path] = new ResAndDepItem();
            m_resAndDepItemDic[loadParam.m_path].m_loadParam = loadParam;
            m_resAndDepItemDic[loadParam.m_path].m_depNameArr = m_Dependencies[loadParam.m_path];
            m_resAndDepItemDic[loadParam.m_path].loadDep();
        }

        public void unLoadDep(string assetBundleName)
        {
            if(m_resAndDepItemDic.ContainsKey(assetBundleName))
            {
                m_resAndDepItemDic[assetBundleName].unloadDep();
                m_resAndDepItemDic.Remove(assetBundleName);
            }
            else
            {
                Debug.Log("Error");
            }
        }
    }
}