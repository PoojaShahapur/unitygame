using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class SelectorCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            Selector selector = new Selector();
            return selector;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}