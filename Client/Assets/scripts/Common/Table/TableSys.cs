using SDK.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    public class TableSys : ITableSys
	{
        private Dictionary<TableID, TableBase> m_dicTable;
		private IResItem m_res;
        private IByteArray m_byteArray;

		public TableSys()
		{
            m_byteArray = Ctx.m_instance.m_factoryBuild.buildByteArray();

			m_dicTable = new Dictionary<TableID, TableBase>();
            m_dicTable[TableID.TABLE_OBJECT] = new TableBase("ObjectBase_client", "ObjectBase_client", "ObjectBase_client");
		}

        // 返回一个表
        public List<TableItemBase> getTable(TableID tableID)
		{
			TableBase table = m_dicTable[tableID];
			if (table == null)
			{
				loadOneTable(tableID);
				table = m_dicTable[tableID];
			}
			return table.m_List;
		}
		
        // 返回一个表中一项，返回的时候表中数据全部加载到 Item 中
        public TableItemBase getItem(TableID tableID, uint itemID)
		{
            TableBase table = m_dicTable[tableID];
            if (null == table.m_byteArray)
			{
				loadOneTable(tableID);
				table = m_dicTable[tableID];
			}
            TableItemBase ret = TableSys.findDataItem(table, itemID);
            if(null == ret.m_itemBody)
            {
                loadOneTableOneItemAll(table, ret);
            }
			return ret;
		}
		
        // 加载一个表
		public void loadOneTable(TableID tableID)
		{
			TableBase table = m_dicTable[tableID];

            LoadParam param = Ctx.m_instance.m_resMgr.getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathTablePath] + table.m_resName;
            param.m_prefabName = table.m_prefabName;
            param.m_loaded = onloaded;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.m_instance.m_resMgr.loadResources(param);
            //TextAsset textAsset = Resources.Load(param.m_path, typeof(TextAsset)) as TextAsset;
		}

        // 加载一个表完成
        public void onloaded(IDispatchObject resEvt)
        {
            m_res = resEvt as IResItem;                         // 类型转换
            TextAsset textAsset = m_res.getObject("") as TextAsset;
            if (textAsset != null)
            {
                m_byteArray.clear();
                m_byteArray.writeBytes(textAsset.bytes, 0, (uint)textAsset.bytes.Length);
                m_byteArray.setPos(0);
                readTable(getTableIDByPath(m_res.GetPath()), m_byteArray);
            }
        }

        // 根据路径查找表的 ID
        protected TableID getTableIDByPath(string path)
        {
            foreach (KeyValuePair<TableID, TableBase> kv in m_dicTable)
            {
                if (Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathTablePath] + kv.Value.m_resName == path)
                {
                    return kv.Key;
                }
            }

            return 0;
        }

        // 加载一个表中一项的所有内容
        public void loadOneTableOneItemAll(TableBase table, TableItemBase itemBase)
        {
            itemBase.parseBodyByteArray(table.m_byteArray, itemBase.m_itemHeader.m_offset);
        }
		
        // 获取一个表的名字
		public string getTableName(TableID tableID)
		{
			TableBase table = m_dicTable[tableID];
			if (table != null)
			{
				return table.m_tableName;
			}			
			return "";
		}

        // 读取一个表，仅仅读取表头
        private void readTable(TableID tableID, IByteArray bytes)
        {
            TableBase table = m_dicTable[tableID];
            bytes.setEndian(Endian.LITTLE_ENDIAN);
            uint len = bytes.readUnsignedInt();
            uint i = 0;
            TableItemBase item = null;
            for (i = 0; i < len; i++)
            {
                if (TableID.TABLE_OBJECT == tableID)
                {
                    item = new TableItemObject();
                }
                item.parseHeaderByteArray(bytes);
                //item.parseAllByteArray(bytes);
                table.m_List.Add(item);
            }
        }

        // 查找表中的一项
        static public TableItemBase findDataItem(TableBase table, uint id)
		{
			int size = table.m_List.Count;
			int low = 0;
			int high = size - 1;
			int middle = 0;
			uint idCur;
			
			while (low <= high)
			{
				middle = (low + high) / 2;
                idCur = table.m_List[middle].m_itemHeader.m_uID;
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