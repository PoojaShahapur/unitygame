using SDK.Common;
using SDK.Lib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 添加一个表的步骤总共分 4 步
     * // 添加一个表的步骤一
     * // 添加一个表的步骤二
     * // 添加一个表的步骤三
     * // 添加一个表的步骤四
     */
    public class TableSys
	{
        private Dictionary<TableID, TableBase> m_dicTable;
		private IResItem m_res;
        private ByteBuffer m_byteArray;

		public TableSys()
		{
			m_dicTable = new Dictionary<TableID, TableBase>();
            m_dicTable[TableID.TABLE_OBJECT] = new TableBase("ObjectBase_client.txt", "ObjectBase_client");
            m_dicTable[TableID.TABLE_CARD] = new TableBase("CardBase_client.txt", "CardBase_client");
            m_dicTable[TableID.TABLE_SKILL] = new TableBase("SkillBase_client.txt", "SkillBase_client");    // 添加一个表的步骤三
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

            if (null != ret && null == ret.m_itemBody)
            {
                loadOneTableOneItemAll(tableID, table, ret);
            }

            if (ret == null)
            {
                Ctx.m_instance.m_logSys.log(string.Format("table name: {0}, table Item {1} 加载失败", (int)tableID, itemID));
            }

			return ret;
		}
		
        // 加载一个表
		public void loadOneTable(TableID tableID)
		{
			TableBase table = m_dicTable[tableID];

            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_pPakSys.getCurResPakPathByResPath(Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathTablePath], table.m_resName));
            param.m_loaded = onLoaded;
            param.m_failed = onFailed;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
		}

        // 加载一个表完成
        public void onLoaded(IDispatchObject resEvt)
        {
            m_res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, m_res.GetPath());

            byte[] bytes = m_res.getBytes("");
            if (bytes != null)
            {
                m_byteArray = Ctx.m_instance.m_factoryBuild.buildByteBuffer();
                m_byteArray.clear();
                m_byteArray.writeBytes(bytes, 0, (uint)bytes.Length);
                m_byteArray.setPos(0);
                readTable(getTableIDByPath(m_res.GetPath()), m_byteArray);
            }

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());
        }

        public void onFailed(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, m_res.GetPath());

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());
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
        public void loadOneTableOneItemAll(TableID tableID, TableBase table, TableItemBase itemBase)
        {
            if (TableID.TABLE_OBJECT == tableID)
            {
                itemBase.parseBodyByteBuffer<TableObjectItemBody>(table.m_byteArray, itemBase.m_itemHeader.m_offset);
            }
            else if (TableID.TABLE_CARD == tableID)
            {
                itemBase.parseBodyByteBuffer<TableCardItemBody>(table.m_byteArray, itemBase.m_itemHeader.m_offset);
            }
            else if (TableID.TABLE_SKILL == tableID)  // 添加一个表的步骤四
            {
                itemBase.parseBodyByteBuffer<TableSkillItemBody>(table.m_byteArray, itemBase.m_itemHeader.m_offset);
            }
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
        private void readTable(TableID tableID, ByteBuffer bytes)
        {
            TableBase table = m_dicTable[tableID];
            table.m_byteArray = bytes;

            bytes.setEndian(Endian.LITTLE_ENDIAN);
            uint len = 0;
            bytes.readUnsignedInt32(ref len);
            uint i = 0;
            TableItemBase item = null;
            for (i = 0; i < len; i++)
            {
                //if (TableID.TABLE_OBJECT == tableID)
                //{
                //    item = new TableItemObject();
                //}
                item = new TableItemBase();
                item.parseHeaderByteBuffer(bytes);
                // 加载完整数据
                //loadOneTableOneItemAll(tableID, table, item);
                //if (TableID.TABLE_OBJECT == tableID)
                //{
                    //item.parseAllByteBuffer<TableObjectItemBody>(bytes);
                //}
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