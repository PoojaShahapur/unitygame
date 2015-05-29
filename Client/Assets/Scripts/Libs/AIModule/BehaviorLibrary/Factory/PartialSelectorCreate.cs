using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Composites;
using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class PartialSelectorCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            PartialSelector partialSelector = new PartialSelector();
            return partialSelector;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}