using SDK.Common;

namespace SDK.Lib
{
    public class ItemSceneDraggableIOBase : ItemSceneIOBase
    {
        public override void onLoaded(IDispatchObject resEvt)            // 资源加载成功
        {
            base.onLoaded(resEvt);

            UIDragObject drag = m_go.AddComponent<UIDragObject>();
            drag.target = m_go.transform;

            m_go.AddComponent<WindowDragTilt>();
        }
    }
}