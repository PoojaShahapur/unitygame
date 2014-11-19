namespace BehaviorLibrary.Components.Conditionals
{
    /**
     * @brief 空闲条件
     */
    public class ConditionIdle : Condition
    {
        public ConditionIdle()
            : base(null)
        {
            boolFunc = onExecBoolFunc;
        }

        protected bool onExecBoolFunc(InsParam inputParam)
        {
            return true;
        }
    }
}