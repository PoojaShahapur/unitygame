using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 资源管理器，不包括资源加载
     */
    public class ResMgrBase
    {
        protected Dictionary<string, ResListenerItem> m_path2ListenItemDic = new Dictionary<string, ResListenerItem>();
        public Dictionary<string, object> m_path2ResDic = new Dictionary<string, object>();

        public void load(LoadParam param)
        {
            if (!m_path2ListenItemDic.ContainsKey(param.m_path))
            {
                m_path2ListenItemDic[param.m_path] = new ResListenerItem();
            }

            m_path2ListenItemDic[param.m_path].copyForm(param);

            param.m_failed = onFailed;
            param.m_loaded = onLoaded;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
        }

        public virtual void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();

            // 获取资源单独保存
            m_path2ResDic[path] = res.getObject(res.getPrefabName());

            if (m_path2ListenItemDic.ContainsKey(path))
            {
                m_path2ListenItemDic[path].m_loaded(resEvt);
            }

            // 彻底卸载
            Ctx.m_instance.m_resLoadMgr.unloadNoRef(path);
        }

        public virtual void onFailed(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();

            if (m_path2ListenItemDic.ContainsKey(path))
            {
                m_path2ListenItemDic[path].m_failed(resEvt);
            }

            // 彻底卸载
            Ctx.m_instance.m_resLoadMgr.unloadNoRef(path);
        }

        public object getRes(string path)
        {
            return m_path2ResDic[path];
        }
    }
}