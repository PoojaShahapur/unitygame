using SDK.Common;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class AttackActionItem
    {
        protected AttackActionNode m_attackActionNode;
        protected HurtActionNode m_hurtActionNode;

        public void parseXmlElem(SecurityElement elem_)
        {
            // 解析攻击
            ArrayList attackActionNodeList = elem_.Children; // AttackAction 攻击动作节点，这个节点只有一个 AttackAction
            SecurityElement attackActionNode = attackActionNodeList[0] as SecurityElement;
            m_attackActionNode = new AttackActionNode();
            m_attackActionNode.parseXmlElem(attackActionNode);

            // 解析被击
            SecurityElement hurtActionNode = null;
            UtilXml.getXmlChild(attackActionNode, "HurtAction", ref hurtActionNode);
            if (hurtActionNode != null)
            {
                m_hurtActionNode = new HurtActionNode();
                m_hurtActionNode.parseXmlElem(hurtActionNode);
            }
        }
    }
}