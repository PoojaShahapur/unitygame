using BehaviorLibrary.Components;
using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class StatefulSequenceCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            StatefulSequence statefulSequence = new StatefulSequence();
            return statefulSequence;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}