using SDK.Lib;

namespace Game.UI
{
    public class SkillCard : SceneCard
    {
        public SkillCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new SkillClickControl(this);
            m_sceneCardBaseData.m_aniControl = new SkillAniControl(this);
            m_sceneCardBaseData.m_dragControl = new SkillDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new SkillBehaviorControl(this);

            m_render = new SceneCardPlayerRender();
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }
    }
}