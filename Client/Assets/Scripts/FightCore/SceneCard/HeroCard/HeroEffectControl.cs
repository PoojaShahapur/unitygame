namespace FightCore
{
    public class HeroEffectControl : EffectControl
    {
        public HeroEffectControl(SceneCardBase rhv) :
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