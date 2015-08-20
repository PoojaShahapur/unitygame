using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class MazeIOControl
    {
        protected MazeRoom m_mazeRoom;

        public MazeIOControl(MazeRoom mazeRoom_)
        {
            m_mazeRoom = mazeRoom_;
        }

        // 开启拖动
        public void enableDrag()
        {
            if (m_mazeRoom.selfGo.GetComponent<UIDragObject>() == null)
            {
                UIDragObject drag = m_mazeRoom.selfGo.AddComponent<UIDragObject>();
                drag.target = m_mazeRoom.selfGo.transform;
                drag.m_startDragDisp = onStartDrag;
                drag.m_moveDisp = onMove;
                drag.m_dropEndDisp = onDragEnd;
                drag.m_canMoveDisp = canMove;
                drag.m_planePt = new Vector3(0, 0, 0);
            }
            if (m_mazeRoom.selfGo.GetComponent<WindowDragTilt>() == null)
            {
                m_mazeRoom.selfGo.AddComponent<WindowDragTilt>();
            }
        }

        // 关闭拖放功能
        public void disableDrag()
        {
            UIDragObject drag = m_mazeRoom.selfGo.GetComponent<UIDragObject>();
            UtilApi.Destroy(drag);
            WindowDragTilt dragTitle = m_mazeRoom.selfGo.GetComponent<WindowDragTilt>();
            UtilApi.Destroy(dragTitle);
        }

        // 能拖动的必然所有的操作都完成后才能操作
        protected void onStartDrag()
        {
            // 保存当前操作
            Ctx.m_instance.m_maze.mazeData.mazeOp.curMazeRoom = m_mazeRoom;     // 设置当前拖放的目标
            enableDragTitle();      // Drag Title 动画
        }

        protected void onMove()
        {
            
        }

        // 拖放结束处理
        protected void onDragEnd()
        {
            int idx = Ctx.m_instance.m_maze.mazeData.roomInfo.getRoomIdx(m_mazeRoom);
            if(idx != m_mazeRoom.iTag)
            {
                MazeRoom mazeRoom = Ctx.m_instance.m_maze.mazeData.roomInfo.getMazeRoom(idx);
                mazeRoom.iTag = m_mazeRoom.iTag;
                Vector3 origPos = mazeRoom.origPos;
                mazeRoom.origPos = m_mazeRoom.origPos;
                m_mazeRoom.iTag = idx;
                m_mazeRoom.origPos = origPos;

                Ctx.m_instance.m_maze.mazeData.roomInfo.updateRoomList();

                mazeRoom.mazeRoomTrackAniControl.moveToDestPos();
                m_mazeRoom.mazeRoomTrackAniControl.moveToDestPos();
            }
        }

        // 指明当前是否可以改变位置
        protected bool canMove()
        {
            return Ctx.m_instance.m_maze.mazeData.mazeOp.bStart;
        }

        public void enableDragTitle()
        {
            WindowDragTilt dragTitle = m_mazeRoom.selfGo.GetComponent<WindowDragTilt>();
            if (dragTitle != null)
            {
                dragTitle.enabled = true;
            }
        }

        public void disableDragTitle()
        {
            WindowDragTilt dragTitle = m_mazeRoom.selfGo.GetComponent<WindowDragTilt>();
            if (dragTitle != null)
            {
                dragTitle.enabled = false;
            }
        }
    }
}