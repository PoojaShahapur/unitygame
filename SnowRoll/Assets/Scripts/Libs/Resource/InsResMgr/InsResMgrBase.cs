using LuaInterface;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 资源管理器，不包括资源加载
     */
    public class InsResMgrBase
    {
        public MDictionary<string, InsResBase> mPath2ResDic;
        protected MList<string> mZeroRefResIDList;      // 没有引用的资源 ID 列表
        protected int mLoadingDepth;          // 加载深度

        public InsResMgrBase()
        {
            mPath2ResDic = new MDictionary<string, InsResBase>();
            mZeroRefResIDList = new MList<string>();
            mLoadingDepth = 0;
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {
            this.unloadAll();
        }

        public T getAndSyncLoad<T>(string path, MAction<IDispatchObject> loadEventHandle, bool isLoadAll = false) where T : InsResBase, new()
        {
            syncLoad<T>(path, loadEventHandle, isLoadAll);
            string resUniqueId = LoadParam.convOrigPathToUniqueId(path);
            return getRes(resUniqueId) as T;
        }

        public T getAndAsyncLoad<T>(string path, LuaTable luaTable = null, LuaFunction luaFunction = null, bool isLoadAll = false) where T : InsResBase, new()
        {            
            T ret = null;
            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(path);
            param.mLoadNeedCoroutine = true;
            param.mResNeedCoroutine = true;
            param.mLuaTable = luaTable;
            param.mLuaFunction = luaFunction;
            param.mIsLoadAll = isLoadAll;
            ret = getAndLoad<T>(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);

            return ret;
        }

        public T getAndAsyncLoad<T>(string path, MAction<IDispatchObject> handle, bool isLoadAll = false) where T : InsResBase, new()
        {
            T ret = null;
            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(path);
            param.mLoadNeedCoroutine = true;
            param.mResNeedCoroutine = true;
            param.mLoadEventHandle = handle;
            param.mIsLoadAll = isLoadAll;
            ret = getAndLoad<T>(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);

            return ret;
        }

        public T getAndLoad<T>(LoadParam param) where T : InsResBase, new()
        {
            load<T>(param);
            return getRes(param.mResUniqueId) as T;
        }

        // 同步加载，立马加载完成，并且返回加载的资源， syncLoad 同步加载资源不能喝异步加载资源的接口同时去加载一个资源，如果异步加载一个资源，这个时候资源还没有加载完成，然后又同步加载一个资源，这个时候获取的资源是没有加载完成的，由于同步加载资源没有回调，因此即使同步加载的资源加载完成，也不可能获取加载完成事件
        public void syncLoad<T>(string path, MAction<IDispatchObject> loadEventHandle, bool isLoadAll = false) where T : InsResBase, new()
        {
            LoadParam param;
            param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(path);
            // param.mLoadEventHandle = onLoadEventHandle;        // 这个地方是同步加载，因此不需要回调，如果写了，就会形成死循环， InsResBase 中的 init 又会调用 onLoadEventHandle 这个函数，这个函数是外部回调的函数，由于同步加载，没有回调，因此不要设置这个 param.mLoadEventHandle = onLoadEventHandle ，内部会自动调用
            param.mLoadEventHandle = loadEventHandle;
            param.mLoadNeedCoroutine = false;
            param.mResNeedCoroutine = false;
            param.mIsLoadAll = isLoadAll;
            load<T>(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        public T createResItem<T>(LoadParam param) where T : InsResBase, new()
        {
            T ret = new T();
            ret.refCountResLoadResultNotify.refCount.incRef();
            ret.setLoadParam(param);

            ret.refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, param.mLoadEventHandle, param.mLuaTable, param.mLuaFunction);

            return ret;
        }

        protected void loadWithResCreatedAndLoad(LoadParam param)
        {
            mPath2ResDic[param.mResUniqueId].refCountResLoadResultNotify.refCount.incRef();
            if (mPath2ResDic[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.mLoadEventHandle != null)
                {
                    param.mLoadEventHandle(mPath2ResDic[param.mResUniqueId]);        // 直接通知上层完成加载
                }
                else if(null != param.mLuaTable && null != param.mLuaFunction)
                {
                    param.mLuaFunction.Call(param.mLuaTable, mPath2ResDic[param.mResUniqueId]);
                }
                else if(null != param.mLuaFunction)
                {
                    param.mLuaFunction.Call(mPath2ResDic[param.mResUniqueId]);
                }
            }
            else
            {
                if (param.mLoadEventHandle != null)
                {
                    mPath2ResDic[param.mResUniqueId].refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, param.mLoadEventHandle, param.mLuaTable, param.mLuaFunction);
                }
            }
        }

        protected void loadWithResCreatedAndNotLoad<T>(LoadParam param, T resItem) where T : InsResBase, new()
        {
            mPath2ResDic[param.mResUniqueId] = resItem;
            mPath2ResDic[param.mResUniqueId].refCountResLoadResultNotify.resLoadState.setLoading();
            param.mLoadEventHandle = onLoadEventHandle;
            Ctx.mInstance.mResLoadMgr.loadAsset(param);
        }

        protected void loadWithNotResCreatedAndNotLoad<T>(LoadParam param) where T : InsResBase, new()
        {
            T resItem = createResItem<T>(param);
            loadWithResCreatedAndNotLoad<T>(param, resItem);
        }

        public virtual void load<T>(LoadParam param) where T : InsResBase, new()
        {
            ++mLoadingDepth;

            if (mPath2ResDic.ContainsKey(param.mResUniqueId))
            {
                loadWithResCreatedAndLoad(param);
            }
            else if(param.mLoadInsRes != null)
            {
                loadWithResCreatedAndNotLoad<T>(param, param.mLoadInsRes as T);
            }
            else
            {
                loadWithNotResCreatedAndNotLoad<T>(param);
            }

            --mLoadingDepth;

            if (mLoadingDepth == 0)
            {
                unloadNoRefResFromList();
            }
        }

        virtual public void unload(string resUniqueId, MAction<IDispatchObject> loadEventHandle)
        {
            if (mPath2ResDic.ContainsKey(resUniqueId))
            {
                mPath2ResDic[resUniqueId].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(null, loadEventHandle);
                mPath2ResDic[resUniqueId].refCountResLoadResultNotify.refCount.decRef();
                if (mPath2ResDic[resUniqueId].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    if (mLoadingDepth != 0)       // 如果加载深度不是 0 的，说明正在加载，不能卸载对象
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

        // 添加无引用资源到 List
        protected void addNoRefResID2List(string resUniqueId)
        {
            mZeroRefResIDList.Add(resUniqueId);
        }

        // 卸载没有引用的资源列表中的资源
        protected void unloadNoRefResFromList()
        {
            foreach (string path in mZeroRefResIDList.list())
            {
                if (mPath2ResDic[path].refCountResLoadResultNotify.refCount.isNoRef())
                {
                    unloadNoRef(path);
                }
            }

            mZeroRefResIDList.Clear();
        }

        protected void unloadNoRef(string resUniqueId)
        {
            mPath2ResDic[resUniqueId].unload();
            // 卸载加载的原始资源
            Ctx.mInstance.mResLoadMgr.unload(resUniqueId, onLoadEventHandle);
            mPath2ResDic.Remove(resUniqueId);
            //UtilApi.UnloadUnusedAssets();           // 异步卸载共用资源
        }

        public virtual void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            string path = res.getResUniqueId();

            if (mPath2ResDic.ContainsKey(path))
            {
                mPath2ResDic[path].refCountResLoadResultNotify.resLoadState.copyFrom(res.refCountResLoadResultNotify.resLoadState);
                if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
                {
                    mPath2ResDic[path].init(res);
                    if (mPath2ResDic[path].isOrigResNeedImmeUnload)
                    {
                        // 卸载资源
                        Ctx.mInstance.mResLoadMgr.unload(path, onLoadEventHandle);
                    }
                }
                else
                {
                    mPath2ResDic[path].failed(res);
                    Ctx.mInstance.mResLoadMgr.unload(path, onLoadEventHandle);
                }
            }
            else
            {
                Ctx.mInstance.mResLoadMgr.unload(path, onLoadEventHandle);
            }
        }

        public object getRes(string path)
        {
            return mPath2ResDic[path];
        }

        // 卸载所有的资源
        public void unloadAll()
        {
            // 卸载资源的时候保存的路径列表
            List<string> pathList = new List<string>();
            foreach (KeyValuePair<string, InsResBase> kv in mPath2ResDic)
            {
                kv.Value.refCountResLoadResultNotify.loadResEventDispatch.clearEventHandle();
                pathList.Add(kv.Key);
            }

            foreach(string path in pathList)
            {
                unload(path, onLoadEventHandle);
            }

            pathList.Clear();
        }
    }
}