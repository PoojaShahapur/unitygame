using System.Collections.Generic;

namespace SDK.Lib
{
    public class ResLoadMgr
    {
        protected uint mMaxParral;                             // 最多同时加载的内容
        protected uint mCurNum;                                // 当前加载的数量
        protected ResLoadData mLoadData;
        protected LoadItem mRetLoadItem;
        protected ResItem mRetResItem;
        protected List<string> mZeroRefResIDList;      // 没有引用的资源 ID 列表
        protected int mLoadingDepth;                   // 加载深度

        public ResLoadMgr()
        {
            mMaxParral = 8;
            mCurNum = 0;
            mLoadData = new ResLoadData();
            mZeroRefResIDList = new List<string>();
            mLoadingDepth = 0;
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
            foreach(LoadItem loadItem in mLoadData.mPath2LDItem.Values)
            {
                if(loadItem.getResUniqueId() == resUniqueId)
                {
                    return true;
                }
            }

            foreach(LoadItem loadItem in mLoadData.mWillLDItem)
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
            if (mLoadData.mPath2Res.ContainsKey(resUniqueId))
            {
                return mLoadData.mPath2Res[resUniqueId];
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
            mLoadData.mPath2Res[param.mResUniqueId].refCountResLoadResultNotify.refCount.incRef();
            if (mLoadData.mPath2Res[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.mLoadEventHandle != null)
                {
                    param.mLoadEventHandle(mLoadData.mPath2Res[param.mResUniqueId]);
                }
            }
            else
            {
                if (param.mLoadEventHandle != null)
                {
                    mLoadData.mPath2Res[param.mResUniqueId].refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, param.mLoadEventHandle);
                }
            }

            resetLoadParam(param);
        }

        protected void loadWithResCreatedAndNotLoad(LoadParam param, ResItem resItem)
        {
            mLoadData.mPath2Res[param.mResUniqueId] = resItem;
            mLoadData.mPath2Res[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.setLoading();

            // 如果不存在 LoadItem ，这个时候才需要创建，如果已经存在 LoadItem，这个时候不需要再次创建，这种情况通常发生在在加载一个资源，当 LoadItem 还没有加载完成，然后卸载了 ResItem，这个时候再次加载的时候，如果不判断，就会再次生成一个 LoadItem，这样 mPath2LDItem 字典里就会覆盖之前的 LoadItem，但是可能回调事件仍然存在，导致回调好几次
            if (!hasLoadItem(param.mResUniqueId))
            {
                LoadItem loadItem = createLoadItem(param);

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

        protected void loadWithNotResCreatedAndNotLoad(LoadParam param)
        {
            ResItem resItem = createResItem(param);
            loadWithResCreatedAndNotLoad(param, resItem);
        }

        // 通用类型，需要自己设置很多参数
        public void load(LoadParam param)
        {
            ++mLoadingDepth;

            if (mLoadData.mPath2Res.ContainsKey(param.mResUniqueId))
            {
                loadWithResCreatedAndLoad(param);
            }
            else if(param.mLoadRes != null)
            {
                loadWithResCreatedAndNotLoad(param, mLoadData.mPath2Res[param.mResUniqueId]);
            }
            else
            {
                loadWithNotResCreatedAndNotLoad(param);
            }

            --mLoadingDepth;

            if (mLoadingDepth == 0)
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
            if (mLoadData.mPath2Res.ContainsKey(resUniqueId))
            {
                // 移除事件监听器，因为很有可能移除的时候，资源还没加载完成，这个时候事件监听器中的处理函数列表还没有清理
                mLoadData.mPath2Res[resUniqueId].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, loadEventHandle);
                mLoadData.mPath2Res[resUniqueId].refCountResLoadResultNotify.refCount.decRef();
                if (mLoadData.mPath2Res[resUniqueId].refCountResLoadResultNotify.refCount.isNoRef())
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
            foreach(string resUniqueId in mLoadData.mPath2Res.Keys)
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
            mZeroRefResIDList.Add(resUniqueId);
        }

        // 卸载没有引用的资源列表中的资源
        protected void unloadNoRefResFromList()
        {
            foreach(string path in mZeroRefResIDList)
            {
                if (mLoadData.mPath2Res[path].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    unloadNoRef(path);
                }
            }

            mZeroRefResIDList.Clear();
        }

        // 不考虑引用计数，直接卸载
        protected void unloadNoRef(string resUniqueId)
        {
            if (mLoadData.mPath2Res.ContainsKey(resUniqueId))
            {
                mLoadData.mPath2Res[resUniqueId].unload();
                mLoadData.mPath2Res[resUniqueId].reset();
                mLoadData.mNoUsedResItem.Add(mLoadData.mPath2Res[resUniqueId]);
                mLoadData.mPath2Res.Remove(resUniqueId);

                // 检查是否存在还没有执行的 LoadItem，如果存在就直接移除
                removeWillLoadItem(resUniqueId);
            }
            else
            {
                
            }
        }

        public void removeWillLoadItem(string resUniqueId)
        {
            foreach(LoadItem loadItem in mLoadData.mWillLDItem)
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
            --mCurNum;
            loadNextItem();
        }

        public void onLoaded(LoadItem item)
        {
            if (mLoadData.mPath2Res.ContainsKey(item.getResUniqueId()))
            {
                mLoadData.mPath2Res[item.getResUniqueId()].init(mLoadData.mPath2LDItem[item.getResUniqueId()]);
            }
            else        // 如果资源已经没有使用的地方了
            {
                item.unload();          // 直接卸载掉
            }
        }

        public void onFailed(LoadItem item)
        {
            string resUniqueId = item.getResUniqueId();
            if (mLoadData.mPath2Res.ContainsKey(resUniqueId))
            {
                mLoadData.mPath2Res[resUniqueId].failed(mLoadData.mPath2LDItem[resUniqueId]);
            }
        }

        protected void releaseLoadItem(LoadItem item)
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
                    mLoadData.mPath2LDItem[resUniqueId] = mLoadData.mWillLDItem[0] as LoadItem;
                    mLoadData.mWillLDItem.RemoveAt(0);
                    mLoadData.mPath2LDItem[resUniqueId].load();

                    ++mCurNum;
                }
            }
        }

        protected ResItem findResFormPool(ResPackType type)
        {
            mRetResItem = null;

            foreach (ResItem item in mLoadData.mNoUsedResItem)
            {
                if (item.resPackType == type)
                {
                    mRetResItem = item;
                    mLoadData.mNoUsedResItem.Remove(mRetResItem);
                    break;
                }
            }

            return mRetResItem;
        }

        protected LoadItem findLoadItemFormPool(ResPackType type)
        {
            mRetLoadItem = null;

            foreach (LoadItem item in mLoadData.mNoUsedLDItem)
            {
                if (item.resPackType == type)
                {
                    mRetLoadItem = item;
                    mLoadData.mNoUsedLDItem.Remove(mRetLoadItem);
                    break;
                }
            }

            return mRetLoadItem;
        }
    }
}