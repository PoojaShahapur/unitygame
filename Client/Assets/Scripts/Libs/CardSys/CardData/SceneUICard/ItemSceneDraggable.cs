using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 场景可拖放
     */
    public class ItemSceneDraggable : ItemSceneBase
    {
        public override void onLoadEventHandle(IDispatchObject dispObj)
        {
            base.onLoadEventHandle(dispObj);

            UIDragObject drag = m_go.AddComponent<UIDragObject>();
            drag.target = m_go.transform;

            //WindowDragTilt title = m_go.AddComponent<WindowDragTilt>();
            m_go.AddComponent<WindowDragTilt>();
        }
    }
}