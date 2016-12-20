using System.Collections.Generic;

namespace SDK.Lib
{
    public class ResLoadMgr
    {
        protected uint m_maxParral;                             // 最多同时加载的内容
        protected uint m_curNum;                                // 当前加载的数量
        protected ResLoadData m_LoadData;
        protected LoadItem m_retLoadItem;
        protected ResItem m_retResItem;
        protected List<string> m_zeroRefResIDList;      // 没有引用的资源 ID 列表
        protected int m_loadingDepth;                   // 加载深度

        public ResLoadMgr()
        {
            m_maxParral = 8;
            m_curNum = 0;
            m_LoadData = new ResLoadData();
            m_zeroRefResIDList = new List<string>();
            m_loadingDepth = 0;
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        // 是否有正在加载的 LoadItem
        public bool hasLoadItem(string resUniqueId)
        {
            foreach(LoadItem loadItem in m_LoadData.m_path2LDItem.Values)
            {
                if(loadItem.getResUniqueId() == resUniqueId)
                {
                    return true;
                }
            }

            foreach(LoadItem loadItem in m_LoadData.m_willLDItem)
            {
                if (loadItem.getResUniqueId() == resUniqueId)
                {
                    return true;
                }
            }

            return false;
        }

        // 重置加载设置
        protected void resetLoadParam(LoadParam loadParam)
        {
            loadParam.mLoadNeedCoroutine = true;
            loadParam.mResNeedCoroutine = true;
        }

        // 资源是否已经加载，包括成功和失败
        public bool isResLoaded(string resUniqueId)
        {
            ResItem res = this.getResource(resUniqueId);
            if (res == null)
            {
                return false;
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded() ||
                res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                return true;
            }

            return false;
        }

        public bool isResSuccessLoaded(string resUniqueId)
        {
            ResItem res = this.getResource(resUniqueId);

            if (res == null)
            {
                return false;
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                return true;
            }

            return false;
        }

        public ResItem getResource(string resUniqueId)
        {
            // 如果 path == null ，程序会宕机
            if (m_LoadData.m_path2Res.ContainsKey(resUniqueId))
            {
                return m_LoadData.m_path2Res[resUniqueId];
            }
            else
            {
                return null;
            }
        }

        public void loadData(LoadParam param)
        {
            load(param);
        }

        // eResourcesType 打包类型资源加载
        public void loadResources(LoadParam param)
        {
            if (MacroDef.PKG_RES_LOAD)
            {
                if (param.mLoadPath.IndexOf(PakSys.PAK_EXT) != -1)     // 如果加载的是打包文件
                {
                    param.mResPackType = ResPackType.ePakType;
                }
                else        // 加载的是非打包文件
                {
                    param.mResPackType = ResPackType.eUnPakType;
                }
                load(param);
            }
            else if (MacroDef.UNPKG_RES_LOAD)
            {
                // 判断资源所在的目录，是在 StreamingAssets 目录还是在 persistentData 目录下，目前由于没有完成，只能从 StreamingAssets 目录下加载
                param.mResPackType = ResPackType.eUnPakType;
                param.mResLoadType = ResLoadType.eLoadStreamingAssets;
                load(param);
            }
            else
            {
                load(param);
            }
        }

        // eBundleType 打包类型资源加载
        public void loadBundle(LoadParam param)
        {
            load(param);
        }

        // eLevelType 打包类型资源加载，都用协程加载
        protected void loadLevel(LoadParam param)
        {
            param.resolveLevel();

            if (MacroDef.PKG_RES_LOAD)
            {
                param.mResPackType = ResPackType.ePakLevelType;
                param.resolvePath();
                load(param);
            }
            else if (MacroDef.UNPKG_RES_LOAD)
            {
                param.mResPackType = ResPackType.eUnPakLevelType;
                param.mResLoadType = ResLoadType.eLoadStreamingAssets;
                load(param);
            }
            else
            {
                load(param);
            }
        }

        // 加载资源，内部决定加载方式，可能从 Resources 下加载或者从 StreamingAssets 下加载，或者从 PersistentDataPath 下加载。isCheckDep 是否检查依赖， "AssetBundleManifest" 这个依赖文件是不需要检查依赖的
        public void loadAsset(LoadParam param, bool isCheckDep = true)
        {
            if (param.mResPackType == ResPackType.eResourcesType)
            {
                param.mResPackType = ResPackType.eResourcesType;
                param.mResLoadType = ResLoadType.eLoadResource;

                loadResources(param);
            }
            else if (param.mResPackType == ResPackType.eBundleType)
            {
                param.mIsCheckDep = isCheckDep;
                loadBundle(param);
            }
            else if (param.mResPackType == ResPackType.eLevelType)
            {
                loadLevel(param);
            }
            else
            {
                loadData(param);
            }
        }

        public ResItem createResItem(LoadParam param)
        {
            ResItem resItem = findResFormPool(param.mResPackType);
            if (ResPackType.eLevelType == param.mResPackType)
            {
                if (resItem == null)
                {
                    resItem = new LevelResItem();
                }
                (resItem as LevelResItem).levelName = param.lvlName;
            }
            else if (ResPackType.eBundleType == param.mResPackType)
            {
                if (resItem == null)
                {
                    resItem = new BundleResItem();
                }

                (resItem as BundleResItem).prefabName = param.prefabName;
            }
            else if (ResPackType.eResourcesType == param.mResPackType)
            {
                if (resItem == null)
                {
                    resItem = new PrefabResItem();
                }

                (resItem as PrefabResItem).prefabName = param.prefabName;
            }
            else if (ResPackType.eDataType == param.mResPackType)
            {
                if (resItem == null)
                {
                    resItem = new DataResItem();
                }
            }
            else if (ResPackType.eUnPakType == param.mResPackType)
            {
                if (resItem == null)
                {
                    resItem = new ABUnPakComFileResItem();
                }
            }
            else if (ResPackType.eUnPakLevelType == param.mResPackType)
            {
                if (resItem == null)
                {
                    resItem = new ABUnPakLevelFileResItem();
                }
                (resItem as ABUnPakLevelFileResItem).levelName = param.lvlName;
            }
            else if (ResPackType.ePakType == param.mResPackType)
            {
                if (resItem == null)
                {
                    resItem = new ABPakComFileResItem();
                }
            }
            else if (ResPackType.ePakLevelType == param.mResPackType)
            {
                if (resItem == null)
                {
                    resItem = new ABPakLevelFileResItem();
                }
                (resItem as ABPakLevelFileResItem).levelName = param.lvlName;
            }
            //else if (ResPackType.ePakMemType == param.mResPackType)
            //{
            //    if (resitem == null)
            //    {
            //        resitem = new ABMemUnPakComFileResItem();
            //    }
            //}
            //else if (ResPackType.ePakMemLevelType == param.mResPackType)
            //{
            //    if (resitem == null)
            //    {
            //        resitem = new ABMemUnPakLevelFileResItem();
            //    }

            //    (resitem as ABMemUnPakLevelFileResItem).levelName = param.lvlName;
            //}

            resItem.refCountResLoadResultNotify.refCount.incRef();
            resItem.setLoadParam(param);

            if (param.mLoadEventHandle != null)
            {
                resItem.refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(
                    null, 
                    param.mLoadEventHandle
                    );
            }

            return resItem;
        }

        protected LoadItem createLoadItem(LoadParam param)
        {
            LoadItem loadItem = findLoadItemFormPool(param.mResPackType);

            if (ResPackType.eResourcesType == param.mResPackType)        // 默认 Bundle 中资源
            {
                if (loadItem == null)
                {
                    loadItem = new ResourceLoadItem();
                }
            }
            else if (ResPackType.eBundleType == param.mResPackType)        // Bundle 打包模式
            {
                if (loadItem == null)
                {
                    loadItem = new BundleLoadItem();
                }
            }
            else if (ResPackType.eLevelType == param.mResPackType)
            {
                if (loadItem == null)
                {
                    loadItem = new LevelLoadItem();
                }

                (loadItem as LevelLoadItem).levelName = param.lvlName;
            }
            else if (ResPackType.eDataType == param.mResPackType)
            {
                if (loadItem == null)
                {
                    loadItem = new DataLoadItem();
                }
            }
            else if (ResPackType.eUnPakType == param.mResPackType || ResPackType.eUnPakLevelType == param.mResPackType)
            {
                if (loadItem == null)
                {
                    loadItem = new ABUnPakLoadItem();
                }
            }
            else if (ResPackType.ePakType == param.mResPackType || ResPackType.ePakLevelType == param.mResPackType)
            {
                if (loadItem == null)
                {
                    loadItem = new ABPakLoadItem();
                }
            }

            loadItem.setLoadParam(param);
            loadItem.nonRefCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, onLoadEventHandle);

            return loadItem;
        }

        // 资源创建并且正在被加载
        protected void loadWithResCreatedAndLoad(LoadParam param)
        {
            m_LoadData.m_path2Res[param.mResUniqueId].refCountResLoadResultNotify.refCount.incRef();
            if (m_LoadData.m_path2Res[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.mLoadEventHandle != null)
                {
                    param.mLoadEventHandle(m_LoadData.m_path2Res[param.mResUniqueId]);
                }
            }
            else
            {
                if (param.mLoadEventHandle != null)
                {
                    m_LoadData.m_path2Res[param.mResUniqueId].refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, param.mLoadEventHandle);
                }
            }

            resetLoadParam(param);
        }

        protected void loadWithResCreatedAndNotLoad(LoadParam param, ResItem resItem)
        {
            m_LoadData.m_path2Res[param.mResUniqueId] = resItem;
            m_LoadData.m_path2Res[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.setLoading();

            // 如果不存在 LoadItem ，这个时候才需要创建，如果已经存在 LoadItem，这个时候不需要再次创建，这种情况通常发生在在加载一个资源，当 LoadItem 还没有加载完成，然后卸载了 ResItem，这个时候再次加载的时候，如果不判断，就会再次生成一个 LoadItem，这样 m_path2LDItem 字典里就会覆盖之前的 LoadItem，但是可能回调事件仍然存在，导致回调好几次
            if (!hasLoadItem(param.mResUniqueId))
            {
                LoadItem loadItem = createLoadItem(param);

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

        protected void loadWithNotResCreatedAndNotLoad(LoadParam param)
        {
            ResItem resItem = createResItem(param);
            loadWithResCreatedAndNotLoad(param, resItem);
        }

        // 通用类型，需要自己设置很多参数
        public void load(LoadParam param)
        {
            ++m_loadingDepth;

            if (m_LoadData.m_path2Res.ContainsKey(param.mResUniqueId))
            {
                loadWithResCreatedAndLoad(param);
            }
            else if(param.mLoadRes != null)
            {
                loadWithResCreatedAndNotLoad(param, m_LoadData.m_path2Res[param.mResUniqueId]);
            }
            else
            {
                loadWithNotResCreatedAndNotLoad(param);
            }

            --m_loadingDepth;

            if (m_loadingDepth == 0)
            {
                unloadNoRefResFromList();
            }
        }

        public ResItem getAndLoad(LoadParam param)
        {
            param.resolvePath();
            load(param);

            return getResource(param.mResUniqueId);
        }

        // 这个卸载有引用计数，如果有引用计数就卸载不了
        public void unload(string resUniqueId, MAction<IDispatchObject> loadEventHandle)
        {
            if (m_LoadData.m_path2Res.ContainsKey(resUniqueId))
            {
                // 移除事件监听器，因为很有可能移除的时候，资源还没加载完成，这个时候事件监听器中的处理函数列表还没有清理
                m_LoadData.m_path2Res[resUniqueId].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, loadEventHandle);
                m_LoadData.m_path2Res[resUniqueId].refCountResLoadResultNotify.refCount.decRef();
                if (m_LoadData.m_path2Res[resUniqueId].refCountResLoadResultNotify.refCount.isNoRef())
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
            foreach(string resUniqueId in m_LoadData.m_path2Res.Keys)
            {
                resUniqueIdList.Add(resUniqueId);
            }

            int idx = 0;
            int len = resUniqueIdList.length();
            while(idx < len)
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
            foreach(string path in m_zeroRefResIDList)
            {
                if (m_LoadData.m_path2Res[path].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    unloadNoRef(path);
                }
            }

            m_zeroRefResIDList.Clear();
        }

        // 不考虑引用计数，直接卸载
        protected void unloadNoRef(string resUniqueId)
        {
            if (m_LoadData.m_path2Res.ContainsKey(resUniqueId))
            {
                m_LoadData.m_path2Res[resUniqueId].unload();
                m_LoadData.m_path2Res[resUniqueId].reset();
                m_LoadData.m_noUsedResItem.Add(m_LoadData.m_path2Res[resUniqueId]);
                m_LoadData.m_path2Res.Remove(resUniqueId);

                // 检查是否存在还没有执行的 LoadItem，如果存在就直接移除
                removeWillLoadItem(resUniqueId);
            }
            else
            {
                
            }
        }

        public void removeWillLoadItem(string resUniqueId)
        {
            foreach(LoadItem loadItem in m_LoadData.m_willLDItem)
            {
                if(loadItem.getResUniqueId() == resUniqueId)
                {
                    releaseLoadItem(loadItem);      // 必然只有一个，如果有多个就是错误
                    break;
                }
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            LoadItem item = dispObj as LoadItem;
            item.nonRefCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, onLoadEventHandle);
            if (item.nonRefCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                onLoaded(item);
            }
            else if (item.nonRefCountResLoadResultNotify.resLoadState.hasFailed())
            {
                onFailed(item);
            }

            releaseLoadItem(item);
            --m_curNum;
            loadNextItem();
        }

        public void onLoaded(LoadItem item)
        {
            if (m_LoadData.m_path2Res.ContainsKey(item.getResUniqueId()))
            {
                m_LoadData.m_path2Res[item.getResUniqueId()].init(m_LoadData.m_path2LDItem[item.getResUniqueId()]);
            }
            else        // 如果资源已经没有使用的地方了
            {
                item.unload();          // 直接卸载掉
            }
        }

        public void onFailed(LoadItem item)
        {
            string resUniqueId = item.getResUniqueId();
            if (m_LoadData.m_path2Res.ContainsKey(resUniqueId))
            {
                m_LoadData.m_path2Res[resUniqueId].failed(m_LoadData.m_path2LDItem[resUniqueId]);
            }
        }

        protected void releaseLoadItem(LoadItem item)
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
                    m_LoadData.m_path2LDItem[resUniqueId] = m_LoadData.m_willLDItem[0] as LoadItem;
                    m_LoadData.m_willLDItem.RemoveAt(0);
                    m_LoadData.m_path2LDItem[resUniqueId].load();

                    ++m_curNum;
                }
            }
        }

        protected ResItem findResFormPool(ResPackType type)
        {
            m_retResItem = null;

            foreach (ResItem item in m_LoadData.m_noUsedResItem)
            {
                if (item.resPackType == type)
                {
                    m_retResItem = item;
                    m_LoadData.m_noUsedResItem.Remove(m_retResItem);
                    break;
                }
            }

            return m_retResItem;
        }

        protected LoadItem findLoadItemFormPool(ResPackType type)
        {
            m_retLoadItem = null;

            foreach (LoadItem item in m_LoadData.m_noUsedLDItem)
            {
                if (item.resPackType == type)
                {
                    m_retLoadItem = item;
                    m_LoadData.m_noUsedLDItem.Remove(m_retLoadItem);
                    break;
                }
            }

            return m_retLoadItem;
        }
    }
}