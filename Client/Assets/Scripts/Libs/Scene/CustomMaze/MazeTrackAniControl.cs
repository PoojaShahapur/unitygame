using SDK.Common;

namespace SDK.Lib
{
    public class MazeTrackAniControl
    {
        protected MazeRoom m_mazeRoom;

        protected NumAniParallel m_numAniParal;

        public MazeTrackAniControl(MazeRoom mazeRoom_)
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
    }
}