using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
    * @brief 从网络下载数据
    */
    public class DownloadItem : ITask, IDispatchObject
    {
        protected byte[] mBytes;
        protected string mVersion = "";
        protected bool mIsRunSuccess = true;
        protected string mLocalPath;

        protected string mLoadPath;            // 完整的目录
        protected string mOrigPath;            // 原始的资源目录
        protected string mLogicPath;

        protected WWW m_w3File;
        protected DownloadType mDownloadType;     // 加载类型
        protected string mResUniqueId;


        protected RefCountResLoadResultNotify m_refCountResLoadResultNotify;

        public DownloadItem()
        {
            m_refCountResLoadResultNotify = new RefCountResLoadResultNotify();
        }

        public RefCountResLoadResultNotify refCountResLoadResultNotify
        {
            get
            {
                return m_refCountResLoadResultNotify;
            }
            set
            {
                m_refCountResLoadResultNotify = value;
            }
        }

        virtual public void reset()
        {
            mLoadPath = "";
            m_w3File = null;
            mDownloadType = DownloadType.eHttpWeb;

            mBytes = null;
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

        public WWW w3File
        {
            get
            {
                return m_w3File;
            }
        }

        public DownloadType downloadType
        {
            get
            {
                return mDownloadType;
            }
            set
            {
                mDownloadType = value;
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

        public void setResUniqueId(string value)
        {
            mResUniqueId = value;
        }

        public string getResUniqueId()
        {
            return mResUniqueId;
        }

        virtual public void init()
        {

        }

        virtual public void failed()
        {

        }

        // 这个是卸载，因为有时候资源加载进来可能已经不用了，需要直接卸载掉
        virtual public void unload()
        {

        }

        virtual protected IEnumerator downloadAsset()
        {
            yield return null;   
        }

        // 检查加载成功
        protected bool isLoadedSuccess(WWW www)
        {
            if (www.error != null)
            {
                return false;
            }

            return true;
        }

        // 加载完成回调处理
        virtual protected void onWWWEnd()
        {
            
        }

        protected void deleteFromCache(string path)
        {
            if (Caching.IsVersionCached(path, 1))
            {
                Caching.CleanCache();
            }
        }

        public void setLoadParam(DownloadParam param)
        {
            this.loadPath = param.mLoadPath;
            this.origPath = param.mOrigPath;
            this.mDownloadType = param.mDownloadType;
            this.setLogicPath(param.mLogicPath);
            this.setResUniqueId(param.mResUniqueId);
        }

        virtual public void load()
        {
            m_refCountResLoadResultNotify.resLoadState.setLoading();

            mLocalPath = Path.Combine(MFileSys.getLocalWriteDir(), UtilLogic.getRelPath(mLoadPath));
            if (!string.IsNullOrEmpty(mVersion))
            {
                mLocalPath = UtilLogic.combineVerPath(mLocalPath, mVersion);
            }

            Ctx.m_instance.m_logSys.log(string.Format("添加下载任务 {0}", mLoadPath));
        }

        public virtual void runTask()
        {

        }

        public virtual void handleResult()
        {

        }
    }
}