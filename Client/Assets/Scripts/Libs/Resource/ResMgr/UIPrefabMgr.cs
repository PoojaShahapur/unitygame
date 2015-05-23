using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要管理各种 UI 的 Prefab 元素
     */
    public class UIPrefabMgr : ResMgrBase
    {
        public override void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            string path = res.GetPath();

            // 获取资源单独保存
            (m_path2ResDic[path] as UIPrefabRes).m_go = res.getObject(res.getPrefabName()) as GameObject;
            m_path2ResDic[path].refCountResLoadResultNotify.loadEventDispatch.dispatchEvent(m_path2ResDic[path]);

            base.onLoadEventHandle(dispObj);
        }
    }
}