using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 法术卡
     */
    public class MagicCard : SceneCard
    {
        public MagicCard(SceneDZData sceneDZData):
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new MagicClickControl(this);
            m_sceneCardBaseData.m_trackAniControl = new MagicAniControl(this);
            m_sceneCardBaseData.m_dragControl = new MagicDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new MagicBehaviorControl(this);

            m_render = new SceneCardPlayerRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }
    }
}