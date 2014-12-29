﻿using Game.Msg;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 背包中的数据
     */
    public class DataPack
    {
        public List<DataItemObjectBase> m_objList = new List<DataItemObjectBase>();             // 道具列表
        public Dictionary<uint, DataItemObjectBase> m_id2ObjDic = new Dictionary<uint, DataItemObjectBase>();       // 道具 thidid到 Obj 字典，加快查找

        // 添加道具列表
        public void psstAddMobileObjectListPropertyUserCmd(List<stObjectOperator> list)
        {
            t_Object_mobile obj;

            foreach (stObjectOperator opt in list)
            {
                obj = opt.mobject;
                DataItemObjectBase item = new DataItemObjectBase();
                item.m_srvItemObject = obj;
                item.m_tableItemObject = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_OBJECT, item.m_srvItemObject.dwObjectID) as TableItemObject;
                m_id2ObjDic[item.m_srvItemObject.dwObjectID] = item;
            }
        }

        public void psstAddMobileObjectPropertyUserCmd(t_Object_mobile obj)
        {
            DataItemObjectBase item = new DataItemObjectBase();
            item.m_srvItemObject = obj;
            item.m_tableItemObject = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_OBJECT, item.m_srvItemObject.dwObjectID) as TableItemObject;
            m_id2ObjDic[item.m_srvItemObject.dwObjectID] = item;
        }

        // 删除一个道具
        public void psstRemoveObjectPropertyUserCmd(uint qwThisID)
        {
            m_objList.Remove(m_id2ObjDic[qwThisID]);
            m_id2ObjDic.Remove(qwThisID);
        }

        // 修改道具数量
        public void psstRefCountObjectPropertyUserCmd(uint qwThisID, uint dwNum, byte type)
        {
            m_id2ObjDic[qwThisID].m_srvItemObject.dwNum = dwNum;
        }
    }
}