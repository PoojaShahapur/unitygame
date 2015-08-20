using SDK.Common;

namespace SDK.Lib
{
    public class MazePlayer : AuxComponent
    {
        protected MazePlayerTrackAniControl m_mazePlayerTrackAniControl;

        public MazePlayer()
        {
            m_mazePlayerTrackAniControl = new MazePlayerTrackAniControl(this);
        }

        public MazePlayerTrackAniControl mazePlayerTrackAniControl
        {
            get
            {
                return m_mazePlayerTrackAniControl;
            }
            set
            {
                m_mazePlayerTrackAniControl = value;
            }
        }

        public void init()
        {
            string path = "RootGo/AgentGo";
            this.selfGo = UtilApi.GoFindChildByPObjAndName(path);
        }

        public void setStartPos()
        {
            m_mazePlayerTrackAniControl.setStartPos();
        }

        public void startMove()
        {
            
        }

        public void clearPath()
        {
            m_mazePlayerTrackAniControl.ptList.Clear();
        }
    }
}
