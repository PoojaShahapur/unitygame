using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 数据下载管理器
     */
    public class DownloadMgr : MsgRouteHandleBase
    {
        protected uint mMaxParral;                             // 最多同时加载的内容
        protected uint mCurNum;                                // 当前加载的数量
        protected DownloadData mLoadData;
        protected DownloadItem mRetLoadItem;
        protected ResMsgRouteCB mResMsgRouteCB;
        protected List<string> mZeroRefResIDList;      // 没有引用的资源 ID 列表
        protected int mLoadingDepth;                   // 加载深度

        public DownloadMgr()
        {
            mMaxParral = 8;
            mCurNum = 0;
            mLoadData = new DownloadData();
            mZeroRefResIDList = new List<string>();
            mLoadingDepth = 0;

            this.addMsgRouteHandle(MsgRouteID.eMRIDLoadedWebRes, onMsgRouteResLoad);
        }

        public void init()
        {
            // 游戏逻辑处理
            mResMsgRouteCB = new ResMsgRouteCB();
            Ctx.mInstance.mMsgRouteNotify.addOneDisp(mResMsgRouteCB);
        }

        public void dispose()
        {

        }

        protected void resetLoadParam(DownloadParam loadParam)
        {
            loadParam.reset();
        }

        // 是否有正在加载的 DownloadItem
        public bool hasDownloadItem(string resUniqueId)
        {
            foreach (DownloadItem loadItem in mLoadData.mPath2LDItem.Values)
            {
                if (loadItem.getResUniqueId() == resUniqueId)
                {
                    return true;
                }
            }

            foreach (DownloadItem loadItem in mLoadData.mWillLDItem)
            {
                if (loadItem.getResUniqueId() == resUniqueId)
                {
                    return true;
                }
            }

            return false;
        }

        // 重置加载设置
        protected void resetDownloadParam(DownloadParam loadParam)
        {
            loadParam.reset();
        }

        // 资源是否已经加载，包括成功和失败
        public bool isDownloaded(string path)
        {
            DownloadItem downloadItem = this.getDownloadItem(path);
            if (downloadItem == null)
            {
                return false;
            }
            else if (downloadItem.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded() ||
                downloadItem.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                return true;
            }

            return false;
        }

        public bool isSuccessDownLoaded(string resUniqueId)
        {
            DownloadItem downloadItem = this.getDownloadItem(resUniqueId);
            if (downloadItem == null)
            {
                return false;
            }
            else if (downloadItem.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                return true;
            }

            return false;
        }

        public DownloadItem getDownloadItem(string resUniqueId)
        {
            foreach (DownloadItem loadItem in mLoadData.mPath2LDItem.Values)
            {
                if (loadItem.getResUniqueId() == resUniqueId)
                {
                    return loadItem;
                }
            }

            foreach (DownloadItem loadItem in mLoadData.mWillLDItem)
            {
                if (loadItem.getResUniqueId() == resUniqueId)
                {
                    return loadItem;
                }
            }

            return null;
        }

        protected DownloadItem createDownloadItem(DownloadParam param)
        {
            DownloadItem loadItem = findDownloadItemFormPool();
            if (loadItem == null)
            {
                if (param.mDownloadType == DownloadType.eWWW)
                {
                    loadItem = new WWWDownloadItem();
                }
                else if (param.mDownloadType == DownloadType.eHttpWeb)
                {
                    loadItem = new HttpWebDownloadItem();
                }
            }
            loadItem.setLoadParam(param);
            loadItem.refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, onLoadEventHandle);
            loadItem.allLoadResEventDispatch.addEventHandle(null, param.mLoadEventHandle);

            return loadItem;
        }

        protected void downloadWithDownloading(DownloadParam param)
        {
            mLoadData.mPath2LDItem[param.mResUniqueId].refCountResLoadResultNotify.refCount.incRef();
            if (mLoadData.mPath2LDItem[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.mLoadEventHandle != null)
                {
                    param.mLoadEventHandle(mLoadData.mPath2LDItem[param.mResUniqueId]);
                }
            }
            else
            {
                if (param.mLoadEventHandle != null)
                {
                    mLoadData.mPath2LDItem[param.mResUniqueId].allLoadResEventDispatch.addEventHandle(null, param.mLoadEventHandle);
                }
            }

            resetLoadParam(param);
        }

        protected void downloadWithNotDownload(DownloadParam param)
        {
            if (!hasDownloadItem(param.mResUniqueId))
            {
                DownloadItem loadItem = createDownloadItem(param);

                if (mCurNum < mMaxParral)
                {
                    // 先增加，否则退出的时候可能是先减 1 ，导致越界出现很大的值
                    ++mCurNum;
                    mLoadData.mPath2LDItem[param.mResUniqueId] = loadItem;
                    mLoadData.mPath2LDItem[param.mResUniqueId].load();
                }
                else
                {
                    mLoadData.mWillLDItem.Add(loadItem);
                }
            }

            resetLoadParam(param);
        }

        // 通用类型，需要自己设置很多参数
        public void download(DownloadParam param)
        {
            ++mLoadingDepth;

            if (mLoadData.mPath2LDItem.ContainsKey(param.mResUniqueId))
            {
                downloadWithDownloading(param);
            }
            else
            {
                downloadWithNotDownload(param);
            }

            --mLoadingDepth;

            if (mLoadingDepth == 0)
            {
                unloadNoRefResFromList();
            }
        }

        public DownloadItem getAndDownload(DownloadParam param)
        {
            //param.resolvePath();
            download(param);
            return getDownloadItem(param.mResUniqueId);
        }

        // 这个卸载有引用计数，如果有引用计数就卸载不了
        public void undownload(string resUniqueId, MAction<IDispatchObject> loadEventHandle)
        {
            if (mLoadData.mPath2LDItem.ContainsKey(resUniqueId))
            {
                // 移除事件监听器，因为很有可能移除的时候，资源还没加载完成，这个时候事件监听器中的处理函数列表还没有清理
                mLoadData.mPath2LDItem[resUniqueId].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, loadEventHandle);
                mLoadData.mPath2LDItem[resUniqueId].refCountResLoadResultNotify.refCount.decRef();
                if (mLoadData.mPath2LDItem[resUniqueId].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    if (mLoadingDepth != 0)
                    {
                        addNoRefResID2List(resUniqueId);
                    }
                    else
                    {
                        unloadNoRef(resUniqueId);
                    }
                }
            }
        }

        // 卸载所有的资源
        public void unloadAll()
        {
            MList<string> resUniqueIdList = new MList<string>();
            foreach (string resUniqueId in mLoadData.mPath2LDItem.Keys)
            {
                resUniqueIdList.Add(resUniqueId);
            }

            int idx = 0;
            int len = resUniqueIdList.length();
            while (idx < len)
            {
                this.unloadNoRef(resUniqueIdList[idx]);
                ++idx;
            }

            resUniqueIdList.Clear();
            resUniqueIdList = null;
        }

        // 添加无引用资源到 List
        protected void addNoRefResID2List(string resUniqueId)
        {
            mZeroRefResIDList.Add(resUniqueId);
        }

        // 卸载没有引用的资源列表中的资源
        protected void unloadNoRefResFromList()
        {
            foreach (string path in mZeroRefResIDList)
            {
                if (mLoadData.mPath2LDItem[path].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    unloadNoRef(path);
                }
            }
            mZeroRefResIDList.Clear();
        }

        // 不考虑引用计数，直接卸载
        protected void unloadNoRef(string resUniqueId)
        {
            if (mLoadData.mPath2LDItem.ContainsKey(resUniqueId))
            {
                mLoadData.mPath2LDItem[resUniqueId].unload();
                mLoadData.mPath2LDItem[resUniqueId].reset();
                mLoadData.mNoUsedLDItem.Add(mLoadData.mPath2LDItem[resUniqueId]);
                mLoadData.mPath2LDItem.Remove(resUniqueId);

                // 检查是否存在还没有执行的 LoadItem，如果存在就直接移除
                removeWillLoadItem(resUniqueId);
            }
            else
            {
                
            }
        }

        public void removeWillLoadItem(string resUniqueId)
        {
            foreach (DownloadItem loadItem in mLoadData.mWillLDItem)
            {
                if (loadItem.getResUniqueId() == resUniqueId)
                {
                    releaseLoadItem(loadItem);      // 必然只有一个，如果有多个就是错误
                    break;
                }
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            DownloadItem item = dispObj as DownloadItem;
            item.refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, onLoadEventHandle);
            if (item.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                onLoaded(item);
            }
            else if (item.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                onFailed(item);
            }

            item.allLoadResEventDispatch.dispatchEvent(item);

            releaseLoadItem(item);
            --mCurNum;
            loadNextItem();
        }

        public void onLoaded(DownloadItem item)
        {
            if (mLoadData.mPath2LDItem.ContainsKey(item.getResUniqueId()))
            {
                mLoadData.mPath2LDItem[item.getResUniqueId()].init();
            }
            else        // 如果资源已经没有使用的地方了
            {
                item.unload();          // 直接卸载掉
            }
        }

        public void onFailed(DownloadItem item)
        {
            string resUniqueId = item.getResUniqueId();
            if (mLoadData.mPath2LDItem.ContainsKey(resUniqueId))
            {
                mLoadData.mPath2LDItem[resUniqueId].failed();
            }
        }

        protected void releaseLoadItem(DownloadItem item)
        {
            item.reset();
            mLoadData.mNoUsedLDItem.Add(item);
            mLoadData.mWillLDItem.Remove(item);
            mLoadData.mPath2LDItem.Remove(item.getResUniqueId());
        }

        protected void loadNextItem()
        {
            if (mCurNum < mMaxParral)
            {
                if (mLoadData.mWillLDItem.Count > 0)
                {
                    string resUniqueId = (mLoadData.mWillLDItem[0] as LoadItem).getResUniqueId();
                    mLoadData.mPath2LDItem[resUniqueId] = mLoadData.mWillLDItem[0] as DownloadItem;
                    mLoadData.mWillLDItem.RemoveAt(0);
                    mLoadData.mPath2LDItem[resUniqueId].load();

                    ++mCurNum;
                }
            }
        }

        protected DownloadItem findDownloadItemFormPool()
        {
            mRetLoadItem = null;
            foreach (DownloadItem item in mLoadData.mNoUsedLDItem)
            {
                mRetLoadItem = item;
                mLoadData.mNoUsedLDItem.Remove(mRetLoadItem);
                break;
            }

            return mRetLoadItem;
        }

        // 资源加载完成，触发下一次加载
        protected void onMsgRouteResLoad(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            DownloadItem loadItem = (msg as LoadedWebResMR).m_task as DownloadItem;
            loadItem.handleResult();
        }
    }
}