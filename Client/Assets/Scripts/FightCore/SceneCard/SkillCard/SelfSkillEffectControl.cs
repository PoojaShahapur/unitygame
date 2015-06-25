namespace FightCore
{
    public class SelfSkillEffectControl : SkillEffectControl
    {
        public SelfSkillEffectControl(SceneCardBase rhv) :
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