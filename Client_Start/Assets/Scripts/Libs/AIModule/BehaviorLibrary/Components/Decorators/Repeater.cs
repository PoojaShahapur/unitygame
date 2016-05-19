using System;

namespace BehaviorLibrary
{
    public class Repeater : SingleBranchComponent
    {
        /// <summary>
        /// executes the behavior every time again
        /// </summary>
        /// <param name="timeToWait">maximum time to wait before executing behavior</param>
        /// <param name="behavior">behavior to run</param>
        public Repeater(BehaviorComponent behavior)
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
            ReturnCode = BehaviorReturnCode.Running;
            return BehaviorReturnCode.Running;
        }
    }
}