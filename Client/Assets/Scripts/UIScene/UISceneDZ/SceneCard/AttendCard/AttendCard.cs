namespace Game.UI
{
    /**
     * @brief 随从卡
     */
    public class AttendCard : SceneCardBase
    {
        public AttendCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_clickControl = new AttendClickControl(this);
            m_aniControl = new AttendAniControl(this);
            m_dragControl = new AttendDragControl(this);
            m_behaviorControl = new AttendBehaviorControl(this);
        }
    }
}