using System.Collections.Generic;

namespace SDK.Common
{
    public class TableBase
    {
        public string m_resName;
        public string m_tableName;      // 表的名字
        public string m_prefabName;     // prefab 名字
        public List<ItemBase> m_List;

        public TableBase(string resname, string tablename, string prefabname)
        {
            m_resName = resname;
            m_tableName = tablename;
            m_prefabName = prefabname;

            m_List = new List<ItemBase>();
        }
    }
}