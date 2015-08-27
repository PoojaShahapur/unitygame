using System.Collections;
using System.Security;

namespace SDK.Lib
{
    /**
     * @brief 攻击特效列表
     */
    public class AttackEffectList
    {
        protected MList<AttackEffectNode> m_attackEffectList;

        public void parseXmlElemList(ArrayList elemList_)
        {
            m_attackEffectList = new MList<AttackEffectNode>();
            AttackEffectNode attackEffectNode;

            foreach (SecurityElement hurtEffectNodeElem_ in elemList_)
            {
                attackEffectNode = new AttackEffectNode();
                m_attackEffectList.Add(attackEffectNode);
                attackEffectNode.parseXmlElem(hurtEffectNodeElem_);
            }
        }
    }
}