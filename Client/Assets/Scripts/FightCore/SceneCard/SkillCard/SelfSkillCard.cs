namespace FightCore
{
    public class SelfSkillCard : SkillCard
    {
        public SelfSkillCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_behaviorControl = new SelfSkillBehaviorControl(this);
            m_sceneCardBaseData.m_ioControl = new SelfSkillIOControl(this);
            m_sceneCardBaseData.m_effectControl = new SelfSkillEffectControl(this);
        }
    }
}