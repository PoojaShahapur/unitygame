﻿using SDK.Common;
using System;

namespace BehaviorLibrary
{
    public class Counter : SingleBranchComponent
    {
        private int _MaxCount;
        private int _Counter = 0;

        /// <summary>
        /// executes the behavior based on a counter
        /// -each time Counter is called the counter increments by 1
        /// -Counter executes the behavior when it reaches the supplied maxCount
        /// </summary>
        /// <param name="maxCount">max number to count to</param>
        /// <param name="behavior">behavior to run</param>
        public Counter(int maxCount, BehaviorComponent behavior)
        {
            _MaxCount = maxCount;
            m_childBehavior = behavior;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                if (_Counter < _MaxCount)
                {
                    _Counter++;
                    ReturnCode = BehaviorReturnCode.Running;
                    return BehaviorReturnCode.Running;
                }
                else
                {
                    _Counter = 0;
                    ReturnCode = m_childBehavior.Behave();
                    return ReturnCode;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Ctx.m_instance.m_logSys.catchLog(e.ToString());
#endif
                ReturnCode = BehaviorReturnCode.Failure;
                return BehaviorReturnCode.Failure;
            }
        }
    }
}