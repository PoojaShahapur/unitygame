using Mono.Xml;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class AttackActionNode : FightActionNodeBase
    {
        protected AttackEffectList m_attackEffectList;
        protected HurtActionNode m_hurtActionNode;

        override public void parseXmlElem(SecurityElement elem_)
        {
            base.parseXmlElem(elem_);

            // 解析攻击特效
            ArrayList attackEffectList = new ArrayList();   // 攻击者攻击特效
            UtilXml.getXmlChildList(elem_, "AttackEffect", ref attackEffectList);
            if (attackEffectList != null)
            {
                m_attackEffectList = new AttackEffectList();
                m_attackEffectList.parseXmlElemList(attackEffectList);
            }
        }
    }
}