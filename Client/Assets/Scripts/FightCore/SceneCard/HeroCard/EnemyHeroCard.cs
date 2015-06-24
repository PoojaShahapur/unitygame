namespace FightCore
{
    /**
     * @brief 英雄卡
     */
    public class EnemyHeroCard : HeroCard
    {
        public EnemyHeroCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_behaviorControl = new EnemyHeroBehaviorControl(this);
        }
    }
}