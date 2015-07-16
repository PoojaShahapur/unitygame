using SDK.Common;
using System.Security;

namespace SDK.Lib
{
    /**
     * @brief 战斗动作节点基类
     */
    public class FightActionNodeBase
    {
        protected int m_HashId;         // 动作的 Id
        protected bool m_needMove;      // 攻击是否需要移动

        virtual public void parseXmlElem(SecurityElement elem_)
        {
            m_HashId = UtilXml.getXmlAttrInt(elem_, "HashId");
            m_needMove = UtilXml.getXmlAttrBool(elem_, "NeedMove");
        }
    }
}