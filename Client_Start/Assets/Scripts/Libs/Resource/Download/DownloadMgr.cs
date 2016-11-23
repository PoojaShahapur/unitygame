using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 数据下载管理器
     */
    public class DownloadMgr : MsgRouteHandleBase
    {
        protected uint m_maxParral;                             // 最多同时加载的内容
        protected uint m_curNum;                                // 当前加载的数量
        protected DownloadData m_LoadData;
        protected DownloadItem m_retLoadItem;
        protected ResMsgRouteCB m_resMsgRouteCB;
        protected List<string> m_zeroRefResIDList;      // 没有引用的资源 ID 列表
        protected int m_loadingDepth;                   // 加载深度

        public DownloadMgr()
        {
            m_maxParral = 8;
            m_curNum = 0;
            m_LoadData = new DownloadData();
            m_zeroRefResIDList = new List<string>();
            m_loadingDepth = 0;

            this.addMsgRouteHandle(MsgRouteID.eMRIDLoadedWebRes, onMsgRouteResLoad);
        }

        public void postInit()
        {
            // 游戏逻辑处理
            m_resMsgRouteCB = new ResMsgRouteCB();
            Ctx.mInstance.mMsgRouteNotify.addOneDisp(m_resMsgRouteCB);
        }

        protected void resetLoadParam(DownloadParam loadParam)
        {
            loadParam.reset();
        }

        // 是否有正在加载的 DownloadItem
        public bool hasDownloadItem(string resUniqueId)
        {
            foreach (DownloadItem loadItem in m_LoadData.m_path2LDItem.Values)
            {
                if (loadItem.getResUniqueId() == resUniqueId)
                {
                    return true;
                }
            }

            foreach (DownloadItem loadItem in m_LoadData.m_willLDItem)
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
            foreach (DownloadItem loadItem in m_LoadData.m_path2LDItem.Values)
            {
                if (loadItem.getResUniqueId() == resUniqueId)
                {
                    return loadItem;
                }
            }

            foreach (DownloadItem loadItem in m_LoadData.m_willLDItem)
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
            m_LoadData.m_path2LDItem[param.mResUniqueId].refCountResLoadResultNotify.refCount.incRef();
            if (m_LoadData.m_path2LDItem[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.mLoadEventHandle != null)
                {
                    param.mLoadEventHandle(m_LoadData.m_path2LDItem[param.mResUniqueId]);
                }
            }
            else
            {
                if (param.mLoadEventHandle != null)
                {
                    m_LoadData.m_path2LDItem[param.mResUniqueId].allLoadResEventDispatch.addEventHandle(null, param.mLoadEventHandle);
                }
            }

            resetLoadParam(param);
        }

        protected void downloadWithNotDownload(DownloadParam param)
        {
            if (!hasDownloadItem(param.mResUniqueId))
            {
                DownloadItem loadItem = createDownloadItem(param);

                if (m_curNum < m_maxParral)
                {
                    // 先增加，否则退出的时候可能是先减 1 ，导致越界出现很大的值
                    ++m_curNum;
                    m_LoadData.m_path2LDItem[param.mResUniqueId] = loadItem;
                    m_LoadData.m_path2LDItem[param.mResUniqueId].load();
                }
                else
                {
                    m_LoadData.m_willLDItem.Add(loadItem);
                }
            }

            resetLoadParam(param);
        }

        // 通用类型，需要自己设置很多参数
        public void load(DownloadParam param)
        {
            ++m_loadingDepth;
            if (m_LoadData.m_path2LDItem.ContainsKey(param.mResUniqueId))
            {
                downloadWithDownloading(param);
            }
            else
            {
                downloadWithNotDownload(param);
            }
            --m_loadingDepth;

            if (m_loadingDepth == 0)
            {
                unloadNoRefResFromList();
            }
        }

        public DownloadItem getAndDownload(DownloadParam param)
        {
            //param.resolvePath();
            load(param);
            return getDownloadItem(param.mResUniqueId);
        }

        // 这个卸载有引用计数，如果有引用计数就卸载不了
        public void unload(string resUniqueId, MAction<IDispatchObject> loadEventHandle)
        {
            if (m_LoadData.m_path2LDItem.ContainsKey(resUniqueId))
            {
                // 移除事件监听器，因为很有可能移除的时候，资源还没加载完成，这个时候事件监听器中的处理函数列表还没有清理
                m_LoadData.m_path2LDItem[resUniqueId].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, loadEventHandle);
                m_LoadData.m_path2LDItem[resUniqueId].refCountResLoadResultNotify.refCount.decRef();
                if (m_LoadData.m_path2LDItem[resUniqueId].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    if (m_loadingDepth != 0)
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
            foreach (string resUniqueId in m_LoadData.m_path2LDItem.Keys)
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
            m_zeroRefResIDList.Add(resUniqueId);
        }

        // 卸载没有引用的资源列表中的资源
        protected void unloadNoRefResFromList()
        {
            foreach (string path in m_zeroRefResIDList)
            {
                if (m_LoadData.m_path2LDItem[path].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    unloadNoRef(path);
                }
            }
            m_zeroRefResIDList.Clear();
        }

        // 不考虑引用计数，直接卸载
        protected void unloadNoRef(string resUniqueId)
        {
            if (m_LoadData.m_path2LDItem.ContainsKey(resUniqueId))
            {
                m_LoadData.m_path2LDItem[resUniqueId].unload();
                m_LoadData.m_path2LDItem[resUniqueId].reset();
                m_LoadData.m_noUsedLDItem.Add(m_LoadData.m_path2LDItem[resUniqueId]);
                m_LoadData.m_path2LDItem.Remove(resUniqueId);

                // 检查是否存在还没有执行的 LoadItem，如果存在就直接移除
                removeWillLoadItem(resUniqueId);
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("DownloadMgr::unloadNoRef, Path Can not Find， ResUniqueId is {0}", resUniqueId));
            }
        }

        public void removeWillLoadItem(string resUniqueId)
        {
            foreach (DownloadItem loadItem in m_LoadData.m_willLDItem)
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
            --m_curNum;
            loadNextItem();
        }

        public void onLoaded(DownloadItem item)
        {
            if (m_LoadData.m_path2LDItem.ContainsKey(item.getResUniqueId()))
            {
                m_LoadData.m_path2LDItem[item.getResUniqueId()].init();
            }
            else        // 如果资源已经没有使用的地方了
            {
                item.unload();          // 直接卸载掉
            }
        }

        public void onFailed(DownloadItem item)
        {
            string resUniqueId = item.getResUniqueId();
            if (m_LoadData.m_path2LDItem.ContainsKey(resUniqueId))
            {
                m_LoadData.m_path2LDItem[resUniqueId].failed();
            }
        }

        protected void releaseLoadItem(DownloadItem item)
        {
            item.reset();
            m_LoadData.m_noUsedLDItem.Add(item);
            m_LoadData.m_willLDItem.Remove(item);
            m_LoadData.m_path2LDItem.Remove(item.getResUniqueId());
        }

        protected void loadNextItem()
        {
            if (m_curNum < m_maxParral)
            {
                if (m_LoadData.m_willLDItem.Count > 0)
                {
                    string resUniqueId = (m_LoadData.m_willLDItem[0] as LoadItem).getResUniqueId();
                    m_LoadData.m_path2LDItem[resUniqueId] = m_LoadData.m_willLDItem[0] as DownloadItem;
                    m_LoadData.m_willLDItem.RemoveAt(0);
                    m_LoadData.m_path2LDItem[resUniqueId].load();

                    ++m_curNum;
                }
            }
        }

        protected DownloadItem findDownloadItemFormPool()
        {
            m_retLoadItem = null;
            foreach (DownloadItem item in m_LoadData.m_noUsedLDItem)
            {
                m_retLoadItem = item;
                m_LoadData.m_noUsedLDItem.Remove(m_retLoadItem);
                break;
            }

            return m_retLoadItem;
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