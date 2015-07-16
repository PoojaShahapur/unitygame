using SDK.Common;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class HurtEffectList
    {
        protected MList<HurtEffectNode> m_hurtEffectList;

        public void parseXmlElemList(ArrayList elemList_)
        {
            m_hurtEffectList = new MList<HurtEffectNode>();
            HurtEffectNode hurtEffectNode;

            foreach (SecurityElement hurtEffectNodeElem_ in elemList_)
            {
                hurtEffectNode = new HurtEffectNode();
                m_hurtEffectList.Add(hurtEffectNode);
                hurtEffectNode.parseXmlElem(hurtEffectNodeElem_);
            }
        }
    }
}