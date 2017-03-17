using System.Collections.Generic;
using System.IO;

namespace SDK.Lib
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
        private MDictionary<TableID, TableBase> mDicTable;
		private ResItem mRes;
        private ByteBuffer mByteArray;

		public TableSys()
		{
			mDicTable = new MDictionary<TableID, TableBase>();
            mDicTable[TableID.TABLE_OBJECT] = new TableBase("ObjectBase_client.bytes", "ObjectBase_client");
            mDicTable[TableID.TABLE_CARD] = new TableBase("CardBase_client.bytes", "CardBase_client");
            mDicTable[TableID.TABLE_SKILL] = new TableBase("SkillBase_client.bytes", "SkillBase_client");    // 添加一个表的步骤三
            mDicTable[TableID.TABLE_JOB] = new TableBase("proBase_client.bytes", "proBase_client");
            mDicTable[TableID.TABLE_SPRITEANI] = new TableBase("FrameAni_client.bytes", "FrameAni_client");
            mDicTable[TableID.TABLE_RACE] = new TableBase("RaceBase_client.bytes", "RaceBase_client");
            mDicTable[TableID.TABLE_STATE] = new TableBase("StateBase_client.bytes", "StateBase_client");
		}

        // 返回一个表
        public List<TableItemBase> getTable(TableID tableID)
		{
			TableBase table = mDicTable[tableID];
			if (null == table)
			{
				loadOneTable(tableID);
				table = mDicTable[tableID];
			}
			return table.mList;
		}
		
        // 返回一个表中一项，返回的时候表中数据全部加载到 Item 中
        public TableItemBase getItem(TableID tableID, uint itemID)
		{
            TableBase table = mDicTable[tableID];
            if (null == table.mByteBuffer)
			{
				loadOneTable(tableID);
				table = mDicTable[tableID];
			}
            TableItemBase ret = TableSys.findDataItem(table, itemID);

            if (null != ret && null == ret.mItemBody)
            {
                loadOneTableOneItemAll(tableID, table, ret);
            }

            if (null == ret)
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("table name: {0}, table Item {1} load failed", (int)tableID, itemID));
                }
            }

			return ret;
		}
		
        // 加载一个表
		public void loadOneTable(TableID tableID)
		{
			TableBase table = mDicTable[tableID];

            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(Path.Combine(Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathTablePath], table.mResName));
            param.mLoadEventHandle = onLoadEventHandle;
            param.mLoadNeedCoroutine = false;
            param.mResNeedCoroutine = false;
            Ctx.mInstance.mResLoadMgr.loadAsset(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
		}

        // 加载一个表完成
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            mRes = dispObj as ResItem;
            if (mRes.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, mRes.getLoadPath());

                byte[] bytes = mRes.getBytes("");
                if (bytes != null)
                {
                    mByteArray = Ctx.mInstance.mFactoryBuild.buildByteBuffer();
                    mByteArray.clear();
                    mByteArray.writeBytes(bytes, 0, (uint)bytes.Length);
                    mByteArray.setPos(0);
                    readTable(getTableIDByPath(mRes.getLogicPath()), mByteArray);
                }
            }
            else if (mRes.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem1, mRes.getLoadPath());
            }

            // 卸载资源
            Ctx.mInstance.mResLoadMgr.unload(mRes.getResUniqueId(), onLoadEventHandle);
        }

        // 根据路径查找表的 ID
        protected TableID getTableIDByPath(string path)
        {
            foreach (KeyValuePair<TableID, TableBase> kv in mDicTable.getData())
            {
                if (Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathTablePath] + kv.Value.mResName == path)
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
                itemBase.parseBodyByteBuffer<TableObjectItemBody>(table.mByteBuffer, itemBase.mItemHeader.mOffset);
            }
            else if (TableID.TABLE_CARD == tableID)
            {
                itemBase.parseBodyByteBuffer<TableCardItemBody>(table.mByteBuffer, itemBase.mItemHeader.mOffset);
            }
            else if (TableID.TABLE_SKILL == tableID)  // 添加一个表的步骤四
            {
                itemBase.parseBodyByteBuffer<TableSkillItemBody>(table.mByteBuffer, itemBase.mItemHeader.mOffset);
            }
            else if (TableID.TABLE_JOB == tableID)
            {
                itemBase.parseBodyByteBuffer<TableJobItemBody>(table.mByteBuffer, itemBase.mItemHeader.mOffset);
            }
            else if (TableID.TABLE_SPRITEANI == tableID)
            {
                itemBase.parseBodyByteBuffer<TableSpriteAniItemBody>(table.mByteBuffer, itemBase.mItemHeader.mOffset);
            }
            else if (TableID.TABLE_RACE == tableID)
            {
                itemBase.parseBodyByteBuffer<TableRaceItemBody>(table.mByteBuffer, itemBase.mItemHeader.mOffset);
            }
            else if (TableID.TABLE_STATE == tableID)
            {
                itemBase.parseBodyByteBuffer<TableStateItemBody>(table.mByteBuffer, itemBase.mItemHeader.mOffset);
            }
        }
		
        // 获取一个表的名字
		public string getTableName(TableID tableID)
		{
			TableBase table = mDicTable[tableID];
			if (null != table)
			{
				return table.mTableName;
			}			
			return "";
		}

        // 读取一个表，仅仅读取表头
        private void readTable(TableID tableID, ByteBuffer bytes)
        {
            TableBase table = mDicTable[tableID];
            table.mByteBuffer = bytes;

            bytes.setEndian(EEndian.eLITTLE_ENDIAN);
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
                table.mList.Add(item);
            }
        }

        // 查找表中的一项
        static public TableItemBase findDataItem(TableBase table, uint id)
		{
			int size = table.mList.Count;
			int low = 0;
			int high = size - 1;
			int middle = 0;
			uint idCur = 0;
			
			while (low <= high)
			{
				middle = (low + high) / 2;
                idCur = table.mList[middle].mItemHeader.mId;
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
                return table.mList[middle];
			}
			return null;
		}
	}
}