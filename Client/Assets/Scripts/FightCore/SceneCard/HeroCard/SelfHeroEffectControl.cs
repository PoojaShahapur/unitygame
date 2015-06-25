namespace FightCore
{
    public class SelfHeroEffectControl : HeroEffectControl
    {
        public SelfHeroEffectControl(SceneCardBase rhv) :
            base(rhv)
        {

        }

        override public void updateCanLaunchAttState(bool bEnable)
        {
            m_frameEffectId = 5;
            base.updateCanLaunchAttState(bEnable);
        }
    }
}