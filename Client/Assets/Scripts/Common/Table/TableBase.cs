﻿using System.Collections.Generic;

namespace SDK.Common
{
    //public class TableBase<T> where T : ItemBase, new()
    public class TableBase
    {
        public string m_resName;
        public string m_tableName;      // 表的名字

        public List<TableItemBase> m_List;
        public ByteBuffer m_byteArray;      // 整个表格所有的原始数据

        public TableBase(string resname, string tablename)
        {
            m_resName = resname;
            m_tableName = tablename;

            m_List = new List<TableItemBase>();
        }
    }
}