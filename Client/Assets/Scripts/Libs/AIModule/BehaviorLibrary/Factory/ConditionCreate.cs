using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class ConditionCreate : ComponentCreate
    {
        public ConditionCreate()
        {
            regHandle();
        }

        protected void regHandle()
        {
            m_id2CreateDic["Idle"] = createConditionIdle;
            m_id2BuildDic["Idle"] = buildConditionIdle;
        }

        override protected BehaviorComponent createDefault()
        {
            Condition condition = new Condition(null);
            return condition;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }

        public BehaviorComponent createConditionIdle()
        {
            ConditionIdle conditionIdle = new ConditionIdle();
            return conditionIdle;
        }

        public void buildConditionIdle(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}