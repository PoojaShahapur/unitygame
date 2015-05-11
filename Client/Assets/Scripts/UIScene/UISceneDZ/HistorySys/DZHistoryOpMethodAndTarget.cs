using System.Collections.Generic;

namespace Game.UI
{
    /**
     * @brief 操作方法
     */
    public enum DZOpMethod
    {

    }

    /**
     * @brief 操作方式和操作目标
     */
    public class DZHistoryOpMethodAndTarget
    {
        protected DZOpMethod m_DZOpMethod;              // 攻击方法
        protected List<DZDefHistoryCard> m_defList;     // 被击目标
    }
}