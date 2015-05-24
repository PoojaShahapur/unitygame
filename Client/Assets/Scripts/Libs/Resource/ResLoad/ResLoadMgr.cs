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

        public ResLoadMgr()
        {
            m_LoadData = new ResLoadData();
            m_id2HandleDic[(int)MsgRouteID.eMRIDLoadedWebRes] = onMsgRouteResLoad;
            m_zeroRefResIDList = new List<string>();
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

        public ResItem loadData(LoadParam param)
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
        public ResItem loadBundle(LoadParam param)
        {
            param.m_resPackType = ResPackType.eBundleType;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;

            return load(param);
        }

        // eLevelType 打包类型资源加载，都用协程加载
        public ResItem loadLevel(LoadParam param)
        {
            param.resolveLevel();

#if PKG_RES_LOAD
            param.m_resPackType = ResPackType.ePakLevelType;
            loadPakRes(param);

            param.m_resPackType = ResPackType.ePakMemLevelType;
            param.m_path = param.m_origPath;        // 恢复加载原始资源
            param.resolvePath();
            return load(param);
#elif UnPKG_RES_LOAD
            param.m_resPackType = ResPackType.eUnPakLevelType;
            param.m_resLoadType = ResLoadType.eStreamingAssets;
            return load(param);
#else
            param.m_resPackType = ResPackType.eLevelType;
            param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
            return load(param);
#endif
        }

        // eResourcesType 打包类型资源加载
        public ResItem loadResources(LoadParam param)
        {
            param.resolvePath();

#if PKG_RES_LOAD
            if (param.m_path.IndexOf(PakSys.PAK_EXT) != -1)     // 如果加载的是打包文件
            {
                param.m_resPackType = ResPackType.ePakType;
                loadPakRes(param);

                param.m_resPackType = ResPackType.ePakMemType;
                param.m_path = param.m_origPath;        // 恢复加载原始资源
                param.resolvePath();
            }
            else        // 加载的是非打包文件
            {
                param.resolvePath();
                param.m_resPackType = ResPackType.eUnPakType;
            }
            return load(param);
#elif UnPKG_RES_LOAD
            // 判断资源所在的目录，是在 StreamingAssets 目录还是在 persistentData 目录下，目前由于没有完成，只能从 StreamingAssets 目录下加载
            param.m_resPackType = ResPackType.eUnPakType;
            param.m_resLoadType = ResLoadType.eStreamingAssets;
            return load(param);
#else
            param.m_resPackType = ResPackType.eResourcesType;
            param.m_resLoadType = ResLoadType.eLoadResource;
            return load(param);
#endif
        }

        // 通用类型，需要自己设置很多参数
        public ResItem load(LoadParam param)
        {
            if (m_LoadData.m_path2Res.ContainsKey(param.m_path))
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
                        m_LoadData.m_path2Res[param.m_path].refCountResLoadResultNotify.loadEventDispatch.addEventHandle(param.m_loadEventHandle);
                    }
                }
            }
            else
            {
                ResItem resitem = findResFormPool(param.m_resPackType);
                if (ResPackType.eLevelType == param.m_resPackType)
                {
                    if (resitem == null)
                    {
                        resitem = new LevelResItem();
                    }
                    (resitem as LevelResItem).levelName = param.lvlName;
                }
                else if (ResPackType.eBundleType == param.m_resPackType)
                {
                    if (resitem == null)
                    {
                        resitem = new BundleResItem();
                    }
                }
                else if (ResPackType.eResourcesType == param.m_resPackType)
                {
                    if (resitem == null)
                    {
                        resitem = new PrefabResItem();
                    }

                    (resitem as PrefabResItem).prefabName = param.prefabName;
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
                        resitem = new ABUnPakComFileResItem();
                    }
                }
                else if (ResPackType.eUnPakLevelType == param.m_resPackType)
                {
                    if (resitem == null)
                    {
                        resitem = new ABUnPakLevelFileResItem();
                    }
                    (resitem as ABUnPakLevelFileResItem).levelName = param.lvlName;
                }
                else if (ResPackType.ePakType == param.m_resPackType)
                {
                    if (resitem == null)
                    {
                        resitem = new ABPakComFileResItem();
                    }
                }
                else if (ResPackType.ePakLevelType == param.m_resPackType)
                {
                    if (resitem == null)
                    {
                        resitem = new ABPakLevelFileResItem();
                    }
                    (resitem as ABPakLevelFileResItem).levelName = param.lvlName;
                    (resitem as ABPakLevelFileResItem).m_origPath = param.m_origPath;
                }
                else if (ResPackType.ePakMemType == param.m_resPackType)
                {
                    if (resitem == null)
                    {
                        resitem = new ABMemUnPakComFileResItem();
                    }
                }
                else if (ResPackType.ePakMemLevelType == param.m_resPackType)
                {
                    if (resitem == null)
                    {
                        resitem = new ABMemUnPakLevelFileResItem();
                    }

                    (resitem as ABMemUnPakLevelFileResItem).levelName = param.lvlName;
                }

                resitem.refCountResLoadResultNotify.refCount.incRef();
                resitem.resNeedCoroutine = param.m_resNeedCoroutine;
                resitem.resPackType = param.m_resPackType;
                resitem.resLoadType = param.m_resLoadType;
                resitem.path = param.m_path;
                resitem.pathNoExt = param.m_pathNoExt;
                resitem.extName = param.extName;

                m_LoadData.m_path2Res[param.m_path] = resitem;

                if (param.m_loadEventHandle != null)
                {
                    m_LoadData.m_path2Res[param.m_path].refCountResLoadResultNotify.loadEventDispatch.addEventHandle(param.m_loadEventHandle);
                }

                // 特殊处理
                if (ResPackType.ePakMemType == param.m_resPackType || ResPackType.ePakMemLevelType == param.m_resPackType)  // 如果是内存加载打包数据，如果包已经加载完成
                {
                    // 这个必须等待参数都设置完成后再调用
                    (resitem as ABMemUnPakFileResItemBase).setPakRes(getResource(param.m_pakPath) as ABPakFileResItemBase);
                }
                else
                {
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

                        (loaditem as LevelLoadItem).levelName = param.lvlName;
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
                            loaditem = new ABUnPakLoadItem();
                        }
                    }
                    else if (ResPackType.ePakType == param.m_resPackType || ResPackType.ePakLevelType == param.m_resPackType)
                    {
                        if (loaditem == null)
                        {
                            loaditem = new ABPakLoadItem();
                        }
                    }

                    loaditem.resPackType = param.m_resPackType;
                    loaditem.resLoadType = param.m_resLoadType;
                    loaditem.path = param.m_path;
                    loaditem.pathNoExt = param.m_pathNoExt;
                    loaditem.extName = param.extName;
                    loaditem.nonRefCountResLoadResultNotify.loadEventDispatch.addEventHandle(onLoadEventHandle);

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
                }
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
                m_LoadData.m_path2Res[path].refCountResLoadResultNotify.refCount.decRef();
                if (m_LoadData.m_path2Res[path].refCountResLoadResultNotify.refCount.refNum == 0)
                {
                    unloadNoRef(path);
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
                if (m_LoadData.m_path2Res[path].refCountResLoadResultNotify.refCount.refNum == 0)
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
            item.nonRefCountResLoadResultNotify.loadEventDispatch.removeEventHandle(onLoadEventHandle);
            if (item.nonRefCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                onLoaded(item);
            }
            else if (item.nonRefCountResLoadResultNotify.resLoadState.hasFailed())
            {
                onFailed(item);
            }
        }

        public void onLoaded(LoadItem item)
        {
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

        protected void loadPakRes(LoadParam param)
        {
            // 打包文件先加载打包，然后单独加载每一个资源
            LoadParam pakParam = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            pakParam.m_resPackType = param.m_resPackType;
            pakParam.m_resLoadType = param.m_resLoadType;
            pakParam.m_path = param.m_path;
            load(pakParam);                     // 确保打包资源必定加载
            Ctx.m_instance.m_poolSys.deleteObj(pakParam);
        }
    }
}