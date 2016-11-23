using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class LoadItem : IDispatchObject
    {
        protected ResPackType mResPackType;    // 资源打包类型
        protected ResLoadType mResLoadType;    // 资源加载类型

        protected string mLoadPath;            // 完整的目录
        protected string mOrigPath;            // 原始的资源目录
        protected string mExtName;             // 扩展名字
        protected string mLogicPath;

        protected WWW m_w3File;
        protected bool mLoadNeedCoroutine;     // 加载是否需要协同程序

        protected AssetBundle m_assetBundle;

        protected NonRefCountResLoadResultNotify m_nonRefCountResLoadResultNotify;
        protected bool mIsLoadAll;               // 是否加载所有的内容
        protected string mResUniqueId;

        public LoadItem()
        {
            mIsLoadAll = false;
            m_nonRefCountResLoadResultNotify = new NonRefCountResLoadResultNotify();
        }

        public ResPackType resPackType
        {
            get
            {
                return mResPackType;
            }
            set
            {
                mResPackType = value;
            }
        }

        public string loadPath
        {
            get
            {
                return mLoadPath;
            }
            set
            {
                mLoadPath = value;
            }
        }

        public string origPath
        {
            get
            {
                return mOrigPath;
            }
            set
            {
                mOrigPath = value;
            }
        }

        public string extName
        {
            get
            {
                return mExtName;
            }
            set
            {
                mExtName = value;
            }
        }

        public WWW w3File
        {
            get
            {
                return m_w3File;
            }
        }

        public bool loadNeedCoroutine
        {
            get
            {
                return mLoadNeedCoroutine;
            }
            set
            {
                mLoadNeedCoroutine = value;
            }
        }

        public ResLoadType resLoadType
        {
            get
            {
                return mResLoadType;
            }
            set
            {
                mResLoadType = value;
            }
        }

        public AssetBundle assetBundle
        {
            get
            {
                return m_assetBundle;
            }
            set
            {
                m_assetBundle = value;
            }
        }

        public void setLogicPath(string value)
        {
            mLogicPath = value;
        }

        public string getLogicPath()
        {
            return mLogicPath;
        }

        public NonRefCountResLoadResultNotify nonRefCountResLoadResultNotify
        {
            get
            {
                return m_nonRefCountResLoadResultNotify;
            }
            set
            {
                m_nonRefCountResLoadResultNotify = value;
            }
        }

        public void setLoadAll(bool value)
        {
            mIsLoadAll = value;
        }

        public bool getLoadAll()
        {
            return mIsLoadAll;
        }

        public void setResUniqueId(string value)
        {
            mResUniqueId = value;
        }

        public string getResUniqueId()
        {
            return mResUniqueId;
        }

        virtual public void load()
        {
            m_nonRefCountResLoadResultNotify.resLoadState.setLoading();
        }

        // 这个是卸载，因为有时候资源加载进来可能已经不用了，需要直接卸载掉
        virtual public void unload()
        {

        }

        virtual public void reset()
        {
            mLoadPath = "";
            m_w3File = null;
            mLoadNeedCoroutine = false;
        }

        virtual protected IEnumerator downloadAsset()
        {
            string path = "";
            if (mResLoadType == ResLoadType.eLoadStreamingAssets)
            {
                path = "file://" + Application.dataPath + "/" + mLoadPath;
            }
            else if (mResLoadType == ResLoadType.eLoadWeb)
            {
                path = Ctx.mInstance.mCfg.mWebIP + mLoadPath;
            }
            deleteFromCache(path);
            m_w3File = WWW.LoadFromCacheOrDownload(path, 1);
            // WWW::LoadFromCacheOrDownload 是加载 AssetBundles 使用的
            //m_w3File = WWW.LoadFromCacheOrDownload(path, UnityEngine.Random.Range(0, int.MaxValue));
            yield return m_w3File;

            onWWWEnd();
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
            if (isLoadedSuccess(m_w3File))
            {
                m_assetBundle = m_w3File.assetBundle;

                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
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
        }
    }
}