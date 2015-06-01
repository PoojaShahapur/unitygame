using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class SequenceCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            Sequence sequence = new Sequence();
            return sequence;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}