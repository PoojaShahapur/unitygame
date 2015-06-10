﻿using SDK.Common;
using System;

namespace BehaviorLibrary
{
    public class Selector : MulBranchComponent
    {
        /// <summary>
        /// Selects among the given behavior components
        /// Performs an OR-Like behavior and will "fail-over" to each successive component until Success is reached or Failure is certain
        /// -Returns Success if a behavior component returns Success
        /// -Returns Running if a behavior component returns Running
        /// -Returns Failure if all behavior components returned Failure
        /// </summary>
        /// <param name="behaviors">one to many behavior components</param>
		public Selector()
        {
            
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            for (int i = 0; i < m_childBehaviorsList.Count; i++)
            {
                try
                {
                    switch (m_childBehaviorsList[i].Behave())
                    {
                        case BehaviorReturnCode.Failure:
                            continue;
                        case BehaviorReturnCode.Success:
                            ReturnCode = BehaviorReturnCode.Success;
                            return ReturnCode;
                        case BehaviorReturnCode.Running:
                            ReturnCode = BehaviorReturnCode.Running;
                            return ReturnCode;
                        default:
                            continue;
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    Ctx.m_instance.m_logSys.catchLog(e.ToString());
#endif
                    continue;
                }
            }

            ReturnCode = BehaviorReturnCode.Failure;
            return ReturnCode;
        }
    }
}