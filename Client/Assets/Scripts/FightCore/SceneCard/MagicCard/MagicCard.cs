using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 法术卡，必然是自己的， Enemy 使用的是 BlackCard
     */
    public class MagicCard : CanOutCard
    {
        public MagicCard(SceneDZData sceneDZData):
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_ioControl = new MagicIOControl(this);
            m_sceneCardBaseData.m_behaviorControl = new MagicBehaviorControl(this);

            m_render = new SelfHandCardRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        // 法术牌只有 MP 属性
        public override void updateCardDataChangeBySvr(t_Card svrCard_ = null)
        {
            base.updateCardDataChangeBySvr(svrCard_);

            if (svrCard_ == null)
            {
                svrCard_ = m_sceneCardItem.svrCard;
            }

            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_HAND)     // 手牌不同更新
                {
                    AuxLabel text = new AuxLabel();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/AttText");       // 攻击
                    text.text = "";
                    text.setSelfGo(m_render.gameObject(), "UIRoot/MpText");         // Magic
                    text.text = svrCard_.mpcost.ToString();
                    text.setSelfGo(m_render.gameObject(), "UIRoot/HpText");       // HP
                    text.text = "";
                }
                if (m_sceneCardItem.cardArea == CardArea.CARDCELLTYPE_COMMON)        // 场牌更新
                {

                }
            }
        }

        public override string getDesc()
        {
            return string.Format("CardType = MagicCard, CardSide = {0}, CardArea = {1}, CardPos = {2}, CardClientId = {3}, ThisId = {4}", getSideStr(), getAreaStr(), getPos(), m_ClientId, getThisId());
        }
    }
}