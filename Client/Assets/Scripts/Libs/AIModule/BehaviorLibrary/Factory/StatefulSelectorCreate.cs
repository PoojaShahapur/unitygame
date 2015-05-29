using BehaviorLibrary.Components;
using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class StatefulSelectorCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            StatefulSelector statefulSelector = new StatefulSelector();
            return statefulSelector;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}