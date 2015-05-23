using SDK.Common;

namespace SDK.Lib
{
    public class ItemSceneDraggableIOBase : ItemSceneIOBase
    {
        public override void onLoadEventHandle(IDispatchObject dispObj)
        {
            base.onLoadEventHandle(dispObj);

            UIDragObject drag = m_go.AddComponent<UIDragObject>();
            drag.target = m_go.transform;

            m_go.AddComponent<WindowDragTilt>();
        }
    }
}