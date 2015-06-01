using System;

namespace BehaviorLibrary
{
    public class RandomDecorator : SingleBranchComponent
    {
        private float _Probability;
        private Func<float> _RandomFunction;

        /// <summary>
        /// randomly executes the behavior
        /// </summary>
        /// <param name="probability">probability of execution</param>
        /// <param name="randomFunction">function that determines probability to execute</param>
        /// <param name="behavior">behavior to execute</param>
        public RandomDecorator(float probability, Func<float> randomFunction, BehaviorComponent behavior)
        {
            _Probability = probability;
            _RandomFunction = randomFunction;
            m_childBehavior = behavior;
        }


        public override BehaviorReturnCode Behave()
        {
            try
            {
                if (_RandomFunction.Invoke() <= _Probability)
                {
                    ReturnCode = m_childBehavior.Behave();
                    return ReturnCode;
                }
                else
                {
                    ReturnCode = BehaviorReturnCode.Running;
                    return BehaviorReturnCode.Running;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.Error.WriteLine(e.ToString());
#endif
                ReturnCode = BehaviorReturnCode.Failure;
                return BehaviorReturnCode.Failure;
            }
        }
    }
}