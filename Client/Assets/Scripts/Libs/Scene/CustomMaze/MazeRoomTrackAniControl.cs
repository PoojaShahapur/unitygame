using SDK.Common;

namespace SDK.Lib
{
    public class MazeRoomTrackAniControl
    {
        protected MazeRoom m_mazeRoom;

        protected NumAniParallel m_numAniParal;

        public MazeRoomTrackAniControl(MazeRoom mazeRoom_)
        {
            m_mazeRoom = mazeRoom_;
            m_numAniParal = new NumAniParallel();
        }

        // 移动动画
        public void moveToDestPos()
        {
            UtilApi.normalRot(m_mazeRoom.selfGo.transform);

            PosAni posAni;
            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_mazeRoom.selfGo);
            posAni.destPos = m_mazeRoom.origPos;

            m_numAniParal.play();
        }

        // 直接移动到目标位置
        public void goToDestPos()
        {
            UtilApi.setPos(m_mazeRoom.selfGo.transform, m_mazeRoom.origPos);
        }
    }
}