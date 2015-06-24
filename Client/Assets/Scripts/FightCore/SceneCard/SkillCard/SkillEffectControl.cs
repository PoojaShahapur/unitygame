namespace FightCore
{
    public class SkillEffectControl : EffectControl
    {
        public SkillEffectControl(SceneCardBase rhv) :
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