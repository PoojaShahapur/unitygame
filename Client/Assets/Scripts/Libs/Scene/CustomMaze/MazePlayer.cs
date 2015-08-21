using SDK.Common;

namespace SDK.Lib
{
    public class MazePlayer : AuxComponent
    {
        protected MazePlayerTrackAniControl m_mazePlayerTrackAniControl;
        protected MazeAnimatorControl m_mazeAnimatorControl;
        protected SceneEffect m_sceneEffect;

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

        public SceneEffect sceneEffect
        {
            get
            {
                return m_sceneEffect;
            }
            set
            {
                m_sceneEffect = value;
            }
        }

        public void init()
        {
            string path = "RootGo/AgentGo";
            this.selfGo = UtilApi.GoFindChildByPObjAndName(path);

            m_mazePlayerTrackAniControl = new MazePlayerTrackAniControl(this);
            m_sceneEffect = Ctx.m_instance.m_sceneEffectMgr.addSceneEffect(31, this.selfGo, false, true, true);
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
