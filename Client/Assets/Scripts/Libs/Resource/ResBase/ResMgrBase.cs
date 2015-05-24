﻿using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 资源管理器，不包括资源加载
     */
    public class ResMgrBase
    {
        public Dictionary<string, InsResBase> m_path2ResDic = new Dictionary<string, InsResBase>();

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

        public virtual InsResBase load<T>(LoadParam param) where T : InsResBase, new()
        {
            if (m_path2ResDic.ContainsKey(param.m_path))
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
            else
            {
                m_path2ResDic[param.m_path] = new T();
                m_path2ResDic[param.m_path].refCountResLoadResultNotify.refCount.incRef();
                m_path2ResDic[param.m_path].m_path = param.m_path;

                m_path2ResDic[param.m_path].refCountResLoadResultNotify.loadEventDispatch.addEventHandle(param.m_loadEventHandle);

                param.m_loadEventHandle = onLoadEventHandle;
                Ctx.m_instance.m_resLoadMgr.loadResources(param);
            }

            return m_path2ResDic[param.m_path];
        }

        virtual public void unload(string path, Action<IDispatchObject> loadEventHandle)
        {
            if (m_path2ResDic.ContainsKey(path))
            {
                m_path2ResDic[path].refCountResLoadResultNotify.loadEventDispatch.removeEventHandle(loadEventHandle);
                m_path2ResDic[path].refCountResLoadResultNotify.refCount.decRef();
                if (m_path2ResDic[path].refCountResLoadResultNotify.refCount.refNum == 0)
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
            }
        }

        public virtual void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            string path = res.GetPath();
            bool bOrigResNeedImmeUnload = true;

            if (m_path2ResDic.ContainsKey(path))
            {
                bOrigResNeedImmeUnload = m_path2ResDic[path].bOrigResNeedImmeUnload;
                m_path2ResDic[path].refCountResLoadResultNotify.resLoadState.copyFrom(res.refCountResLoadResultNotify.resLoadState);
                m_path2ResDic[path].refCountResLoadResultNotify.onLoadEventHandle(m_path2ResDic[path]);
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("路径不能查找到 {0}", path));
            }

            if (bOrigResNeedImmeUnload)
            {
                // 卸载资源
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