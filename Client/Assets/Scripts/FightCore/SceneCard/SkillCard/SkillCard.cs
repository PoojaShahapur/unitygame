using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    public class SkillCard : NotOutCard
    {
        public SkillCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new SkillAniControl(this);
            m_sceneCardBaseData.m_behaviorControl = new SkillBehaviorControl(this);

            m_render = new SkillRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        override public void setBaseInfo(EnDZPlayer m_playerSide, CardArea area, CardType cardType)
        {
            UtilApi.setPos(this.transform(), m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)area].transform.localPosition);
        }

        override public void updateCardDataChangeBySvr(t_Card svrCard_ = null)
        {
            base.updateCardDataChangeBySvr();

            if (svrCard_ == null)
            {
                svrCard_ = m_sceneCardItem.svrCard;
            }

            AuxLabel text = new AuxLabel();
            text.setSelfGo(m_render.gameObject(), "UIRoot/MpText");         // 消耗
            text.text = svrCard_.mpcost.ToString();
        }

        override public void updateCardDataNoChangeByTable()
        {

        }

        public override string getDesc()
        {
            return string.Format("CardType = SkillCard, CardSide = {0}, CardArea = {1}, CardPos = {2}, CardClientId = {3}, ThisId = {4}", getSideStr(), getAreaStr(), getPos(), m_ClientId, getThisId());
        }
    }
}