using SDK.Common;
using UnityEngine;

namespace FightCore
{
    public class CanOutIOControl : ExceptBlackIOControl
    {
        public CanOutIOControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        // 开启拖动
        override public void enableDrag()
        {
            if (m_card.gameObject().GetComponent<UIDragObject>() == null)
            {
                UIDragObject drag = m_card.gameObject().AddComponent<UIDragObject>();
                drag.target = m_card.gameObject().transform;
                drag.m_startDragDisp = onStartDrag;
                drag.m_moveDisp = onMove;
                drag.m_dropEndDisp = onDragEnd;
                drag.m_canMoveDisp = canMove;
                drag.m_planePt = new Vector3(0, SceneDZCV.DRAG_YDELTA, 0);
            }
            if (m_card.gameObject().GetComponent<WindowDragTilt>() == null)
            {
                m_card.gameObject().AddComponent<WindowDragTilt>();
            }
        }

        // 关闭拖放功能
        override public void disableDrag()
        {
            UIDragObject drag = m_card.gameObject().GetComponent<UIDragObject>();
            //drag.enabled = false;
            UtilApi.Destroy(drag);
            WindowDragTilt dragTitle = m_card.gameObject().GetComponent<WindowDragTilt>();
            //dragTitle.enabled = false;
            UtilApi.Destroy(dragTitle);
        }
    }
}