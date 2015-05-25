using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using SDK.Common;
using System.IO;

namespace SDK.Lib
{
    public class ResLoadMgr : MsgRouteHandleBase
    {
        protected uint m_maxParral = 8;                             // 最多同时加载的内容
        protected uint m_curNum = 0;                                // 当前加载的数量
        protected ResLoadData m_LoadData;
        protected LoadItem m_retLoadItem;
        protected ResItem m_retResItem;
        protected ResMsgRouteCB m_resMsgRouteCB;
        protected List<string> m_zeroRefResIDList;      // 没有引用的资源 ID 列表
        protected bool m_bLoading;      // 是否正在加载中

        public ResLoadMgr()
        {
            m_LoadData = new ResLoadData();
            m_id2HandleDic[(int)MsgRouteID.eMRIDLoadedWebRes] = onMsgRouteResLoad;
            m_zeroRefResIDList = new List<string>();
            m_bLoading = false;
        }

        public void postInit()
        {
            // 游戏逻辑处理
            m_resMsgRouteCB = new ResMsgRouteCB();
            Ctx.m_instance.m_msgRouteList.addOneDisp(m_resMsgRouteCB);
        }

        // 重置加载设置
        protected void resetLoadParam(LoadParam loadParam)
        {
            loadParam.m_loadNeedCoroutine = true;
            loadParam.m_resNeedCoroutine = true;
        }

        public ResItem getResource(string path)
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

        public void loadData(LoadParam param)
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
            load(param);
        }

        // eBundleType 打包类型资源加载
        public void loadBundle(LoadParam param)
        {
            param.m_resPackType = ResPackType.eBundleType;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;

            load(param);
        }

        // eLevelType 打包类型资源加载，都用协程加载
        public void loadLevel(LoadParam param)
        {
            param.resolveLevel();

#if PKG_RES_LOAD
            param.m_resPackType = ResPackType.ePakLevelType;
            param.resolvePath();
            load(param);
#elif UnPKG_RES_LOAD
            param.m_resPackType = ResPackType.eUnPakLevelType;
            param.m_resLoadType = ResLoadType.eStreamingAssets;
            load(param);
#else
            param.m_resPackType = ResPackType.eLevelType;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            load(param);
#endif
        }

        // eResourcesType 打包类型资源加载
        public void loadResources(LoadParam param)
        {
            param.resolvePath();

#if PKG_RES_LOAD
            if (param.m_path.IndexOf(PakSys.PAK_EXT) != -1)     // 如果加载的是打包文件
            {
                param.m_resPackType = ResPackType.ePakType;
            }
            else        // 加载的是非打包文件
            {
                param.m_resPackType = ResPackType.eUnPakType;
            }
            load(param);
#elif UnPKG_RES_LOAD
            // 判断资源所在的目录，是在 StreamingAssets 目录还是在 persistentData 目录下，目前由于没有完成，只能从 StreamingAssets 目录下加载
            param.m_resPackType = ResPackType.eUnPakType;
            param.m_resLoadType = ResLoadType.eStreamingAssets;
            return load(param);
#else
            param.m_resPackType = ResPackType.eResourcesType;
            param.m_resLoadType = ResLoadType.eLoadResource;
            load(param);
#endif
        }

        public ResItem createResItem(LoadParam param)
        {
            ResItem resItem = findResFormPool(param.m_resPackType);
            if (ResPackType.eLevelType == param.m_resPackType)
            {
                if (resItem == null)
                {
                    resItem = new LevelResItem();
                }
                (resItem as LevelResItem).levelName = param.lvlName;
            }
            else if (ResPackType.eBundleType == param.m_resPackType)
            {
                if (resItem == null)
                {
                    resItem = new BundleResItem();
                }
            }
            else if (ResPackType.eResourcesType == param.m_resPackType)
            {
                if (resItem == null)
                {
                    resItem = new PrefabResItem();
                }

                (resItem as PrefabResItem).prefabName = param.prefabName;
            }
            else if (ResPackType.eDataType == param.m_resPackType)
            {
                if (resItem == null)
                {
                    resItem = new DataResItem();
                }
            }
            else if (ResPackType.eUnPakType == param.m_resPackType)
            {
                if (resItem == null)
                {
                    resItem = new ABUnPakComFileResItem();
                }
            }
            else if (ResPackType.eUnPakLevelType == param.m_resPackType)
            {
                if (resItem == null)
                {
                    resItem = new ABUnPakLevelFileResItem();
                }
                (resItem as ABUnPakLevelFileResItem).levelName = param.lvlName;
            }
            else if (ResPackType.ePakType == param.m_resPackType)
            {
                if (resItem == null)
                {
                    resItem = new ABPakComFileResItem();
                }
            }
            else if (ResPackType.ePakLevelType == param.m_resPackType)
            {
                if (resItem == null)
                {
                    resItem = new ABPakLevelFileResItem();
                }
                (resItem as ABPakLevelFileResItem).levelName = param.lvlName;
                (resItem as ABPakLevelFileResItem).m_origPath = param.m_origPath;
            }
            //else if (ResPackType.ePakMemType == param.m_resPackType)
            //{
            //    if (resitem == null)
            //    {
            //        resitem = new ABMemUnPakComFileResItem();
            //    }
            //}
            //else if (ResPackType.ePakMemLevelType == param.m_resPackType)
            //{
            //    if (resitem == null)
            //    {
            //        resitem = new ABMemUnPakLevelFileResItem();
            //    }

            //    (resitem as ABMemUnPakLevelFileResItem).levelName = param.lvlName;
            //}

            resItem.refCountResLoadResultNotify.refCount.incRef();
            resItem.resNeedCoroutine = param.m_resNeedCoroutine;
            resItem.resPackType = param.m_resPackType;
            resItem.resLoadType = param.m_resLoadType;
            resItem.path = param.m_path;
            resItem.pathNoExt = param.m_pathNoExt;
            resItem.extName = param.extName;

            if (param.m_loadEventHandle != null)
            {
                resItem.refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(param.m_loadEventHandle);
            }

            return resItem;
        }

        protected LoadItem createLoadItem(LoadParam param)
        {
            LoadItem loadItem = findLoadItemFormPool(param.m_resPackType);

            if (ResPackType.eResourcesType == param.m_resPackType)        // 默认 Bundle 中资源
            {
                if (loadItem == null)
                {
                    loadItem = new ResourceLoadItem();
                }
            }
            else if (ResPackType.eBundleType == param.m_resPackType)        // Bundle 打包模式
            {
                if (loadItem == null)
                {
                    loadItem = new BundleLoadItem();
                }
            }
            else if (ResPackType.eLevelType == param.m_resPackType)
            {
                if (loadItem == null)
                {
                    loadItem = new LevelLoadItem();
                }

                (loadItem as LevelLoadItem).levelName = param.lvlName;
            }
            else if (ResPackType.eDataType == param.m_resPackType)
            {
                if (loadItem == null)
                {
                    loadItem = new DataLoadItem();
                }

                (loadItem as DataLoadItem).m_version = param.m_version;
            }
            else if (ResPackType.eUnPakType == param.m_resPackType || ResPackType.eUnPakLevelType == param.m_resPackType)
            {
                if (loadItem == null)
                {
                    loadItem = new ABUnPakLoadItem();
                }
            }
            else if (ResPackType.ePakType == param.m_resPackType || ResPackType.ePakLevelType == param.m_resPackType)
            {
                if (loadItem == null)
                {
                    loadItem = new ABPakLoadItem();
                }
            }

            loadItem.resPackType = param.m_resPackType;
            loadItem.resLoadType = param.m_resLoadType;
            loadItem.path = param.m_path;
            loadItem.pathNoExt = param.m_pathNoExt;
            loadItem.extName = param.extName;
            loadItem.loadNeedCoroutine = param.m_loadNeedCoroutine;
            loadItem.nonRefCountResLoadResultNotify.loadResEventDispatch.addEventHandle(onLoadEventHandle);

            return loadItem;
        }

        // 资源创建并且正在被加载
        protected void loadWithResCreatedAndLoad(LoadParam param)
        {
            m_LoadData.m_path2Res[param.m_path].refCountResLoadResultNotify.refCount.incRef();
            if (m_LoadData.m_path2Res[param.m_path].refCountResLoadResultNotify.resLoadState.hasLoaded())
            {
                if (param.m_loadEventHandle != null)
                {
                    param.m_loadEventHandle(m_LoadData.m_path2Res[param.m_path]);
                }
            }
            else
            {
                if (param.m_loadEventHandle != null)
                {
                    m_LoadData.m_path2Res[param.m_path].refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(param.m_loadEventHandle);
                }
            }

            resetLoadParam(param);
        }

        protected void loadWithResCreatedAndNotLoad(LoadParam param, ResItem resItem)
        {
            m_LoadData.m_path2Res[param.m_path] = resItem;
            m_LoadData.m_path2Res[param.m_path].refCountResLoadResultNotify.resLoadState.setLoading();
            LoadItem loadItem = createLoadItem(param);

            if (m_curNum < m_maxParral)
            {
                m_LoadData.m_path2LDItem[param.m_path] = loadItem;
                m_LoadData.m_path2LDItem[param.m_path].load();
                ++m_curNum;
            }
            else
            {
                m_LoadData.m_willLDItem.Add(loadItem);
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
            m_bLoading = true;
            if (m_LoadData.m_path2Res.ContainsKey(param.m_path))
            {
                loadWithResCreatedAndLoad(param);
            }
            else if(param.m_loadRes != null)
            {
                loadWithResCreatedAndNotLoad(param, m_LoadData.m_path2Res[param.m_path]);
            }
            else
            {
                loadWithNotResCreatedAndNotLoad(param);
            }
            m_bLoading = false;

            unloadNoRefResFromList();
        }

        public ResItem getAndLoad(LoadParam param)
        {
            load(param);
            return getResource(param.m_path);
        }

        // 这个卸载有引用计数，如果有引用计数就卸载不了
        public void unload(string path, Action<IDispatchObject> loadEventHandle)
        {
            if (m_LoadData.m_path2Res.ContainsKey(path))
            {
                m_LoadData.m_path2Res[path].refCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(loadEventHandle);
                m_LoadData.m_path2Res[path].refCountResLoadResultNotify.refCount.decRef();
                if (m_LoadData.m_path2Res[path].refCountResLoadResultNotify.refCount.bNoRef())
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
            foreach(string path in m_zeroRefResIDList)
            {
                if (m_LoadData.m_path2Res[path].refCountResLoadResultNotify.refCount.bNoRef())
                {
                    unloadNoRef(path);
                }
            }
            m_zeroRefResIDList.Clear();
        }

        // 不考虑引用计数，直接卸载
        protected void unloadNoRef(string path)
        {
            if (m_LoadData.m_path2Res.ContainsKey(path))
            {
                m_LoadData.m_path2Res[path].unload();
                m_LoadData.m_path2Res[path].reset();
                m_LoadData.m_noUsedResItem.Add(m_LoadData.m_path2Res[path]);

                m_LoadData.m_path2Res.Remove(path);
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("路径不能查找到 {0}", path));
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            LoadItem item = dispObj as LoadItem;
            item.nonRefCountResLoadResultNotify.loadResEventDispatch.removeEventHandle(onLoadEventHandle);
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
            if (m_LoadData.m_path2Res.ContainsKey(item.path))
            {
                m_LoadData.m_path2Res[item.path].init(m_LoadData.m_path2LDItem[item.path]);
            }
            else        // 如果资源已经没有使用的地方了
            {
                item.unload();          // 直接卸载掉
            }
        }

        public void onFailed(LoadItem item)
        {
            string path = item.path;
            if (m_LoadData.m_path2Res.ContainsKey(path))
            {
                m_LoadData.m_path2Res[path].failed(m_LoadData.m_path2LDItem[path]);
            }
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
                    string path = (m_LoadData.m_willLDItem[0] as LoadItem).path;
                    m_LoadData.m_path2LDItem[path] = m_LoadData.m_willLDItem[0] as LoadItem;
                    m_LoadData.m_willLDItem.RemoveAt(0);
                    m_LoadData.m_path2LDItem[path].load();

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

        // 资源加载完成，触发下一次加载
        protected void onMsgRouteResLoad(MsgRouteBase msg)
        {
            DataLoadItem loadItem = (msg as LoadedWebResMR).m_task as DataLoadItem;
            loadItem.handleResult();
        }
    }
}