using SDK.Common;
using System.Collections;
using System.Collections.Generic;

namespace SDK.Common
{
    public class TableSys
	{
        private Dictionary<TableID, TableBase> m_dicTable;

		public TableSys()
		{
			m_dicTable = new Dictionary<TableID, TableBase>();
            m_dicTable[TableID.TABLE_OBJECT] = new TableBase("base", "base", "base");
		}

        public List<ItemBase> getTable(TableID tableID)
		{
			TableBase table = m_dicTable[tableID];
			if (table == null)
			{
				loadOneTable(tableID);
				table = m_dicTable[tableID];
			}
			return table.m_List;
		}
		
		public ItemBase getItem(TableID tableID, uint itemID)
		{
            TableBase table = m_dicTable[tableID];
			if (table == null)
			{
				loadOneTable(tableID);
				table = m_dicTable[tableID];
			}
			ItemBase ret = TableSys.findDataItem(table, itemID);
			return ret;
		}
		
		public void loadOneTable(TableID tableID)
		{			
			TableBase table = m_dicTable[tableID];
            byte[] bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(Ctx.m_instance.m_cfg.m_path, table.m_resName + ".txt");
            ByteArray byteArray = new ByteArray();
            byteArray.writeBytes(bytes, 0, (uint)bytes.Length);
            byteArray.position = 0;
            readTable(TableID.TABLE_OBJECT, byteArray);
		}
		
		// 与getItem不同的方面在于：如果找到对应的项，不报错
		public ItemBase getItemEx(TableID tableID, uint itemID)
		{
			TableBase table = m_dicTable[tableID];
			if (table == null)
			{
				loadOneTable(tableID);
				table = m_dicTable[tableID];
			}
			
			ItemBase ret = TableSys.findDataItem(table, itemID);
			return ret;
		}
		
		public string getTableName(TableID tableID)
		{
			TableBase table = m_dicTable[tableID];
			if (table != null)
			{
				return table.m_tableName;
			}			
			return "";
		}

        private void readTable(TableID tableID, ByteArray bytes)
        {
            TableBase table = m_dicTable[tableID];
            bytes.endian = Endian.LITTLE_ENDIAN;
            uint len = bytes.readUnsignedInt();
            uint i = 0;
            ItemBase item = new ItemBase();

            for (i = 0; i < len; i++)
            {
                if (TableID.TABLE_OBJECT == tableID)
                {
                    item = new ObjectItem();
                }
                item.parseHeaderByteArray(bytes);
                //item.parseByteArrayTestServer(bytes);
                table.m_List.Add(item);
            }
        }

		static public ItemBase findDataItem(TableBase table, uint id)
		{
			int size = table.m_List.Count;
			int low = 0;
			int high = size - 1;
			int middle = 0;
			uint idCur;
			
			while (low <= high)
			{
				middle = (low + high) / 2;
                idCur = table.m_List[middle].m_uID;
				if (idCur == id)
				{
					break;
				}
				if (id < idCur)
				{
					high = middle - 1;
				}
				else
				{
					low = middle + 1;
				}
			}
			
			if (low <= high)
			{
                return table.m_List[middle];
			}
			return null;
		}
	}
}