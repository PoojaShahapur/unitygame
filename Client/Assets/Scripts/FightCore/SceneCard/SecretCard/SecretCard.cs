using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 奥秘卡，必然是自己的， Enemy 使用的是 BlackCard
     */
    public class SecretCard : CanOutCard
    {
        public SecretCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new SecretAniControl(this);
            m_sceneCardBaseData.m_ioControl = new SecretIOControl(this);
            m_sceneCardBaseData.m_behaviorControl = new SecretBehaviorControl(this);

            m_render = new SelfHandCardRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        public override string getDesc()
        {
            return string.Format("CardType = SecretCard, CardSide = {0}, CardArea = {1}, CardPos = {2}, CardClientId = {3}, ThisId = {4}", getSideStr(), getAreaStr(), getPos(), m_ClientId, getThisId());
        }
    }
}