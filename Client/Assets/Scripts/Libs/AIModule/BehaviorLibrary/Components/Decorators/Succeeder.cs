using System;

namespace BehaviorLibrary
{
    public class Succeeder : SingleBranchComponent
    {
        /// <summary>
        /// returns a success even when the decorated component failed
        /// </summary>
        /// <param name="behavior">behavior to run</param>
        public Succeeder(BehaviorComponent behavior)
        {
            m_childBehavior = behavior;
        }
        
        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            ReturnCode = m_childBehavior.Behave();
            if (ReturnCode == BehaviorReturnCode.Failure) {
                ReturnCode = BehaviorReturnCode.Success;
            }
            return ReturnCode;
        }
    }
}