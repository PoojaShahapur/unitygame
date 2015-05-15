using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要管理各种 UI 的 Prefab 元素
     */
    public class UIPrefabMgr : ResMgrBase
    {
        public override void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();

            // 获取资源单独保存
            (m_path2ResDic[path] as UIPrefabRes).m_go = res.getObject(res.getPrefabName()) as GameObject;
            if (m_path2ListenItemDic[path].m_loaded != null)
            {
                m_path2ListenItemDic[path].m_loaded(m_path2ResDic[path]);
            }

            base.onLoaded(resEvt);
        }
    }
}