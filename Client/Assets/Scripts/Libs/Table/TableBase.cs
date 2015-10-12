using System.Collections.Generic;

namespace SDK.Lib
{
    //public class TableBase<T> where T : ItemBase, new()
    public class TableBase
    {
        public string m_resName;
        public string m_tableName;      // 表的名字

        public List<TableItemBase> m_List;
        public ByteBuffer m_byteBuffer;      // 整个表格所有的原始数据

        public TableBase(string resName, string tablename)
        {
            m_resName = resName;
            m_tableName = tablename;

            m_List = new List<TableItemBase>();
        }
    }
}