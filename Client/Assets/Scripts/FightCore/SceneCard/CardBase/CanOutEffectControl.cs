namespace FightCore
{
    public class CanOutEffectControl : ExceptBlackEffectControl
    {
        public CanOutEffectControl(SceneCardBase rhv) :
            base(rhv)
        {

        }

        // 随从卡、法术卡，手牌中的卡，拖到场景中后，如果原来有 m_frameEffect ，会自动切换到场牌特效，但是如果原来没有，需要自己切换
        override public void updateCanLaunchAttState(bool bEnable)
        {
            if (m_card.sceneCardItem.bSelfSide())
            {
                m_frameEffectId = 1;

                if (bEnable)
                {
                    if (m_card.sceneCardItem != null)
                    {
                        if (m_card.behaviorControl.canLaunchAtt())
                        {
                            addFrameEffect();
                            m_frameEffect.play();
                        }
                        else
                        {
                            if (m_frameEffect != null)
                            {
                                m_frameEffect.stop();
                            }
                        }
                    }
                }
                else
                {
                    if (m_frameEffect != null)
                    {
                        m_frameEffect.stop();
                    }
                }
            }
        }
    }
}