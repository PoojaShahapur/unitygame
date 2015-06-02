using SDK.Lib;

namespace Game.UI
{
    public class SkillCard : SceneCardBase
    {
        public SkillCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_clickControl = new SkillClickControl(this);
            m_aniControl = new SkillAniControl(this);
            m_dragControl = new SkillDragControl(this);
            m_behaviorControl = new SkillBehaviorControl(this);

            m_render = new SceneCardPlayerRender();
            m_effectControl = new EffectControl(this);
        }
    }
}