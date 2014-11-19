using System;

namespace BehaviorLibrary.Components.Actions
{
    public class BehaviorAction : LeafComponent
    {
        private Func<InsParam, BehaviorReturnCode> _Action;

        public BehaviorAction() { }

        public BehaviorAction(Func<InsParam, BehaviorReturnCode> action)
        {
            if (action != null)
            {
                _Action = action;
            }
            else
            {
                _Action = DefaultAction;
            }
        }

        protected BehaviorReturnCode DefaultAction(InsParam inputParam)
        {
            return BehaviorReturnCode.Success;
        }

        public Func<InsParam, BehaviorReturnCode> actionFunc
        {
            set
            {
                _Action = value;
            }
        }

        public override BehaviorReturnCode Behave(InsParam inputParam)
        {
            try
            {
                switch (_Action.Invoke(inputParam))
                {
                    case BehaviorReturnCode.Success:
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    case BehaviorReturnCode.Failure:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    case BehaviorReturnCode.Running:
                        ReturnCode = BehaviorReturnCode.Running;
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