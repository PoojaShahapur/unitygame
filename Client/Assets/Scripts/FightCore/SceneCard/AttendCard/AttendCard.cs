using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 随从卡，必然是自己的， Enemy 随从卡是用的是 BlackCard，即可以作为攻击者，也可以作为被击者
     */
    public class AttendCard : CanOutCard
    {
        public AttendCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_ioControl = new AttendIOControl(this);
            m_render = new SelfHandCardRender(this);
            m_sceneCardBaseData.m_effectControl = new AttendCardEffectControl(this);
        }

        public override string getDesc()
        {
            return string.Format("CardType = AttendCard, CardSide = {0}, CardArea = {1}, CardPos = {2}, CardClientId = {3}, ThisId = {4}", getSideStr(), getAreaStr(), getPos(), m_ClientId, getThisId());
        }
    }
}