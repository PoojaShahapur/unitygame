using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 一类组件的创建
     */
    public class ComponentCreate
    {
        protected Dictionary<string, Func<BehaviorComponent>> m_id2CreateDic = new Dictionary<string, Func<BehaviorComponent>>();
        protected Dictionary<string, Action<BehaviorComponent, SecurityElement>> m_id2BuildDic = new Dictionary<string, Action<BehaviorComponent, SecurityElement>>();

        public BehaviorComponent createComp(string type)
        {
            if(string.IsNullOrEmpty(type))      // 若果这个是 "" ，就说明创建默认的
            {
                return createDefault();
            }
            else
            {
                return m_id2CreateDic[type]();
            }
        }

        public void buildComp(string type, BehaviorComponent comp, SecurityElement xmlElem)
        {
            if (string.IsNullOrEmpty(type))
            {
                buildDefault(comp, xmlElem);
            }
            else
            {
                m_id2BuildDic[type](comp, xmlElem);
            }
        }

        virtual protected BehaviorComponent createDefault()
        {
            return null;
        }

        virtual protected void buildDefault(BehaviorComponent comp, SecurityElement xmlElem)
        {
            
        }
    }
}