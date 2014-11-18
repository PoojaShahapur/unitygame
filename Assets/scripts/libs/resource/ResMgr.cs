using UnityEngine;
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

        public ResMgr()
        {
            m_loadParam = new LoadParam();
            m_LoadData = new ResLoadData();
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

        // eBundleType 打包类型资源加载
        public IRes loadBundle(LoadParam param)
        {
            param.m_resPackType = ResPackType.eBundleType;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;

            // 资源尽量异步加载
            param.m_resNeedCoroutine = true;
            param.m_loadNeedCoroutine = true;

            param.m_path = Config.StreamingAssets + param.m_path + ".unity3d";

            return load(param);
        }

        // eLevelType 打包类型资源加载
        public IRes loadLevel(LoadParam param)
        {
            param.m_resPackType = ResPackType.eLevelType;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            param.m_resNeedCoroutine = true;
            param.m_loadNeedCoroutine = true;

            param.m_path = Config.StreamingAssets + param.m_path + ".unity3d";

            return load(param);
        }

        // eResourcesType 打包类型资源加载
        public IRes loadResources(LoadParam param)
        {
            param.m_resPackType = ResPackType.eResourcesType;
            param.m_resLoadType = ResLoadType.eLoadResource;

            // 资源尽量异步加载
            param.m_resNeedCoroutine = true;
            param.m_loadNeedCoroutine = true;

            return load(param);
        }

        // 通用类型，需要自己设置很多参数
        public IRes load(LoadParam param)
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

            // 可能同步加载， m_LoadData.m_path2LDItem[param.m_path].load() 就加载完了，直接删除了
            if (m_LoadData.m_path2Res.ContainsKey(param.m_path))
            {
                return m_LoadData.m_path2Res[param.m_path];
            }

            return null;
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
    }
}