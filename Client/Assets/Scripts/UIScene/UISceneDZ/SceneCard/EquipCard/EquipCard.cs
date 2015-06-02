using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 武器卡
     */
    public class EquipCard : SceneCardBase
    {
        public EquipCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_clickControl = new EquipClickControl(this);
            m_aniControl = new EquipAniControl(this);
            m_dragControl = new EquipDragControl(this);
            m_behaviorControl = new EquipBehaviorControl(this);

            m_render = new SceneCardPlayerRender();
            m_effectControl = new EffectControl(this);
        }
    }
}