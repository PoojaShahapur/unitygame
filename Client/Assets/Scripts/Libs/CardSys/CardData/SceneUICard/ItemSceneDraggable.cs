using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 场景可拖放
     */
    public class ItemSceneDraggable : ItemSceneBase
    {
        public override void onLoaded(IDispatchObject resEvt)            // 资源加载成功
        {
            base.onLoaded(resEvt);

            UIDragObject drag = m_go.AddComponent<UIDragObject>();
            drag.target = m_go.transform;

            //WindowDragTilt title = m_go.AddComponent<WindowDragTilt>();
            m_go.AddComponent<WindowDragTilt>();
        }
    }
}