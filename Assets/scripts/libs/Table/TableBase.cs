using System.Collections.Generic;

namespace SDK.Lib
{
    //public class TableBase<T> where T : ItemBase, new()
    public class TableBase
    {
        public string m_resName;
        public string m_tableName;
        public List<ItemBase> m_List;

        public TableBase(string resname, string tablename)
        {
            m_resName = resname;
            m_tableName = tablename;
            m_List = new List<ItemBase>();
        }
    }
}
