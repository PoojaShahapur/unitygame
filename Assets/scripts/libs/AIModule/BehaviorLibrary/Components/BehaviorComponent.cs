using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components
{
    public abstract class BehaviorComponent
    {
        protected BehaviorReturnCode ReturnCode;

        public BehaviorComponent() { }

        // 第一次进入调用
        public virtual void init()
        {

        }

        // 添加子节点
        public virtual void addChild(BehaviorComponent child)
        {

        }
        public abstract BehaviorReturnCode Behave(InsParam inputParam);

        // 退出的时候调用
        public virtual void exit()
        {

        }
    }
}