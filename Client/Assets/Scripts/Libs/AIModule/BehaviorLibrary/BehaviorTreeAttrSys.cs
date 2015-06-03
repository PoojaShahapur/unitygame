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
        eNone,      // 默认值
        e1000,
    }

    /**
     * @brief 每一个行为树的基本属性
     */
    public class BTAttrItem
    {
        public BTID m_id;           // 行为树 id ，查找使用
        public string m_name;       // 名字
        public string m_path;       // 行为树所在的资源目录(包括名字和扩展名)
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

        public BTID getBTIDByName(string name)
        {
            foreach(KeyValuePair<BTID, BTAttrItem> keyVal in m_id2ItemDic)
            {
                if(keyVal.Value.m_name == name)
                {
                    return keyVal.Key;
                }
            }

            return BTID.eNone;
        }
    }
}