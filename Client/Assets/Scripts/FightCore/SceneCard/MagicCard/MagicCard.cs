using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 法术卡
     */
    public class MagicCard : CanOutCard
    {
        public MagicCard(SceneDZData sceneDZData):
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new MagicClickControl(this);
            m_sceneCardBaseData.m_trackAniControl = new MagicAniControl(this);
            m_sceneCardBaseData.m_dragControl = new MagicDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new MagicBehaviorControl(this);

            m_render = new SceneCardPlayerRender(this);
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
    }
}