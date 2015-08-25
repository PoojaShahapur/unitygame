using SDK.Lib;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class AttackActionItem
    {
        protected string m_id;
        protected AttackActionNode m_attackActionNode;
        protected HurtActionNode m_hurtActionNode;

        public AttackActionNode attackActionNode
        {
            get
            {
                return m_attackActionNode;
            }
            set
            {
                m_attackActionNode = value;
            }
        }

        public HurtActionNode hurtActionNode
        {
            get
            {
                return m_hurtActionNode;
            }
            set
            {
                m_hurtActionNode = value;
            }
        }

        public void parseXmlElem(SecurityElement elem_)
        {
            // 解析 Id
            UtilXml.getXmlAttrStr(elem_, "Id", ref m_id);

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