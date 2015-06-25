namespace FightCore
{
    /**
     * @brief 英雄卡
     */
    public class SelfHeroCard : HeroCard
    {
        public SelfHeroCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_behaviorControl = new SelfHeroBehaviorControl(this);
            m_sceneCardBaseData.m_effectControl = new SelfHeroEffectControl(this);
        }
    }
}