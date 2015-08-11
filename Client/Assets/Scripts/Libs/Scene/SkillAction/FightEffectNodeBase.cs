using SDK.Common;
using System.Security;

namespace SDK.Lib
{
    /**
     * @brief 战斗特效的基类
     */
    public class FightEffectNodeBase
    {
        protected string m_effectId;    // 特效 Id
        protected float m_delayTime;    // 延迟时间
        protected string m_linkBone;    // 连接的骨头

        virtual public void parseXmlElem(SecurityElement elem_)
        {
            UtilXml.getXmlAttrStr(elem_, "EffectId", ref m_effectId);
            UtilXml.getXmlAttrFloat(elem_, "DelayTime", ref m_delayTime);
            UtilXml.getXmlAttrStr(elem_, "LinkBone", ref m_linkBone);
        }
    }
}