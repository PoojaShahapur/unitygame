using Mono.Xml;
using SDK.Common;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class AttackActionNode : FightActionNodeBase
    {
        protected AttackEffectList m_attackEffectList;
        protected HurtActionNode m_hurtActionNode;

        public void parseXml(string str)
        {
            SecurityParser _xmlDoc = new SecurityParser();
            _xmlDoc.LoadXml(str);

            SecurityElement rootNode = _xmlDoc.ToXml();         // Config 节点
            // 解析攻击
            ArrayList attackActionNodeList = rootNode.Children; // AttackAction 攻击动作节点，这个节点只有一个 AttackAction
            SecurityElement attackActionNode = attackActionNodeList[0] as SecurityElement;
            parseXmlElem(attackActionNode);

            // 解析攻击特效
            ArrayList attackEffectList = new ArrayList();   // 攻击者攻击特效
            UtilXml.getXmlChildList(attackActionNode, "AttackEffect", ref attackEffectList);
            if (attackEffectList != null)
            {
                m_attackEffectList = new AttackEffectList();
                m_attackEffectList.parseXmlElemList(attackEffectList);
            }

            // 解析被击
            SecurityElement hurtActionNode = null;
            UtilXml.getXmlChild(attackActionNode, "HurtAction", ref hurtActionNode);
            if (hurtActionNode != null)
            {
                m_hurtActionNode = new HurtActionNode();
                m_hurtActionNode.parseXmlElem(hurtActionNode);
            }
        }

        override public void parseXmlElem(SecurityElement elem_)
        {
            base.parseXmlElem(elem_);
        }
    }
}