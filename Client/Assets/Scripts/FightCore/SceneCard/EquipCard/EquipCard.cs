using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 武器卡
     */
    public class EquipCard : NotOutCard
    {
        public EquipCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new EquipAniControl(this);
            m_sceneCardBaseData.m_ioControl = new EquipIOControl(this);
            m_sceneCardBaseData.m_behaviorControl = new EquipBehaviorControl(this);

            m_render = new EquipRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        override public void updateCardDataChangeBySvr(t_Card svrCard_ = null)
        {

        }

        override public void updateCardDataNoChangeByTable()
        {

        }

        public override string getDesc()
        {
            return string.Format("CardType = EquipCard, CardSide = {0}, CardArea = {1}, CardPos = {2}, CardClientId = {3}, ThisId = {4}", getSideStr(), getAreaStr(), getPos(), m_ClientId, getThisId());
        }
    }
}