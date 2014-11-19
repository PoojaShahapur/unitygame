using System;

namespace BehaviorLibrary.Components.Conditionals
{
    public class Condition : MulBranchComponent
    {
        private Func<InsParam, Boolean> _Bool;

        /// <summary>
        /// Returns a return code equivalent to the test 
        /// -Returns Success if true
        /// -Returns Failure if false
        /// </summary>
        /// <param name="test">the value to be tested</param>
        public Condition(Func<InsParam, Boolean> test)
        {
            _Bool = test;
        }

        public Func<InsParam, Boolean> boolFunc
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
        public override BehaviorReturnCode Behave(InsParam inputParam)
        {

            try
            {
                switch (_Bool.Invoke(inputParam))
                {
                    case true:
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    case false:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    default:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.Error.WriteLine(e.ToString());
#endif
                ReturnCode = BehaviorReturnCode.Failure;
                return ReturnCode;
            }
        }
    }
}