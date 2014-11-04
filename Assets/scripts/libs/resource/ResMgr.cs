﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class ResMgr : IResMgr
    {
        protected uint m_maxParral = 8;                             // 最多同时加载的内容
        protected uint m_curNum = 0;                                // 当前加载的数量
        protected LoadParam m_loadParam;
        protected ResLoadData m_LoadData;
        //protected AsyncLoadTask m_AsyncLoadTask = new AsyncLoadTask();                    // 异步加载任务

        //protected bool m_IsSyncLoad = true;                         // 是否是同步加载

        public ResMgr()
        {
            m_loadParam = new LoadParam();
            m_LoadData = new ResLoadData();

            //m_AsyncLoadTask.AsyncResLoadData = m_AsyncLoadData;
            //m_AsyncLoadTask.start();
        }

        public LoadParam loadParam
        {
            get
            {
                return m_loadParam;
            }
        }

        public IRes getResource(string path)
        {
            if (m_LoadData.m_path2Res.ContainsKey(path))
            {
                return m_LoadData.m_path2Res[path];
            }
            else
            {
                return null;
            }
        }

        public IRes load(LoadParam param)
        {
            //if(m_IsSyncLoad)
            //{
                return Syncload(param);
            //}
            //else
            //{
            //    AsyncLoad(param);
            //}

            //return null;
        }

        public IRes Syncload(LoadParam param)
        {
            if (m_LoadData.m_path2Res.ContainsKey(param.m_path))
            {
                //if (param.m_cb != null)
                if (param.m_loadedcb != null)
                {
                    Ctx.m_instance.m_shareMgr.m_evt.m_param = m_LoadData.m_path2Res[param.m_path];
                    param.m_loadedcb(Ctx.m_instance.m_shareMgr.m_evt);
                }
                return m_LoadData.m_path2Res[param.m_path];
            }

            Res resitem = findResFormPool(param.m_resPackType, param.m_resNeedCoroutine);
            if (ResPackType.eLevelType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    m_LoadData.m_path2Res[param.m_path] = new LevelRes();
                }
                else
                {
                    m_LoadData.m_path2Res[param.m_path] = resitem;
                }
            }
            else if (ResPackType.eBundleType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    m_LoadData.m_path2Res[param.m_path] = new BundleRes();
                }
                else
                {
                    m_LoadData.m_path2Res[param.m_path] = resitem;
                }

                (m_LoadData.m_path2Res[param.m_path] as BundleRes).prefabName = param.m_prefabName;
            }
            else if (ResPackType.eResourcesType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    m_LoadData.m_path2Res[param.m_path] = new PrefabRes();
                }
                else
                {
                    m_LoadData.m_path2Res[param.m_path] = resitem;
                }
            }

            m_LoadData.m_path2Res[param.m_path].resNeedCoroutine = param.m_resNeedCoroutine;
            m_LoadData.m_path2Res[param.m_path].resPackType = param.m_resPackType;
            m_LoadData.m_path2Res[param.m_path].path = param.m_path;
            m_LoadData.m_path2Res[param.m_path].resLoadType = param.m_resLoadType;

            if (param.m_loadedcb != null)
            {
                m_LoadData.m_path2Res[param.m_path].addEventListener(EventID.LOADED_EVENT, param.m_loadedcb);
            }

            LoadItem loaditem = findLoadItemFormPool(param.m_resPackType, param.m_resNeedCoroutine);
            if (loaditem == null)
            {
                if (ResPackType.eResourcesType == param.m_resPackType)        // 默认 Bundle 中资源
                {
                    loaditem = new ResourceLoadItem();
                }
                else if (ResPackType.eBundleType == param.m_resPackType)        // Bundle 打包模式
                {
                    //if (ResLoadType.eLoadResource == param.m_resLoadType)            // 如果是从默认的 Bundle 中加载
                    //{
                    //    loaditem = new ResourceLoadItem();
                    //}
                    //else //if (ResLoadType.eLoadDisc == param.m_resLoadType)
                    //{
                        loaditem = new BundleLoadItem();
                    //}
                }
                else if (ResPackType.eLevelType == param.m_resPackType)
                {
                    loaditem = new LevelLoadItem();
                }
            }

            loaditem.resPackType = param.m_resPackType;
            loaditem.resLoadType = param.m_resLoadType;
            loaditem.path = param.m_path;
            loaditem.onLoaded += onLoad;
            if (ResPackType.eLevelType == param.m_resPackType)
            {
                (loaditem as LevelLoadItem).levelName = param.m_lvlName;
            }

            if (m_curNum < m_maxParral)
            {
                m_LoadData.m_path2LDItem[param.m_path] = loaditem;
                m_LoadData.m_path2LDItem[param.m_path].load();
                ++m_curNum;
            }
            else
            {
                m_LoadData.m_willLDItem.Add(loaditem);
            }

            return m_LoadData.m_path2Res[param.m_path];
        }

        public void unload(string path)
        {
            if (m_LoadData.m_path2Res.ContainsKey(path))
            {
                //if (m_LoadData.m_path2Res[path].resNeedCoroutine)
                //{
                    //GameObject.Destroy(m_path2Res[path]); // 不再从 gameobject 上移除了
                //}

                m_LoadData.m_path2Res[path].unload();
                m_LoadData.m_path2Res[path].reset();
                m_LoadData.m_noUsedLDItem.Add(m_LoadData.m_path2Res[path]);

                m_LoadData.m_path2Res.Remove(path);
            }
        }

        public void onLoad(LoadItem item)
        {
            string path = item.path;
            item.onLoaded -= onLoad;
            if (m_LoadData.m_path2Res[path] != null)
            {
                m_LoadData.m_path2Res[path].init(m_LoadData.m_path2LDItem[path]);
            }

            //if (item.loadNeedCoroutine)
            //{
                //GameObject.Destroy(item); // 不再从 gameobject 上移除了
            //}

            item.reset();
            m_LoadData.m_noUsedLDItem.Add(item);
            m_LoadData.m_path2LDItem.Remove(path);

            --m_curNum;
            loadNextItem();
        }

        protected void loadNextItem()
        {
            if (m_curNum < m_maxParral)
            {
                if (m_LoadData.m_willLDItem.Count > 0)
                {
                    m_LoadData.m_path2LDItem[(m_LoadData.m_willLDItem[0] as LoadItem).path] = m_LoadData.m_willLDItem[0] as LoadItem;
                    m_LoadData.m_willLDItem.RemoveAt(0);

                    ++m_curNum;
                }
            }
        }

        protected Res findResFormPool(ResPackType type, bool resNeedCoroutine)
        {
            //foreach (Res item in m_LoadData.m_noUsedResItem)
            //{
            //    if(item.type == type && item.resNeedCoroutine == resNeedCoroutine)
            //    {
            //        return item;
            //    }
            //}

            return null;
        }

        protected LoadItem findLoadItemFormPool(ResPackType type, bool loadNeedCoroutine)
        {
            //foreach (LoadItem item in m_LoadData.m_noUsedLDItem)
            //{
            //    if (item.type == type && item.loadNeedCoroutine == loadNeedCoroutine)
            //    {
            //        return item;
            //    }
            //}

            return null;
        }

        public LoadParam getLoadParam()
        {
            return m_loadParam;
        }

        //#region AsyncLoad

        //protected AsyncResLoadData m_AsyncLoadData = new AsyncResLoadData();

        // 线程异步加载
        //public IRes AsyncLoad(LoadParam param)
        //{
        //    if (m_AsyncLoadData.m_path2Res.ContainsKey(param.m_path))
        //    {
        //        if (param.m_loadedcb != null)
        //        {
        //            Ctx.m_instance.m_shareMgr.m_evt.m_param = m_AsyncLoadData.m_path2Res[param.m_path];
        //            param.m_loadedcb(Ctx.m_instance.m_shareMgr.m_evt);
        //        }
        //        return m_AsyncLoadData.m_path2Res[param.m_path];
        //    }

        //    AsyncRes resitem = null;
        //    if (param.m_type == ResPackType.eLevelType)
        //    {
        //        if (resitem == null)
        //        {
        //            m_AsyncLoadData.m_path2Res[param.m_path] = new AsyncLevelRes();
        //        }
        //        else
        //        {
        //            m_AsyncLoadData.m_path2Res[param.m_path] = resitem;
        //        }
        //    }
        //    else if (param.m_type == ResPackType.eBundleType)
        //    {
        //        if (resitem == null)
        //        {
        //            m_AsyncLoadData.m_path2Res[param.m_path] = new AsyncBundleRes();
        //        }
        //        else
        //        {
        //            m_AsyncLoadData.m_path2Res[param.m_path] = resitem;
        //        }

        //        (m_AsyncLoadData.m_path2Res[param.m_path] as AsyncBundleRes).prefabName = param.m_prefabName;
        //    }

        //    m_AsyncLoadData.m_path2Res[param.m_path].type = param.m_type;
        //    m_AsyncLoadData.m_path2Res[param.m_path].path = param.m_path;
        //    m_AsyncLoadData.m_path2Res[param.m_path].resLoadType = param.m_resLoadType;

        //    if (param.m_loadedcb != null)
        //    {
        //        m_AsyncLoadData.m_path2Res[param.m_path].addEventListener(EventID.LOADED_EVENT, param.m_loadedcb);
        //    }

        //    AsyncLoadItem loaditem = null;
        //    if (loaditem == null)
        //    {
        //        if (ResPackType.eBundleType == param.m_type)        // Bundle 打包模式
        //        {
        //            if (ResLoadType.eLoadDisc == param.m_resLoadType)
        //            {
        //                loaditem = new AsyncBundleLoadItem();
        //            }
        //        }
        //        else if (ResPackType.eLevelType == param.m_type)
        //        {
        //            loaditem = new AsyncLevelLoadItem();
        //        }
        //    }

        //    loaditem.type = param.m_type;
        //    loaditem.resLoadType = param.m_resLoadType;
        //    loaditem.path = param.m_path;
        //    loaditem.onLoaded += onAsyncLoad;
        //    if (ResPackType.eLevelType == param.m_type)
        //    {
        //        (loaditem as AsyncLevelLoadItem).levelName = param.m_lvlName;
        //    }

        //    if (m_curNum < m_maxParral)
        //    {
        //        m_AsyncLoadData.m_path2LDItem[param.m_path] = loaditem;
        //        // 线程中加载
        //        //m_AsyncLoadData.m_path2LDItem[param.m_path].load();
        //        ++m_curNum;
        //    }
        //    else
        //    {
        //        m_AsyncLoadData.m_willLDItem.Add(loaditem);
        //    }

        //    return m_AsyncLoadData.m_path2Res[param.m_path];
        //}

        //public void onAsyncLoad(LoadItem item)
        //{
        //    string path = item.path;
        //    item.onLoaded -= onAsyncLoad;
        //    if (m_AsyncLoadData.m_path2Res[path] != null)
        //    {
        //        m_AsyncLoadData.m_path2Res[path].init(m_AsyncLoadData.m_path2LDItem[path]);
        //    }

        //    item.reset();
        //    m_AsyncLoadData.m_noUsedLDItem.Add(item);
        //    m_AsyncLoadData.m_path2LDItem.Remove(path);

        //    --m_curNum;
        //    AsyncLoadNextItem();
        //}

        //protected void AsyncLoadNextItem()
        //{
        //    if (m_curNum < m_maxParral)
        //    {
        //        if (m_AsyncLoadData.m_willLDItem.Count > 0)
        //        {
        //            m_LoadData.m_path2LDItem[(m_AsyncLoadData.m_willLDItem[0] as LoadItem).path] = m_AsyncLoadData.m_willLDItem[0] as LoadItem;
        //            m_LoadData.m_willLDItem.RemoveAt(0);

        //            ++m_curNum;
        //        }
        //    }
        //}

        //#endregion

        //public void OnTick(float delta)
        //{
        //    foreach (AsyncLoadItem loadItem in m_AsyncLoadData.m_path2LDItem.Values)
        //    {
        //        if (loadItem.ResLoadState == ResLoadState.eLoaded)  // 加载成功
        //        {
        //            onAsyncLoad(loadItem);
        //        }
        //        else if (loadItem.ResLoadState == ResLoadState.eFailed) // 加载失败
        //        {
        //            onAsyncLoad(loadItem);
        //        }
        //    }
        //}
    }
}