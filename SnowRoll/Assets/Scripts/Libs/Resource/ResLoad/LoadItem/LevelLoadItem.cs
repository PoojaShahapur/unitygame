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
        protected bool mIsSuccess;
        protected AssetBundleCreateRequest mAsyncRequest;
        protected AsyncOperation mAsyncOperation;

        public LevelLoadItem()
        {
            this.mIsSuccess = false;
            this.mAsyncRequest = null;
            this.mAsyncOperation = null;
        }

        public string levelName
        {
            get
            {
                return this.mLevelName;
            }
            set
            {
                this.mLevelName = value;
            }
        }

        override public void reset()
        {
            base.reset();
        }

        override public void load()
        {
            base.load();

            if(ResLoadType.eLoadResource == this.mResLoadType)
            {
                //this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                //this.mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
                if (mLoadNeedCoroutine)
                {
                    Ctx.mInstance.mCoroutineMgr.StartCoroutine(this.loadFromDefaultAssetBundleByCoroutine());
                }
                else
                {
                    this.loadFromDefaultAssetBundle();
                }
            }
            else if (ResLoadType.eLoadStreamingAssets == this.mResLoadType ||
                ResLoadType.eLoadLocalPersistentData == this.mResLoadType)
            {
                // 需要加载 AssetBundles 加载
                if (mLoadNeedCoroutine)
                {
                    Ctx.mInstance.mCoroutineMgr.StartCoroutine(this.loadFromAssetBundleByCoroutine());
                }
                else
                {
                    this.loadFromAssetBundle();
                }
            }
            else if (ResLoadType.eLoadWeb == this.mResLoadType)
            {
                Ctx.mInstance.mCoroutineMgr.StartCoroutine(this.downloadAsset());
            }
        }

        override public void unload()
        {
            this.mIsSuccess = false;
            this.mAsyncRequest = null;
            this.mAsyncOperation = null;

            base.unload();
        }

        protected IEnumerator loadFromDefaultAssetBundleByCoroutine()
        {
            //string path = Application.dataPath + "/" + mPath;
#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            this.mAsyncOperation = Application.LoadLevelAsync(mLevelName);
#else
            this.mAsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(mLevelName);
#endif

            yield return this.mAsyncOperation;

            if (null != this.mAsyncOperation && this.mAsyncOperation.isDone)
            {
                this.mIsSuccess = true;
            }
            else
            {
                this.mIsSuccess = false;
            }

            this.assetAssetBundlesLevelLoaded();
        }

        protected void loadFromDefaultAssetBundle()
        {
#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            if (Application.CanStreamedLevelBeLoaded(mLevelName))
            {
                this.mIsSuccess = true;
                Application.LoadLevel(mLevelName);
            }
            else
            {
                this.mIsSuccess = false;
            }
#else
            if (Application.CanStreamedLevelBeLoaded(mLevelName))
            {
                this.mIsSuccess = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene(mLevelName);
            }
            else
            {
                this.mIsSuccess = false;
            }
#endif

            this.assetAssetBundlesLevelLoaded();
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
            //AssetBundleCreateRequest req = null;

            //path = ResPathResolve.msABLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;

            //req = AssetBundle.LoadFromFileAsync(path);
            //yield return req;

            //this.mAssetBundle = req.assetBundle;
            
            path = ResPathResolve.msABLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;

            this.mAsyncRequest = AssetBundle.LoadFromFileAsync(path);
            yield return this.mAsyncRequest;

            this.mAssetBundle = this.mAsyncRequest.assetBundle;
#endif

            this.assetAssetBundlesLevelLoaded();
        }

        protected void loadFromAssetBundle()
        {
            string path = "";

            path = ResPathResolve.msABLoadRootPathList[(int)this.mResLoadType] + "/" + mLoadPath;

            // UNITY_5_2 没有
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            this.mAssetBundle = AssetBundle.CreateFromFile(path);
#else
            this.mAssetBundle = AssetBundle.LoadFromFile(path);
#endif

            this.assetAssetBundlesLevelLoaded();
        }

        protected void assetAssetBundlesLevelLoaded()
        {
            if (ResLoadType.eLoadResource == this.mResLoadType)
            {
                if (this.mIsSuccess)
                {
                    this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    this.mNonRefCountResLoadResultNotify.resLoadState.setFailed();
                }
            }
            else
            {
                if (this.mAssetBundle != null)
                {
                    this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    this.mNonRefCountResLoadResultNotify.resLoadState.setFailed();
                }
            }

            this.mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public bool hasSuccessLoaded()
        {
            return this.mNonRefCountResLoadResultNotify.resLoadState.hasSuccessLoaded();
        }

        override protected void updateProgress()
        {
            if (ResLoadType.eLoadResource == this.mResLoadType)
            {
                if (null != this.mAsyncOperation)
                {
                    this.mLoadProgress = this.mAsyncOperation.progress;
                }
            }
            else
            {
                if (null != this.mAsyncRequest)
                {
                    this.mLoadProgress = this.mAsyncRequest.progress;
                }
            }

            base.updateProgress();
        }
    }
}