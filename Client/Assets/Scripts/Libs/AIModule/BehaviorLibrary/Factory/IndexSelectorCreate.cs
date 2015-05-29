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
    public class IndexSelectorCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            IndexSelector indexSelector = new IndexSelector();
            return indexSelector;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}