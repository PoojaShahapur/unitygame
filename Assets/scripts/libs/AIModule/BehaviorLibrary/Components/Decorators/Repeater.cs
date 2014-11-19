using System;
using BehaviorLibrary;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Decorators;
using BehaviorLibrary.Components.Utility;

namespace BehaviorLibrary.Components.Decorators
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
        public override BehaviorReturnCode Behave(InsParam inputParam)
        {
            ReturnCode = m_childBehavior.Behave(inputParam);
            ReturnCode = BehaviorReturnCode.Running;
            return BehaviorReturnCode.Running;
        }
    }
}