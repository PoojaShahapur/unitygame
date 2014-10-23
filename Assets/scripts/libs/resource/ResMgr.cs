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
        // 因为资源有些需要协同程序，因此重复利用资源
        protected Dictionary<string, LoadItem> m_path2LDItem;       // 正在加载的内容 loaditem
        protected ArrayList m_willLDItem;                           // 将要加载的 loaditem
        protected ArrayList m_noUsedLDItem;                         // 没有被使用的 loaditem
        protected Dictionary<string, Res> m_path2Res;
        protected ArrayList m_noUsedResItem;                         // 没有被使用的 Res

        public ResMgr()
        {
            m_path2LDItem = new Dictionary<string, LoadItem>();
            m_path2Res = new Dictionary<string, Res>();
            m_loadParam = new LoadParam();
            m_willLDItem = new ArrayList();
            m_noUsedLDItem = new ArrayList();
            m_noUsedResItem = new ArrayList();
        }

        public LoadParam loadParam
        {
            get
            {
                return m_loadParam;
            }
        }

        public IRes load(LoadParam param)
        {
            if (m_path2Res.ContainsKey(param.m_path))
            {
                if (param.m_cb != null)
                {
                    param.m_cb(m_path2Res[param.m_path]);
                }
                return m_path2Res[param.m_path];
            }

            Res resitem = findResFormPool(param.m_type, param.m_resNeedCoroutine);
            if (param.m_type == ResPackType.eLevelType)
            {
                if (!resitem)
                {
                    m_path2Res[param.m_path] = Ctx.m_instance.m_dataTrans.gameObject.AddComponent<LevelRes>() as LevelRes;
                    if (!param.m_resNeedCoroutine)
                    {
                        //m_path2Res[param.m_path] = new LevelRes();
                        m_path2Res[param.m_path].enabled = false;
                    }
                }
                else
                {
                    m_path2Res[param.m_path] = resitem;
                    m_path2Res[param.m_path].enabled = true;
                }
            }
            else if (param.m_type == ResPackType.eBundleType)
            {
                if (!resitem)
                {
                    m_path2Res[param.m_path] = Ctx.m_instance.m_dataTrans.gameObject.AddComponent<BundleRes>() as BundleRes;
                    if (!param.m_resNeedCoroutine)
                    {
                        m_path2Res[param.m_path].enabled = false;
                    }
                }
                else
                {
                    m_path2Res[param.m_path] = resitem;
                    m_path2Res[param.m_path].enabled = true;
                }

                (m_path2Res[param.m_path] as BundleRes).prefabName = param.m_prefabName;
            }

            m_path2Res[param.m_path].resNeedCoroutine = param.m_resNeedCoroutine;
            m_path2Res[param.m_path].type = param.m_type;
            m_path2Res[param.m_path].path = param.m_path;
            m_path2Res[param.m_path].resLoadType = param.m_resLoadType;

            if (param.m_cb != null)
            {
                m_path2Res[param.m_path].onInitedCB += param.m_cb;
            }

            LoadItem loaditem = findLoadItemFormPool(param.m_type, param.m_resNeedCoroutine);
            if (!loaditem)
            {
                if (param.m_type)
                loaditem = Ctx.m_instance.m_dataTrans.gameObject.AddComponent<LoadItem>() as LoadItem; 

                if (!param.m_loadNeedCoroutine)
                {
                    // loaditem = new LoadItem();
                    loaditem.enabled = false;
                }
            }
            else
            {
                loaditem.enabled = true;
            }

            loaditem.type = param.m_type;
            loaditem.resLoadType = param.m_resLoadType;
            loaditem.path = param.m_path;
            loaditem.onLoaded += onLoad;
            if (ResPackType.eLevelType == param.m_type)
            {
                (loaditem as LevelLoadItem).levelName = param.m_lvlName;
            }

            if (m_curNum < m_maxParral)
            {
                m_path2LDItem[param.m_path] = loaditem;
                m_path2LDItem[param.m_path].load();
                ++m_curNum;
            }
            else
            {
                m_willLDItem.Add(loaditem);
            }

            return m_path2Res[param.m_path];
        }

        public void unload(string path)
        {
            if (m_path2Res.ContainsKey(path))
            {
                if (m_path2Res[path].resNeedCoroutine)
                {
                    m_path2Res[path].enabled = false;       // 不移除，仅仅是不再更新
                    //GameObject.Destroy(m_path2Res[path]); // 不再从 gameobject 上移除了
                }

                m_path2Res[path].unload();
                m_path2Res[path].reset();
                m_noUsedLDItem.Add(m_path2Res[path]);

                m_path2Res.Remove(path);
            }
        }

        public void onLoad(LoadItem item)
        {
            string path = item.path;
            item.onLoaded -= onLoad;
            if(m_path2Res[path] != null)
            {
                m_path2Res[path].init(m_path2LDItem[path]);
            }

            if (item.loadNeedCoroutine)
            {
                item.enabled = false;       // 不移除，仅仅是不再更新
                //GameObject.Destroy(item); // 不再从 gameobject 上移除了
            }

            item.reset();
            m_noUsedLDItem.Add(item);
            m_path2LDItem.Remove(path);

            --m_curNum;
            loadNextItem();
        }

        protected void loadNextItem()
        {
            if (m_curNum < m_maxParral)
            {
                if(m_willLDItem.Count > 0)
                {
                    m_path2LDItem[(m_willLDItem[0] as LoadItem).path] = m_willLDItem[0] as LoadItem;
                    m_willLDItem.RemoveAt(0);

                    ++m_curNum;
                }
            }
        }

        protected Res findResFormPool(ResPackType type, bool resNeedCoroutine)
        {
            foreach(Res item in m_noUsedResItem)
            {
                if(item.type == type && item.resNeedCoroutine == resNeedCoroutine)
                {
                    return item;
                }
            }

            return null;
        }

        protected LoadItem findLoadItemFormPool(ResPackType type, bool loadNeedCoroutine)
        {
            foreach (LoadItem item in m_noUsedLDItem)
            {
                if (item.type == type && item.loadNeedCoroutine == loadNeedCoroutine)
                {
                    return item;
                }
            }

            return null;
        }

        public LoadParam getLoadParam()
        {
            return m_loadParam;
        }
    }
}