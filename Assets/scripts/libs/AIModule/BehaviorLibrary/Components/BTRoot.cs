namespace BehaviorLibrary.Components
{
    public class BTRoot : SingleBranchComponent
    {
        public override BehaviorReturnCode Behave(InsParam inputParam)
        {
            return m_childBehavior.Behave(inputParam);
        }
    }
}