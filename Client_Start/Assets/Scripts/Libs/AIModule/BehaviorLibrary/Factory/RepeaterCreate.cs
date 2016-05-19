﻿using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 一类组件的创建
     */
    public class RepeaterCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            Repeater repeater = new Repeater(null);
            return repeater;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}