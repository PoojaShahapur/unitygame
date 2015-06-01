using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class CounterCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            Counter counter = new Counter(0, null);
            return counter;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}