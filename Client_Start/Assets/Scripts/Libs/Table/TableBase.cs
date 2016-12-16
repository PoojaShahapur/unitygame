using System.Collections.Generic;

namespace SDK.Lib
{
    //public class TableBase<T> where T : ItemBase, new()
    public class TableBase
    {
        public string mResName;
        public string mTableName;      // 表的名字

        public List<TableItemBase> mList;
        public ByteBuffer mByteBuffer;      // 整个表格所有的原始数据

        public TableBase(string resName, string tablename)
        {
            mResName = resName;
            mTableName = tablename;

            mList = new List<TableItemBase>();
        }
    }
}