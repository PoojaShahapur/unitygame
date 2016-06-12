using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 支持从本地和 Web 服务器加载场景和场景 Bundle 资源 ，采用 WWW 下载
     */
    public class LevelLoadItem : LoadItem
    {
        protected string m_levelName;

        public string levelName
        {
            get
            {
                return m_levelName;
            }
            set
            {
                m_levelName = value;
            }
        }

        override public void load()
        {
            base.load();
            if(ResLoadType.eLoadResource == m_resLoadType)
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
            }
            else if (ResLoadType.eLoadStreamingAssets == m_resLoadType ||
                ResLoadType.eLoadLocalPersistentData == m_resLoadType)
            {
                // 需要加载 AssetBundles 加载
                if (m_loadNeedCoroutine)
                {
                    Ctx.m_instance.m_coroutineMgr.StartCoroutine(loadFromAssetBundleByCoroutine());
                }
                else
                {
                    loadFromAssetBundle();
                }
            }
            else if (ResLoadType.eLoadWeb == m_resLoadType)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(downloadAsset());
            }
        }

        override public void unload()
        {
            base.unload();
        }

        protected IEnumerator loadFromAssetBundleByCoroutine()
        {
            string path = "";
            path = ResPathResolve.msLoadRootPathList[(int)m_resLoadType] + "/" + m_loadPath;
            // UNITY_5_2 没有
            AssetBundleCreateRequest req = null;

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            byte[] bytes = Ctx.m_instance.m_fileSys.LoadFileByte(path);
            req = AssetBundle.CreateFromMemory(bytes);
            yield return req;
#else
            req = AssetBundle.LoadFromFileAsync(path);
            yield return req;
#endif

            m_assetBundle = req.assetBundle;
            assetAssetBundlesLevelLoaded();
        }

        protected void loadFromAssetBundle()
        {
            string path;
            path = ResPathResolve.msLoadRootPathList[(int)m_resLoadType] + "/" + m_loadPath;
            // UNITY_5_2 没有
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            m_assetBundle = AssetBundle.CreateFromFile(path);
#else
            m_assetBundle = AssetBundle.LoadFromFile(path);
#endif
            assetAssetBundlesLevelLoaded();
        }

        protected void assetAssetBundlesLevelLoaded()
        {
            if (m_assetBundle != null)
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}