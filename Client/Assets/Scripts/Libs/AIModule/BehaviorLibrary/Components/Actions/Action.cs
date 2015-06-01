using System;

namespace BehaviorLibrary
{
    public class Action : LeafComponent
    {
        private Func<BehaviorReturnCode> _Action;

        public Action() { }

        public Action(Func<BehaviorReturnCode> action)
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

        protected BehaviorReturnCode DefaultAction()
        {
            return BehaviorReturnCode.Success;
        }

        public Func<BehaviorReturnCode> actionFunc
        {
            set
            {
                _Action = value;
            }
        }

        public override BehaviorReturnCode Behave()
        {
            try
            {
                switch (_Action.Invoke())
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