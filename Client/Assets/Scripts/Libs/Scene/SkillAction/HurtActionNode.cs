using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class HurtActionNode : FightActionNodeBase
    {
        protected float m_delayTime;    // 延迟播放时间
        protected HurtEffectList m_hurtEffectList;

        override public void parseXmlElem(SecurityElement elem_)
        {
            base.parseXmlElem(elem_);

            ArrayList hurtEffectNodeList = elem_.Children; // AttackAction 攻击动作节点，这个节点只有一个 AttackAction
            if(hurtEffectNodeList != null)
            {
                m_hurtEffectList = new HurtEffectList();
            }
        }
    }
}