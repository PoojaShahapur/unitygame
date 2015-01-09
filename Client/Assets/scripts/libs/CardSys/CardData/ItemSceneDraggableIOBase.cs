using SDK.Common;

namespace SDK.Lib
{
    public class ItemSceneDraggableIOBase : ItemSceneIOBase
    {
        public override void onloaded(IDispatchObject resEvt)            // 资源加载成功
        {
            base.onloaded(resEvt);

            UIDragObject drag = m_go.AddComponent<UIDragObject>();
            drag.target = m_go.transform;

            WindowDragTilt title = m_go.AddComponent<WindowDragTilt>();
        }
    }
}