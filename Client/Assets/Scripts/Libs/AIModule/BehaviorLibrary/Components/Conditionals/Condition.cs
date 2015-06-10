using SDK.Common;
using System;

namespace BehaviorLibrary
{
    public class Condition : MulBranchComponent
    {
        private Func<Boolean> _Bool;

        /// <summary>
        /// Returns a return code equivalent to the test 
        /// -Returns Success if true
        /// -Returns Failure if false
        /// </summary>
        /// <param name="test">the value to be tested</param>
        public Condition(Func<Boolean> test)
        {
            _Bool = test;
        }

        public Func<Boolean> boolFunc
        {
            get
            {
                return _Bool;
            }
            set
            {
                _Bool = value;
            }
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                switch (_Bool.Invoke())
                {
                    case true:
                    {
                        // 执行所有的代码
                        execAllChild();
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    }
                    case false:
                    {
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    }
                    default:
                    {
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Ctx.m_instance.m_logSys.catchLog(e.ToString());
#endif
                ReturnCode = BehaviorReturnCode.Failure;
                return ReturnCode;
            }
        }
    }
}