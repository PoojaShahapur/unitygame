using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class InverterCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            Inverter inverter = new Inverter();
            return inverter;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}