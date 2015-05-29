namespace BehaviorLibrary.Components
{
    public class BTRoot : SingleBranchComponent
    {
        public override BehaviorReturnCode Behave()
        {
            return m_childBehavior.Behave();
        }
    }
}