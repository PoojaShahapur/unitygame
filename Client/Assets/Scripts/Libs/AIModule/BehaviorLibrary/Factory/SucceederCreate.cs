using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Decorators;
using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 一类组件的创建
     */
    public class SucceederCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            Succeeder selector = new Succeeder(null);
            return selector;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}