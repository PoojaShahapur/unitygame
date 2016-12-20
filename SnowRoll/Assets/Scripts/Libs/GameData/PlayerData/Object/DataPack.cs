using Game.Msg;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 背包中的数据
     */
    public class DataPack
    {
        public List<DataItemObjectBase> m_objList = new List<DataItemObjectBase>();             // 道具列表
        public Dictionary<uint, DataItemObjectBase> mId2ObjDic = new Dictionary<uint, DataItemObjectBase>();       // 道具 thidid到 Obj 字典，加快查找

        // 添加测试数据
        public void postConstruct()
        {
            // test 数据
            //addObjectByTableID(710);
            //addObjectByTableID(712);
        }

        public void addObjectByTableID(uint tableid)
        {
            DataItemObjectBase item = new DataItemObjectBase();
            m_objList.Add(item);
            mId2ObjDic[tableid] = item;

            item.m_tableItemObject = Ctx.mInstance.mTableSys.getItem(TableID.TABLE_OBJECT, tableid) as TableItemBase;
        }

        // 添加道具列表
        public void psstAddMobileObjectListPropertyUserCmd(List<stObjectOperator> list)
        {
            clearPack();

            t_Object_mobile obj;

            foreach (stObjectOperator opt in list)
            {
                obj = opt.mobject;
                DataItemObjectBase item = new DataItemObjectBase();
                item.m_srvItemObject = obj;
                item.m_tableItemObject = Ctx.mInstance.mTableSys.getItem(TableID.TABLE_OBJECT, item.m_srvItemObject.dwObjectID) as TableItemBase;
                m_objList.Add(item);
                mId2ObjDic[item.m_srvItemObject.dwThisID] = item;
            }
        }

        public void psstAddMobileObjectPropertyUserCmd(t_Object_mobile obj)
        {
            DataItemObjectBase item = new DataItemObjectBase();
            item.m_srvItemObject = obj;
            item.m_tableItemObject = Ctx.mInstance.mTableSys.getItem(TableID.TABLE_OBJECT, item.m_srvItemObject.dwObjectID) as TableItemBase;
            m_objList.Add(item);
            mId2ObjDic[item.m_srvItemObject.dwThisID] = item;
        }

        // 删除一个道具
        public void psstRemoveObjectPropertyUserCmd(uint qwThisID)
        {
            m_objList.Remove(mId2ObjDic[qwThisID]);
            mId2ObjDic.Remove(qwThisID);
        }

        // 修改道具数量
        public void psstRefCountObjectPropertyUserCmd(uint qwThisID, uint dwNum, byte type)
        {
            mId2ObjDic[qwThisID].m_srvItemObject.dwNum = dwNum;
        }

        protected void clearPack()
        {
            m_objList.Clear();
            mId2ObjDic.Clear();
        }
    }
}