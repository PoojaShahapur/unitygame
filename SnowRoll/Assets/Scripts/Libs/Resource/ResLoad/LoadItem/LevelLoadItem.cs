using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 支持从本地和 Web 服务器加载场景和场景 Bundle 资源 ，采用 WWW 下载
     */
    public class LevelLoadItem : LoadItem
    {
        protected string mLevelName;

        public string levelName
        {
            get
            {
                return mLevelName;
            }
            set
            {
                mLevelName = value;
            }
        }

        override public void load()
        {
            base.load();

            if(ResLoadType.eLoadResource == mResLoadType)
            {
                mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
            }
            else if (ResLoadType.eLoadStreamingAssets == mResLoadType ||
                ResLoadType.eLoadLocalPersistentData == mResLoadType)
            {
                // 需要加载 AssetBundles 加载
                if (mLoadNeedCoroutine)
                {
                    Ctx.mInstance.mCoroutineMgr.StartCoroutine(loadFromAssetBundleByCoroutine());
                }
                else
                {
                    loadFromAssetBundle();
                }
            }
            else if (ResLoadType.eLoadWeb == mResLoadType)
            {
                Ctx.mInstance.mCoroutineMgr.StartCoroutine(downloadAsset());
            }
        }

        override public void unload()
        {
            base.unload();
        }

        protected IEnumerator loadFromAssetBundleByCoroutine()
        {
            string path = "";
            // UNITY_5_2 没有
            //AssetBundleCreateRequest req = null;

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            //byte[] bytes = Ctx.mInstance.m_fileSys.LoadFileByte(path);
            //req = AssetBundle.CreateFromMemory(bytes);
            //yield return req;

            // UNITY_5_2 没有异步从文件加载的 LoadFromFileAsync 接口，只有从内存异步加载的 CreateFromMemory 接口，因此直接使用 WWW 读取，就不先从文件系统将二进制读取进来，然后再调用 CreateFromMemory 了，不知道 WWW 和 从文件系统读取二进制再 CreateFromMemory 哪个更快。
            WWW www = null;
            path = ResPathResolve.msFileLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;
            www = new WWW(path);
            yield return www;
            mAssetBundle = www.assetBundle;

            www.Dispose();
            www = null;
#else
            AssetBundleCreateRequest req = null;

            path = ResPathResolve.msABLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;
            
            req = AssetBundle.LoadFromFileAsync(path);
            yield return req;

            mAssetBundle = req.assetBundle;
#endif

            assetAssetBundlesLevelLoaded();
        }

        protected void loadFromAssetBundle()
        {
            string path = "";
            path = ResPathResolve.msABLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;
            
            // UNITY_5_2 没有
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            mAssetBundle = AssetBundle.CreateFromFile(path);
#else
            mAssetBundle = AssetBundle.LoadFromFile(path);
#endif

            assetAssetBundlesLevelLoaded();
        }

        protected void assetAssetBundlesLevelLoaded()
        {
            if (mAssetBundle != null)
            {
                mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                mNonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}