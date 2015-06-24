namespace FightCore
{
    public class EnemySkillBehaviorControl : SkillBehaviorControl
    {
        public EnemySkillBehaviorControl(SceneCardBase rhv) : 
            base(rhv)
        {

        }

        override public bool canLaunchAtt()
        {
            return false;
        }
    }
}
