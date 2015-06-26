namespace FightCore
{
    public class HeroEffectControl : NotOutEffectControl
    {
        public HeroEffectControl(SceneCardBase rhv) :
            base(rhv)
        {

        }

        override public void updateCardAttackedState()
        {
            m_frameEffectId = 5;
            base.updateCardAttackedState();
        }
    }
}