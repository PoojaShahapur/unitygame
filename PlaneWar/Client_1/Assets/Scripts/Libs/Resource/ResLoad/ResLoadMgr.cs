﻿namespace SDK.Lib
{
    public class ResLoadMgr
    {
        protected uint mTotalNum;                   // 当前总共加载数量(包括同步和异步)
        protected uint mMaxAllowAsyncNum;                // 最多同时加载的内容(仅包含异步)
        protected uint mCurAsyncNum;                // 当前加载的数量(仅包含异步)

        protected ResLoadData mLoadData;
        protected LoadItem mRetLoadItem;
        protected ResItem mRetResItem;
        protected MList<string> mZeroRefResIDList;  // 没有引用的资源 ID 列表
        protected LoopDepth mLoopDepth;             // 加载深度

        public ResLoadMgr()
        {
            this.mTotalNum = 0;
            this.mMaxAllowAsyncNum = 8;
            this.mCurAsyncNum = 0;

            this.mLoadData = new ResLoadData();
            this.mZeroRefResIDList = new MList<string>();

            this.mLoopDepth = new LoopDepth();
            this.mLoopDepth.setZeroHandle(null, this.unloadNoRefResFromList);
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
            foreach(LoadItem loadItem in this.mLoadData.mPath2LDItem.Values)
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

            if (null == res)
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

            if (null == res)
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

                this.load(param);
            }
            else if (MacroDef.UNPKG_RES_LOAD)
            {
                // 判断资源所在的目录，是在 StreamingAssets 目录还是在 persistentData 目录下，目前由于没有完成，只能从 StreamingAssets 目录下加载
                param.mResPackType = ResPackType.eUnPakType;
                param.mResLoadType = ResLoadType.eLoadStreamingAssets;

                this.load(param);
            }
            else
            {
                this.load(param);
            }
        }

        // eBundleType 打包类型资源加载
        public void loadBundle(LoadParam param)
        {
            this.load(param);
        }

        // eLevelType 打包类型资源加载，都用协程加载
        protected void loadLevel(LoadParam param)
        {
            param.resolveLevel();

            if (MacroDef.PKG_RES_LOAD)
            {
                param.mResPackType = ResPackType.ePakLevelType;
                param.resolvePath();
                this.load(param);
            }
            else if (MacroDef.UNPKG_RES_LOAD)
            {
                param.mResPackType = ResPackType.eUnPakLevelType;
                param.mResLoadType = ResLoadType.eLoadStreamingAssets;
                this.load(param);
            }
            else
            {
                this.load(param);
            }
        }

        // 加载资源，内部决定加载方式，可能从 Resources 下加载或者从 StreamingAssets 下加载，或者从 PersistentDataPath 下加载。isCheckDep 是否检查依赖， "AssetBundleManifest" 这个依赖文件是不需要检查依赖的
        public void loadAsset(LoadParam param, bool isCheckDep = true)
        {
            if (param.mResPackType == ResPackType.eResourcesType)
            {
                param.mResPackType = ResPackType.eResourcesType;
                param.mResLoadType = ResLoadType.eLoadResource;

                this.loadResources(param);
            }
            else if (param.mResPackType == ResPackType.eBundleType)
            {
                param.mIsCheckDep = isCheckDep;
                this.loadBundle(param);
            }
            else if (param.mResPackType == ResPackType.eLevelType)
            {
                this.loadLevel(param);
            }
            else
            {
                this.loadData(param);
            }
        }

        public ResItem createResItem(LoadParam param)
        {
            ResItem resItem = this.findResFormPool(param.mResPackType);

            if (ResPackType.eLevelType == param.mResPackType)
            {
                if (null == resItem)
                {
                    resItem = new LevelResItem();
                }

                (resItem as LevelResItem).levelName = param.lvlName;
            }
            else if (ResPackType.eBundleType == param.mResPackType)
            {
                if (null == resItem)
                {
                    resItem = new BundleResItem();
                }

                (resItem as BundleResItem).prefabName = param.prefabName;
            }
            else if (ResPackType.eResourcesType == param.mResPackType)
            {
                if (null == resItem)
                {
                    resItem = new PrefabResItem();
                }

                (resItem as PrefabResItem).prefabName = param.prefabName;
            }
            else if (ResPackType.eDataType == param.mResPackType)
            {
                if (null == resItem)
                {
                    resItem = new DataResItem();
                }
            }
            else if (ResPackType.eUnPakType == param.mResPackType)
            {
                if (null == resItem)
                {
                    resItem = new ABUnPakComFileResItem();
                }
            }
            else if (ResPackType.eUnPakLevelType == param.mResPackType)
            {
                if (null == resItem)
                {
                    resItem = new ABUnPakLevelFileResItem();
                }

                (resItem as ABUnPakLevelFileResItem).levelName = param.lvlName;
            }
            else if (ResPackType.ePakType == param.mResPackType)
            {
                if (null == resItem)
                {
                    resItem = new ABPakComFileResItem();
                }
            }
            else if (ResPackType.ePakLevelType == param.mResPackType)
            {
                if (null == resItem)
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

            if (null != param.mLoadEventHandle)
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
                if (null == loadItem)
                {
                    loadItem = new ResourceLoadItem();
                }
            }
            else if (ResPackType.eBundleType == param.mResPackType)        // Bundle 打包模式
            {
                if (null == loadItem)
                {
                    loadItem = new BundleLoadItem();
                }
            }
            else if (ResPackType.eLevelType == param.mResPackType)
            {
                if (null == loadItem)
                {
                    loadItem = new LevelLoadItem();
                }

                (loadItem as LevelLoadItem).levelName = param.lvlName;
            }
            else if (ResPackType.eDataType == param.mResPackType)
            {
                if (null == loadItem)
                {
                    loadItem = new DataLoadItem();
                }
            }
            else if (ResPackType.eUnPakType == param.mResPackType || ResPackType.eUnPakLevelType == param.mResPackType)
            {
                if (null == loadItem)
                {
                    loadItem = new ABUnPakLoadItem();
                }
            }
            else if (ResPackType.ePakType == param.mResPackType || ResPackType.ePakLevelType == param.mResPackType)
            {
                if (null == loadItem)
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
            this.mLoadData.mPath2Res[param.mResUniqueId].refCountResLoadResultNotify.refCount.incRef();

            if (this.mLoadData.mPath2Res[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if(null != param.mLoadEventHandle)
                {
                    param.mLoadEventHandle(mLoadData.mPath2Res[param.mResUniqueId]);
                }
            }
            else
            {
                if (null != param.mLoadEventHandle)
                {
                    this.mLoadData.mPath2Res[param.mResUniqueId].refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, param.mLoadEventHandle);
                }
            }

            this.resetLoadParam(param);
        }

        protected void loadWithResCreatedAndNotLoad(LoadParam param, ResItem resItem)
        {
            this.mLoadData.mPath2Res[param.mResUniqueId] = resItem;
            this.mLoadData.mPath2Res[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.setLoading();

            // 如果不存在 LoadItem ，这个时候才需要创建，如果已经存在 LoadItem，这个时候不需要再次创建，这种情况通常发生在在加载一个资源，当 LoadItem 还没有加载完成，然后卸载了 ResItem，这个时候再次加载的时候，如果不判断，就会再次生成一个 LoadItem，这样 mPath2LDItem 字典里就会覆盖之前的 LoadItem，但是可能回调事件仍然存在，导致回调好几次
            if (!hasLoadItem(param.mResUniqueId))
            {
                LoadItem loadItem = this.createLoadItem(param);

                if (this.mCurAsyncNum < this.mMaxAllowAsyncNum)  // 如果没有超过最大界限
                {
                    // 先增加，否则退出的时候可能是先减 1 ，导致越界出现很大的值
                    ++this.mCurAsyncNum;
                    ++this.mTotalNum;

                    this.mLoadData.mPath2LDItem[param.mResUniqueId] = loadItem;
                    this.mLoadData.mPath2LDItem[param.mResUniqueId].load();
                }
                else if(!loadItem.loadNeedCoroutine)       // 如果超过，但是是同步加载，只能继续加载，否则逻辑层可能认为已经加载完成
                {
                    // 先增加，否则退出的时候可能是先减 1 ，导致越界出现很大的值
                    ++this.mTotalNum;
                    loadItem.setIsOverloading(true);

                    this.mLoadData.mPath2LDItem[param.mResUniqueId] = loadItem;
                    this.mLoadData.mPath2LDItem[param.mResUniqueId].load();
                }
                else    // 放入将要加载队列排队
                {
                    this.mLoadData.mWillLDItem.Add(loadItem);
                }
            }

            this.resetLoadParam(param);
        }

        protected void loadWithNotResCreatedAndNotLoad(LoadParam param)
        {
            ResItem resItem = this.createResItem(param);
            this.loadWithResCreatedAndNotLoad(param, resItem);
        }

        // 通用类型，需要自己设置很多参数
        public void load(LoadParam param)
        {
            this.mLoopDepth.incDepth();

            if (this.mLoadData.mPath2Res.ContainsKey(param.mResUniqueId))
            {
                this.loadWithResCreatedAndLoad(param);
            }
            else if(null != param.mLoadRes)
            {
                this.loadWithResCreatedAndNotLoad(param, this.mLoadData.mPath2Res[param.mResUniqueId]);
            }
            else
            {
                this.loadWithNotResCreatedAndNotLoad(param);
            }

            this.mLoopDepth.decDepth();

            //if (!this.mLoopDepth.isInDepth())
            //{
            //    this.unloadNoRefResFromList();
            //}
        }

        public ResItem getAndLoad(LoadParam param)
        {
            param.resolvePath();
            this.load(param);

            return getResource(param.mResUniqueId);
        }

        // 这个卸载有引用计数，如果有引用计数就卸载不了
        public void unload(string resUniqueId, MAction<IDispatchObject> loadEventHandle)
        {
            if (this.mLoadData.mPath2Res.ContainsKey(resUniqueId))
            {
                // 移除事件监听器，因为很有可能移除的时候，资源还没加载完成，这个时候事件监听器中的处理函数列表还没有清理
                this.mLoadData.mPath2Res[resUniqueId].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, loadEventHandle);
                this.mLoadData.mPath2Res[resUniqueId].refCountResLoadResultNotify.refCount.decRef();

                if (this.mLoadData.mPath2Res[resUniqueId].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    if (this.mLoopDepth.isInDepth())
                    {
                        this.addNoRefResID2List(resUniqueId);
                    }
                    else
                    {
                        this.unloadNoRef(resUniqueId);
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
            this.mZeroRefResIDList.Add(resUniqueId);
        }

        // 卸载没有引用的资源列表中的资源
        protected void unloadNoRefResFromList()
        {
            int idx = 0;
            int len = this.mZeroRefResIDList.Count();
            string path = "";

            while(idx < len)
            {
                path = this.mZeroRefResIDList[idx];

                // 防止在真正卸载前，又被引用，例如加载完成后，立刻卸载资源，然后在立刻加载资源
                if (this.mLoadData.mPath2Res[path].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    this.unloadNoRef(path);
                }

                ++idx;
            }

            this.mZeroRefResIDList.Clear();
        }

        // 不考虑引用计数，直接卸载
        protected void unloadNoRef(string resUniqueId)
        {
            if (this.mLoadData.mPath2Res.ContainsKey(resUniqueId))
            {
                this.mLoadData.mPath2Res[resUniqueId].unload();
                this.mLoadData.mPath2Res[resUniqueId].reset();
                this.mLoadData.mNoUsedResItem.Add(mLoadData.mPath2Res[resUniqueId]);
                this.mLoadData.mPath2Res.Remove(resUniqueId);

                // 检查是否存在还没有执行的 LoadItem，如果存在就直接移除
                this.removeWillLoadItem(resUniqueId);
            }
            else
            {
                
            }
        }

        public void removeWillLoadItem(string resUniqueId)
        {
            foreach(LoadItem loadItem in this.mLoadData.mWillLDItem)
            {
                if(loadItem.getResUniqueId() == resUniqueId)
                {
                    this.releaseLoadItem(loadItem);      // 必然只有一个，如果有多个就是错误

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
                this.onLoaded(item);
            }
            else if (item.nonRefCountResLoadResultNotify.resLoadState.hasFailed())
            {
                this.onFailed(item);
            }

            bool isOverloading = item.isOverloading();
            this.releaseLoadItem(item);

            --this.mTotalNum;

            if (!isOverloading)
            {
                --this.mCurAsyncNum;
                this.loadNextItem();
            }
        }

        public void onLoaded(LoadItem item)
        {
            this.mLoopDepth.incDepth();

            item.onLoaded();

            if (this.mLoadData.mPath2Res.ContainsKey(item.getResUniqueId()))
            {
                this.mLoadData.mPath2Res[item.getResUniqueId()].init(this.mLoadData.mPath2LDItem[item.getResUniqueId()]);
            }
            else        // 如果资源已经没有使用的地方了
            {
                item.unload();          // 直接卸载掉
            }

            this.mLoopDepth.decDepth();
        }

        public void onFailed(LoadItem item)
        {
            this.mLoopDepth.incDepth();

            item.onFailed();

            string resUniqueId = item.getResUniqueId();

            if (this.mLoadData.mPath2Res.ContainsKey(resUniqueId))
            {
                this.mLoadData.mPath2Res[resUniqueId].failed(this.mLoadData.mPath2LDItem[resUniqueId]);
            }

            this.mLoopDepth.decDepth();
        }

        protected void releaseLoadItem(LoadItem item)
        {
            item.reset();

            this.mLoadData.mNoUsedLDItem.Add(item);
            this.mLoadData.mWillLDItem.Remove(item);
            this.mLoadData.mPath2LDItem.Remove(item.getResUniqueId());
        }

        protected void loadNextItem()
        {
            if (this.mCurAsyncNum < this.mMaxAllowAsyncNum)
            {
                if (this.mLoadData.mWillLDItem.Count > 0)
                {
                    string resUniqueId = (mLoadData.mWillLDItem[0] as LoadItem).getResUniqueId();
                    this.mLoadData.mPath2LDItem[resUniqueId] = this.mLoadData.mWillLDItem[0] as LoadItem;
                    this.mLoadData.mWillLDItem.RemoveAt(0);
                    this.mLoadData.mPath2LDItem[resUniqueId].load();

                    ++this.mCurAsyncNum;
                    ++this.mTotalNum;
                }
            }
        }

        protected ResItem findResFormPool(ResPackType type)
        {
            this.mRetResItem = null;

            foreach (ResItem item in this.mLoadData.mNoUsedResItem)
            {
                if (item.resPackType == type)
                {
                    this.mRetResItem = item;
                    this.mLoadData.mNoUsedResItem.Remove(mRetResItem);

                    break;
                }
            }

            return this.mRetResItem;
        }

        protected LoadItem findLoadItemFormPool(ResPackType type)
        {
            this.mRetLoadItem = null;

            foreach (LoadItem item in this.mLoadData.mNoUsedLDItem)
            {
                if (item.resPackType == type)
                {
                    this.mRetLoadItem = item;
                    this.mLoadData.mNoUsedLDItem.Remove(this.mRetLoadItem);

                    break;
                }
            }

            return this.mRetLoadItem;
        }
    }
}