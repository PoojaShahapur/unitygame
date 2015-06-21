using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 随从卡
     */
    public class AttendCard : CanOutCard
    {
        public AttendCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new AttendAniControl(this);
            m_sceneCardBaseData.m_ioControl = new AttendDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new AttendBehaviorControl(this);

            m_render = new SceneCardPlayerRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        public override string getDesc()
        {
            return string.Format("CardType = AttendCard, CardSide = {0}, CardArea = {1}, CardPos = {2}, CardClientId = {3}, ThisId = {4}", getSideStr(), getAreaStr(), getPos(), m_ClientId, getThisId());
        }
    }
}