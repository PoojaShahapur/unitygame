using SDK.Lib;

namespace FightCore
{
    public class SecretCard : CanOutCard
    {
        public SecretCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new SecretClickControl(this);
            m_sceneCardBaseData.m_trackAniControl = new SecretAniControl(this);
            m_sceneCardBaseData.m_dragControl = new SecretDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new SecretBehaviorControl(this);

            m_render = new SceneCardPlayerRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        public override string getDesc()
        {
            return string.Format("CardType = SecretCard, CardSide = {0}, CardArea = {1}, CardPos = {2}, CardClientId = {3}, ThisId = {4}", getSideStr(), getAreaStr(), getPos(), m_ClientId, getThisId());
        }
    }
}