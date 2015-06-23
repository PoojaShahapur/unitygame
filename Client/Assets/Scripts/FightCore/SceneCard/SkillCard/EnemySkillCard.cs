namespace FightCore
{
    public class EnemySkillCard : SkillCard
    {
        public EnemySkillCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_ioControl = new EnemySkillIOControl(this);
        }
    }
}