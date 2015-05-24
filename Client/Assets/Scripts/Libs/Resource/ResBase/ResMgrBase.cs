using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 资源管理器，不包括资源加载
     */
    public class ResMgrBase
    {
        public Dictionary<string, InsResBase> m_path2ResDic;
        protected List<string> m_zeroRefResIDList;      // 没有引用的资源 ID 列表
        protected bool m_bLoading;      // 是否正在加载中

        public ResMgrBase()
        {
            m_path2ResDic = new Dictionary<string, InsResBase>();
            m_zeroRefResIDList = new List<string>();
            m_bLoading = false;
        }

        // 同步加载，立马加载完成，并且返回加载的资源
        public T syncGet<T>(string path) where T : InsResBase, new()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = path;
            //param.m_loadEventHandle = onLoadEventHandle;        // 这个地方是同步加载，因此不需要回调，如果写了，就会形成死循环
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            load<T>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
            return m_path2ResDic[path] as T;
        }

        public T createResItem<T>(LoadParam param) where T : InsResBase, new()
        {
            m_path2ResDic[param.m_path] = new T();
            m_path2ResDic[param.m_path].refCountResLoadResultNotify.refCount.incRef();
            m_path2ResDic[param.m_path].m_path = param.m_path;

            m_path2ResDic[param.m_path].refCountResLoadResultNotify.loadEventDispatch.addEventHandle(param.m_loadEventHandle);

            return m_path2ResDic[param.m_path] as T;
        }

        protected void loadWithResCreatedAndLoad<T>(LoadParam param, T resItem) where T : InsResBase, new()
        {
            m_path2ResDic[param.m_path].refCountResLoadResultNotify.refCount.incRef();
            if (m_path2ResDic[param.m_path].refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.m_loadEventHandle != null)
                {
                    param.m_loadEventHandle(m_path2ResDic[param.m_path]);        // 直接通知上层完成加载
                }
            }
            else
            {
                if (param.m_loadEventHandle != null)
                {
                    m_path2ResDic[param.m_path].refCountResLoadResultNotify.loadEventDispatch.addEventHandle(param.m_loadEventHandle);
                }
            }
        }

        protected void loadWithResCreatedAndNotLoad<T>(LoadParam param, T resItem) where T : InsResBase, new()
        {
            param.m_loadEventHandle = onLoadEventHandle;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
        }

        protected void loadWithNotResCreatedAndNotLoad<T>(LoadParam param) where T : InsResBase, new()
        {
            createResItem<T>(param);
            loadWithResCreatedAndNotLoad<T>(param, m_path2ResDic[param.m_path] as T);
        }

        public virtual void load<T>(LoadParam param) where T : InsResBase, new()
        {
            m_bLoading = true;
            if (m_path2ResDic.ContainsKey(param.m_path))
            {
                loadWithResCreatedAndLoad(param, m_path2ResDic[param.m_path] as T);
            }
            else if(param.m_loadInsRes != null)
            {
                loadWithResCreatedAndNotLoad<T>(param, m_path2ResDic[param.m_path] as T);
            }
            else
            {
                loadWithNotResCreatedAndNotLoad<T>(param);
            }
            m_bLoading = false;
        }

        virtual public void unload(string path, Action<IDispatchObject> loadEventHandle)
        {
            if (m_path2ResDic.ContainsKey(path))
            {
                m_path2ResDic[path].refCountResLoadResultNotify.loadEventDispatch.removeEventHandle(loadEventHandle);
                m_path2ResDic[path].refCountResLoadResultNotify.refCount.decRef();
                if (m_path2ResDic[path].refCountResLoadResultNotify.refCount.refNum == 0)
                {
                    if (m_bLoading)
                    {
                        addNoRefResID2List(path);
                    }
                    else
                    {
                        unloadNoRef(path);
                    }
                }
            }
        }

        // 添加无引用资源到 List
        protected void addNoRefResID2List(string path)
        {
            m_zeroRefResIDList.Add(path);
        }

        // 卸载没有引用的资源列表中的资源
        protected void unloadNoRefResFromList()
        {
            foreach (string path in m_zeroRefResIDList)
            {
                if (m_path2ResDic[path].refCountResLoadResultNotify.refCount.refNum == 0)
                {
                    unloadNoRef(path);
                }
            }
            m_zeroRefResIDList.Clear();
        }

        protected void unloadNoRef(string path)
        {
            m_path2ResDic[path].unload();
            m_path2ResDic.Remove(path);

            // 卸载加载的原始资源
            if (!m_path2ResDic[path].bOrigResNeedImmeUnload)
            {
                Ctx.m_instance.m_resLoadMgr.unload(path, onLoadEventHandle);
            }
            UtilApi.UnloadUnusedAssets();           // 异步卸载共用资源
        }

        public virtual void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            string path = res.GetPath();

            if (m_path2ResDic.ContainsKey(path))
            {
                m_path2ResDic[path].refCountResLoadResultNotify.resLoadState.copyFrom(res.refCountResLoadResultNotify.resLoadState);
                if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
                {
                    m_path2ResDic[path].init(res);
                    if (m_path2ResDic[path].bOrigResNeedImmeUnload)
                    {
                        // 卸载资源
                        Ctx.m_instance.m_resLoadMgr.unload(path, onLoadEventHandle);
                    }
                }
                else
                {
                    m_path2ResDic[path].failed(res);
                    Ctx.m_instance.m_resLoadMgr.unload(path, onLoadEventHandle);
                }
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("路径不能查找到 {0}", path));
                Ctx.m_instance.m_resLoadMgr.unload(path, onLoadEventHandle);
            }
        }

        public object getRes(string path)
        {
            return m_path2ResDic[path];
        }

        // 卸载所有的资源
        public void unloadAll()
        {
            // 卸载资源的时候保存的路径列表
            List<string> pathList = new List<string>();
            foreach (KeyValuePair<string, InsResBase> kv in m_path2ResDic)
            {
                kv.Value.refCountResLoadResultNotify.loadEventDispatch.clearEventHandle();
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