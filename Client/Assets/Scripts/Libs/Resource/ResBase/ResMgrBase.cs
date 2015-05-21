using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 资源管理器，不包括资源加载
     */
    public class ResMgrBase
    {
        protected Dictionary<string, ResListenerItem> m_path2ListenItemDic = new Dictionary<string, ResListenerItem>(); // 这里面记录的是外部回调，千万不要把 onLoaded 这个函数加入进入
        public Dictionary<string, InsResBase> m_path2ResDic = new Dictionary<string, InsResBase>();

        // 同步加载，立马加载完成，并且返回加载的资源
        public T syncGet<T>(string path) where T : InsResBase, new()
        {
            LoadParam param;
            param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = path;
            //param.m_loaded = onLoaded;        // 这个地方是同步加载，因此不需要回调，如果写了，就会形成死循环
            //param.m_failed = onFailed;
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
                m_path2ResDic[param.m_path].incRef();
                if (m_path2ResDic[param.m_path].m_isLoaded && m_path2ResDic[param.m_path].m_isSucceed)
                {
                    if (param.m_loaded != null)
                    {
                        param.m_loaded(m_path2ResDic[param.m_path]);        // 直接通知上层完成加载
                    }
                    return m_path2ResDic[param.m_path];
                }
            }
            else
            {
                m_path2ResDic[param.m_path] = new T();
                m_path2ResDic[param.m_path].incRef();
                m_path2ResDic[param.m_path].m_path = param.m_path;
            }

            if (!m_path2ListenItemDic.ContainsKey(param.m_path))
            {
                m_path2ListenItemDic[param.m_path] = new ResListenerItem();
                m_path2ListenItemDic[param.m_path].copyForm(param);
                param.m_failed = onFailed;
                param.m_loaded = onLoaded;
                Ctx.m_instance.m_resLoadMgr.loadResources(param);
            }
            else
            {
                m_path2ListenItemDic[param.m_path].copyForm(param);
            }

            return m_path2ResDic[param.m_path];
        }

        virtual public void unload(string path)
        {
            if (m_path2ResDic.ContainsKey(path))
            {
                m_path2ResDic[path].decRef();
                if (m_path2ResDic[path].refNum == 0)
                {
                    m_path2ResDic[path].unload();
                    m_path2ResDic.Remove(path);

                    // 卸载加载的原始资源
                    //Ctx.m_instance.m_resLoadMgr.unloadNoRef(path);
                    UtilApi.UnloadUnusedAssets();           // 异步卸载共用资源
                }
            }
        }

        public virtual void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();
            m_path2ListenItemDic.Remove(path);

            if (m_path2ResDic.ContainsKey(path))
            {
                m_path2ResDic[path].m_isLoaded = true;
                m_path2ResDic[path].m_isSucceed = true;
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("路径不能查找到 {0}", path));
            }

            //if (m_path2ListenItemDic.ContainsKey(path))
            //{
            //    m_path2ListenItemDic[path].m_loaded(resEvt);
            //}

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unloadNoRef(path);
        }

        public virtual void onFailed(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();

            if (m_path2ResDic.ContainsKey(path))
            {
                m_path2ResDic[path].m_isLoaded = true;
                m_path2ResDic[path].m_isSucceed = false;
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("路径不能查找到 {0}", path));
            }

            if (m_path2ListenItemDic.ContainsKey(path))
            {
                if (m_path2ListenItemDic[path].m_failed != null)
                {
                    m_path2ListenItemDic[path].m_failed(resEvt);
                }
                m_path2ListenItemDic.Remove(path);
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("路径不能查找到 {0}", path));
            }

            // 彻底卸载
            Ctx.m_instance.m_resLoadMgr.unloadNoRef(path);
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
                pathList.Add(kv.Key);
            }

            foreach(string path in pathList)
            {
                unload(path);
            }

            pathList.Clear();
        }
    }
}