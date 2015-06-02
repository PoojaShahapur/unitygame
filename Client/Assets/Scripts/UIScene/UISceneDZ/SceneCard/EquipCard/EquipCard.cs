using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 武器卡
     */
    public class EquipCard : SceneCard
    {
        public EquipCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new EquipClickControl(this);
            m_sceneCardBaseData.m_aniControl = new EquipAniControl(this);
            m_sceneCardBaseData.m_dragControl = new EquipDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new EquipBehaviorControl(this);

            m_render = new SceneCardPlayerRender();
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }
    }
}