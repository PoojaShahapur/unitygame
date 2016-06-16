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
        protected string mText;
        protected string mVersion = "";

        protected string mLocalPath;            // 本地文件系统目录
        protected string mLoadPath;             // 加载目录
        protected string mOrigPath;             // 原始的加载目录
        protected string mLogicPath;            // 逻辑获取的目录
        protected string mResUniqueId;          // 资源 Unique Id
        protected string mDownloadNoVerPath;         // 下载目录，没有版本号
        protected string mDownloadVerPath;         // 下载目录，有版本号

        protected WWW m_w3File;
        protected DownloadType mDownloadType;   // 加载类型
        protected ResLoadType mResLoadType;
        protected ResPackType mResPackType;

        protected RefCountResLoadResultNotify m_refCountResLoadResultNotify;
        protected ResEventDispatch mAllLoadResEventDispatch;    // 事件分发器，这个是记录逻辑的事件分发器

        protected bool mIsWriteFile;
        protected long mFileLen;

        public DownloadItem()
        {
            mResLoadType = ResLoadType.eLoadWeb;
            m_refCountResLoadResultNotify = new RefCountResLoadResultNotify();
            mAllLoadResEventDispatch = new ResEventDispatch();
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

        public ResEventDispatch allLoadResEventDispatch
        {
            get
            {
                return mAllLoadResEventDispatch;
            }
            set
            {
                mAllLoadResEventDispatch = value;
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

        public bool hasSuccessLoaded()
        {
            return m_refCountResLoadResultNotify.resLoadState.hasSuccessLoaded();
        }

        public bool hasFailed()
        {
            return m_refCountResLoadResultNotify.resLoadState.hasFailed();
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

        public string getLoadPath()
        {
            return mLoadPath;
        }

        public byte[] getBytes()
        {
            return mBytes;
        }

        public string getText()
        {
            return mText;
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
            if (string.IsNullOrEmpty(www.error) && www.isDone)
            {
                return true;
            }

            return false;
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
            this.mResLoadType = param.mResLoadType;
            this.mResPackType = param.mResPackType;
            this.mVersion = param.mVersion;
            this.mIsWriteFile = param.mIsWriteFile;
            this.mFileLen = param.mFileLen;
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

            mDownloadNoVerPath = ResPathResolve.msFileLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;
            mDownloadVerPath = ResPathResolve.msFileLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath + "?ver=" + mVersion;

            Ctx.m_instance.m_logSys.log(string.Format("添加下载任务 {0}", mLoadPath));
        }

        public virtual void runTask()
        {

        }

        public virtual void handleResult()
        {

        }

        // 加载完成写入本地文件系统
        public virtual void writeFile()
        {
            if (mBytes != null)
            {
                if (UtilPath.existFile(mLocalPath))
                {
                    UtilPath.deleteFile(mLocalPath);
                }
                else
                {
                    string path = UtilPath.getFilePathNoName(mLocalPath);
                    if (!UtilPath.existDirectory(path))
                    {
                        UtilPath.createDirectory(path);
                    }
                }
                MDataStream dataStream = new MDataStream(mLocalPath, null, FileMode.CreateNew, FileAccess.Write);
                dataStream.writeByte(mBytes);
                dataStream.dispose();
            }
        }
    }
}