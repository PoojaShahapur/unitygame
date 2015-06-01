using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class PartialSequenceCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            PartialSequence partialSequence = new PartialSequence();
            return partialSequence;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}