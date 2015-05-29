using System.Collections.Generic;

namespace BehaviorLibrary
{
    /**
     * @brief 行为树属性系统
     */

    /**
     * @brief 行为树ID
     */
    public enum BTID
    {
        e1000,
    }

    /**
     * @brief 每一个行为树的基本属性
     */
    public class BTAttrItem
    {
        public string m_path;       // 行为树目录(包括名字和扩展名)
    }

    /**
     * @brief 行为树的基本属性
     */
    public class BTAttrSys
    {
        public Dictionary<BTID, BTAttrItem> m_id2ItemDic;

        public BTAttrSys()
        {
            m_id2ItemDic = new Dictionary<BTID, BTAttrItem>();
        }
    }
}