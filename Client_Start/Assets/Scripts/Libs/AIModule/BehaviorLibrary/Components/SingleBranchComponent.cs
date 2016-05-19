namespace BehaviorLibrary
{
    public class SingleBranchComponent : BehaviorComponent
    {
        protected BehaviorComponent m_childBehavior;

        public override void addChild(BehaviorComponent child)
        {
            m_childBehavior = child;
        }
    }
}