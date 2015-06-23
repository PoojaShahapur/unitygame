namespace FightCore
{
    public class SelfSkillCard : SkillCard
    {
        public SelfSkillCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_ioControl = new SelfSkillIOControl(this);
        }
    }
}