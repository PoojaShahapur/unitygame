namespace Game.UI
{
    /**
     * @brief 法术卡
     */
    public class MagicCard : SceneCardBase
    {
        public MagicCard(SceneDZData sceneDZData):
            base(sceneDZData)
        {
            m_clickControl = new MagicClickControl(this);
            m_aniControl = new MagicAniControl(this);
            m_dragControl = new MagicDragControl(this);
            m_behaviorControl = new MagicBehaviorControl(this);
        }
    }
}