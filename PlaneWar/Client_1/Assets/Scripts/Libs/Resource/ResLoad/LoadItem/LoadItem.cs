using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class LoadItem : IDispatchObject, IDelayHandleItem, ITickedObject, ILoadProgress, INoOrPriorityObject
    {
        protected ResPackType mResPackType;    // 资源打包类型
        protected ResLoadType mResLoadType;    // 资源加载类型

        protected string mLoadPath;            // 完整的目录
        protected string mOrigPath;            // 原始的资源目录
        protected string mExtName;             // 扩展名字
        protected string mLogicPath;

        protected WWW mW3File;
        protected bool mLoadNeedCoroutine;     // 加载是否需要协同程序

        protected AssetBundle mAssetBundle;

        protected NonRefCountResLoadResultNotify mNonRefCountResLoadResultNotify;
        protected bool mIsLoadAll;              // 是否加载所有的内容
        protected string mResUniqueId;

        protected float mLoadProgress;          // 加载进度
        protected AddOnceEventDispatch mProgressEventDispatch;
        protected bool mIsOverloading;          // 是否是超负荷加载

        public LoadItem()
        {
            this.mIsLoadAll = false;
            this.mNonRefCountResLoadResultNotify = new NonRefCountResLoadResultNotify();
            this.mProgressEventDispatch = null;
            this.mLoadProgress = 0;
        }

        public ResPackType resPackType
        {
            get
            {
                return this.mResPackType;
            }
            set
            {
                this.mResPackType = value;
            }
        }

        public string loadPath
        {
            get
            {
                return this.mLoadPath;
            }
            set
            {
                this.mLoadPath = value;
            }
        }

        public string origPath
        {
            get
            {
                return this.mOrigPath;
            }
            set
            {
                this.mOrigPath = value;
            }
        }

        public string extName
        {
            get
            {
                return this.mExtName;
            }
            set
            {
                this.mExtName = value;
            }
        }

        public WWW w3File
        {
            get
            {
                return this.mW3File;
            }
        }

        public bool loadNeedCoroutine
        {
            get
            {
                return this.mLoadNeedCoroutine;
            }
            set
            {
                this.mLoadNeedCoroutine = value;
            }
        }

        public ResLoadType resLoadType
        {
            get
            {
                return this.mResLoadType;
            }
            set
            {
                this.mResLoadType = value;
            }
        }

        public AssetBundle assetBundle
        {
            get
            {
                return this.mAssetBundle;
            }
            set
            {
                this.mAssetBundle = value;
            }
        }

        public void setLogicPath(string value)
        {
            this.mLogicPath = value;
        }

        public string getLogicPath()
        {
            return this.mLogicPath;
        }

        public NonRefCountResLoadResultNotify nonRefCountResLoadResultNotify
        {
            get
            {
                return this.mNonRefCountResLoadResultNotify;
            }
            set
            {
                this.mNonRefCountResLoadResultNotify = value;
            }
        }

        public bool isOverloading()
        {
            return this.mIsOverloading;
        }

        public void setIsOverloading(bool value)
        {
            this.mIsOverloading = value;
        }

        public void setLoadAll(bool value)
        {
            this.mIsLoadAll = value;
        }

        public bool getLoadAll()
        {
            return this.mIsLoadAll;
        }

        public void setResUniqueId(string value)
        {
            this.mResUniqueId = value;
        }

        public string getResUniqueId()
        {
            return this.mResUniqueId;
        }

        protected void clearProgressHandle(bool isNeedDispatch = true)
        {
            if (null != this.mProgressEventDispatch)
            {
                if (isNeedDispatch)
                {
                    this.mProgressEventDispatch.dispatchEvent(this);
                }
                this.mProgressEventDispatch.clearEventHandle();
                Ctx.mInstance.mLoadProgressMgr.removeProgress(this);
            }
        }

        virtual public void load()
        {
            if (null != this.mProgressEventDispatch && this.mProgressEventDispatch.hasEventHandle())
            {
                Ctx.mInstance.mLoadProgressMgr.addProgress(this);
                this.mProgressEventDispatch.dispatchEvent(this);
            }

            this.mNonRefCountResLoadResultNotify.resLoadState.setLoading();
        }

        // 这个是卸载，因为有时候资源加载进来可能已经不用了，需要直接卸载掉
        virtual public void unload()
        {
            this.clearProgressHandle();
            this.mLoadProgress = 0.0f;
        }

        virtual public void reset()
        {
            this.mLoadPath = "";
            this.mW3File = null;
            this.mLoadNeedCoroutine = false;
            this.mLoadProgress = 0;
            this.mIsOverloading = false;
        }

        // 成功加载
        public void onLoaded()
        {
            this.mLoadProgress = 1.0f;
            this.clearProgressHandle();
        }

        // 成功失败
        public void onFailed()
        {
            this.mLoadProgress = 0.0f;
            this.clearProgressHandle();
        }

        virtual protected IEnumerator downloadAsset()
        {
            string path = "";

            if (this.mResLoadType == ResLoadType.eLoadStreamingAssets)
            {
                path = "file://" + Application.dataPath + "/" + mLoadPath;
            }
            else if (this.mResLoadType == ResLoadType.eLoadWeb)
            {
                path = Ctx.mInstance.mCfg.mWebIP + this.mLoadPath;
            }

            this.deleteFromCache(path);
            this.mW3File = WWW.LoadFromCacheOrDownload(path, 1);
            // WWW::LoadFromCacheOrDownload 是加载 AssetBundles 使用的
            //mW3File = WWW.LoadFromCacheOrDownload(path, UnityEngine.Random.Range(0, int.MaxValue));
            yield return this.mW3File;

            this.onWWWEnd();
        }

        // 检查加载成功
        protected bool isLoadedSuccess(WWW www)
        {
            if (!UtilApi.isWWWNoError(www))
            {
                return false;
            }

            return true;
        }

        // 加载完成回调处理
        virtual protected void onWWWEnd()
        {
            if (this.isLoadedSuccess(this.mW3File))
            {
                this.mAssetBundle = this.mW3File.assetBundle;

                this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                this.mNonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            this.mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        protected void deleteFromCache(string path)
        {
            if(Caching.IsVersionCached(path, 1))
            {
                //Caching.DeleteFromCache(path);
                Caching.CleanCache();
            }
        }

        public void setLoadParam(LoadParam param)
        {
            this.resPackType = param.mResPackType;
            this.resLoadType = param.mResLoadType;
            this.loadPath = param.mLoadPath;
            this.origPath = param.mOrigPath;
            this.extName = param.extName;
            this.loadNeedCoroutine = param.mLoadNeedCoroutine;
            this.setLoadAll(param.mIsLoadAll);
            this.setLogicPath(param.mLogicPath);
            this.setResUniqueId(param.mResUniqueId);

            // 设置加载参数
            if (null != param.mProgressEventHandle)
            {
                if (null == this.mProgressEventDispatch)
                {
                    this.mProgressEventDispatch = new AddOnceEventDispatch();
                }

                this.mProgressEventDispatch.addEventHandle(null, param.mProgressEventHandle);
            }
        }

        public void onTick(float delta, TickMode tickMode)
        {
            this.updateProgress();
        }

        // 更新加载进度
        virtual protected void updateProgress()
        {
            if (null != this.mProgressEventDispatch)
            {
                this.mProgressEventDispatch.dispatchEvent(this);
            }

            if(this.mLoadProgress == 1.0f)
            {
                this.clearProgressHandle(false);
            }
        }

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        public float getProgress()
        {
            return this.mLoadProgress;
        }
    }
}