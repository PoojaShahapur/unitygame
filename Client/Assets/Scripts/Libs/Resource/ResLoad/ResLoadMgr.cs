using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using SDK.Common;
using System.IO;

namespace SDK.Lib
{
    public class ResLoadMgr
    {
        protected uint m_maxParral = 8;                             // 最多同时加载的内容
        protected uint m_curNum = 0;                                // 当前加载的数量
        protected ResLoadData m_LoadData;
        protected LoadItem m_retLoadItem;
        protected ResItem m_retResItem;

        public ResLoadMgr()
        {
            m_LoadData = new ResLoadData();
        }

        // 重置加载设置
        protected void resetLoadParam(LoadParam loadParam)
        {
            loadParam.m_loadNeedCoroutine = true;
            loadParam.m_resNeedCoroutine = true;
        }

        public IResItem getResource(string path)
        {
            // 如果 path == null ，程序会宕机
            if (m_LoadData.m_path2Res.ContainsKey(path))
            {
                return m_LoadData.m_path2Res[path];
            }
            else
            {
                return null;
            }
        }

        public IResItem loadData(LoadParam param)
        {
            param.m_resPackType = ResPackType.eDataType;
            
            if (ResLoadType.eStreamingAssets == param.m_resLoadType)
            {
                param.m_path = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalReadDir(), param.m_path);
            }
            else if (ResLoadType.ePersistentData == param.m_resLoadType)
            {
                param.m_path = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), param.m_path);
            }
            else if (ResLoadType.eLoadWeb == param.m_resLoadType)
            {
                param.m_path = Path.Combine(Ctx.m_instance.m_cfg.m_webIP, param.m_path);
            }
            //if (!string.IsNullOrEmpty(param.m_version))
            //{
            //    param.m_path = string.Format("{0}?v={1}", param.m_path, param.m_version);
            //}
            return load(param);
        }

        // eBundleType 打包类型资源加载
        public IResItem loadBundle(LoadParam param)
        {
            param.m_resPackType = ResPackType.eBundleType;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;

            // 资源尽量异步加载
            //param.m_resNeedCoroutine = true;
            //param.m_loadNeedCoroutine = true;

            //param.m_path = Config.StreamingAssets + param.m_path + ".unity3d";

            return load(param);
        }

        // eLevelType 打包类型资源加载，都用协程加载
        public IResItem loadLevel(LoadParam param)
        {
            param.m_resPackType = ResPackType.eLevelType;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;

            return load(param);

            //param.m_resPackType = ResPackType.eUnPakLevelType;
            //param.m_resLoadType = ResLoadType.eStreamingAssets;

            //return load(param);
        }

        // eResourcesType 打包类型资源加载
        public IResItem loadResources(LoadParam param)
        {
            param.m_resPackType = ResPackType.eResourcesType;
            param.m_resLoadType = ResLoadType.eLoadResource;

            return load(param);

            // 判断资源所在的目录，是在 StreamingAssets 目录还是在 persistentData 目录下，目前由于没有完成，只能从 StreamingAssets 目录下加载
            //param.m_resPackType = ResPackType.eUnPakType;
            //param.m_resLoadType = ResLoadType.eStreamingAssets;

            //return load(param);
        }

        // 通用类型，需要自己设置很多参数
        public IResItem load(LoadParam param)
        {
            if (m_LoadData.m_path2Res.ContainsKey(param.m_path))
            {
                resetLoadParam(param);
                m_LoadData.m_path2Res[param.m_path].increaseRef();
                if (param.m_loaded != null)
                {
                    param.m_loaded(m_LoadData.m_path2Res[param.m_path]);
                }
                if (m_LoadData.m_path2Res.ContainsKey(param.m_path))        // 有可能在回调的函数中这个资源被释放了
                {
                    return m_LoadData.m_path2Res[param.m_path];
                }
                else
                {
                    return null;
                }
            }

            ResItem resitem = findResFormPool(param.m_resPackType);
            if (ResPackType.eLevelType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    resitem = new LevelResItem();
                }
                (resitem as LevelResItem).levelName = param.m_lvlName;
            }
            else if (ResPackType.eBundleType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    resitem = new BundleResItem();
                }

                (resitem as BundleResItem).prefabName = param.m_prefabName;
            }
            else if (ResPackType.eResourcesType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    resitem = new PrefabResItem();
                }

                (resitem as PrefabResItem).prefabName = param.m_prefabName;
            }
            else if (ResPackType.eDataType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    resitem = new DataResItem();
                }
            }
            else if (ResPackType.eUnPakType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    resitem = new UnPakFileResItem();
                }

                (resitem as FileResItem).m_extName = param.m_extName;
            }
            else if (ResPackType.eUnPakLevelType == param.m_resPackType)
            {
                if (resitem == null)
                {
                    resitem = new UnPakLevelFileResItem();
                }
                (resitem as UnPakLevelFileResItem).levelName = param.m_lvlName;
                (resitem as UnPakLevelFileResItem).m_extName = param.m_extName;
            }

            resitem.resNeedCoroutine = param.m_resNeedCoroutine;
            resitem.resPackType = param.m_resPackType;
            resitem.resLoadType = param.m_resLoadType;
            resitem.path = param.m_path;

            m_LoadData.m_path2Res[param.m_path] = resitem;

            if (param.m_loaded != null)
            {
                m_LoadData.m_path2Res[param.m_path].addEventListener(EventID.LOADED_EVENT, param.m_loaded);
            }
            if (param.m_failed != null)
            {
                m_LoadData.m_path2Res[param.m_path].addEventListener(EventID.FAILED_EVENT, param.m_failed);
            }

            LoadItem loaditem = findLoadItemFormPool(param.m_resPackType);
            
            if (ResPackType.eResourcesType == param.m_resPackType)        // 默认 Bundle 中资源
            {
                if (loaditem == null)
                {
                    loaditem = new ResourceLoadItem();
                }
            }
            else if (ResPackType.eBundleType == param.m_resPackType)        // Bundle 打包模式
            {
                if (loaditem == null)
                {
                    loaditem = new BundleLoadItem();
                }
            }
            else if (ResPackType.eLevelType == param.m_resPackType)
            {
                if (loaditem == null)
                {
                    loaditem = new LevelLoadItem();
                }

                (loaditem as LevelLoadItem).levelName = param.m_lvlName;
            }
            else if (ResPackType.eDataType == param.m_resPackType)
            {
                if (loaditem == null)
                {
                    loaditem = new DataLoadItem();
                }

                (loaditem as DataLoadItem).m_version = param.m_version;
            }
            else if (ResPackType.eUnPakType == param.m_resPackType || ResPackType.eUnPakLevelType == param.m_resPackType)
            {
                if (loaditem == null)
                {
                    loaditem = new UnPakLoadItem();
                }

                (loaditem as UnPakLoadItem).m_extName = param.m_extName;
            }

            loaditem.resPackType = param.m_resPackType;
            loaditem.resLoadType = param.m_resLoadType;
            loaditem.path = param.m_path;
            loaditem.onLoaded += onLoaded;
            loaditem.onFailed += onFailed;

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

            resetLoadParam(param);

            // 可能同步加载， m_LoadData.m_path2LDItem[param.m_path].load() 就加载完了，直接删除了
            if (m_LoadData.m_path2Res.ContainsKey(param.m_path))
            {
                return m_LoadData.m_path2Res[param.m_path];
            }

            return null;
        }

        // 这个卸载有引用计数，如果有引用计数就卸载不了
        public void unload(string path)
        {
            if (m_LoadData.m_path2Res.ContainsKey(path))
            {
                m_LoadData.m_path2Res[path].decreaseRef();
                if (m_LoadData.m_path2Res[path].refNum == 0)
                {
                    unloadNoRef(path);
                }
            }
        }

        // 不考虑引用计数，直接卸载
        public void unloadNoRef(string path)
        {
            m_LoadData.m_path2Res[path].unload();
            m_LoadData.m_path2Res[path].reset();
            m_LoadData.m_noUsedResItem.Add(m_LoadData.m_path2Res[path]);

            m_LoadData.m_path2Res.Remove(path);
        }

        public void onLoaded(LoadItem item)
        {
            item.onLoaded -= onLoaded;
            if (m_LoadData.m_path2Res.ContainsKey(item.path))
            {
                m_LoadData.m_path2Res[item.path].init(m_LoadData.m_path2LDItem[item.path]);
            }

            releaseLoadItem(item);
            --m_curNum;
            loadNextItem();
        }

        public void onFailed(LoadItem item)
        {
            string path = item.path;
            item.onFailed -= onFailed;
            if (m_LoadData.m_path2Res.ContainsKey(path))
            {
                m_LoadData.m_path2Res[path].failed(m_LoadData.m_path2LDItem[path]);
            }

            releaseLoadItem(item);
            --m_curNum;
            loadNextItem();
        }

        protected void releaseLoadItem(LoadItem item)
        {
            item.reset();
            m_LoadData.m_noUsedLDItem.Add(item);
            m_LoadData.m_path2LDItem.Remove(item.path);
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