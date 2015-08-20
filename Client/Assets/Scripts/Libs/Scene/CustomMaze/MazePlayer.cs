using SDK.Common;

namespace SDK.Lib
{
    public class MazePlayer : AuxComponent
    {
        protected MazePlayerTrackAniControl m_mazePlayerTrackAniControl;
        protected MazeAnimatorControl m_mazeAnimatorControl;

        public MazePlayer()
        {
            m_mazeAnimatorControl = new MazeAnimatorControl();
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

            m_mazePlayerTrackAniControl = new MazePlayerTrackAniControl(this);
            Ctx.m_instance.m_sceneEffectMgr.addSceneEffect(7, this.selfGo, false, true, true);
        }

        public void setStartPos()
        {
            m_mazePlayerTrackAniControl.setStartPos();
        }

        public void startMove()
        {
            m_mazePlayerTrackAniControl.startMove();
        }

        public void clearPath()
        {
            m_mazePlayerTrackAniControl.ptList.Clear();
        }
    }
}
