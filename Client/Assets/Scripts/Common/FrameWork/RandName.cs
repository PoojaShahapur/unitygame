using SDK.Lib;
using System;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 随机名字
     */
    public class RandName
    {
        protected string[] m_nameList;

        public string getRandName()
        {
            if (null == m_nameList)
            {
                loadRandNameTable();
            }

            int rand = UtilApi.Range(0, m_nameList.Length - 1);
            return m_nameList[rand];
        }
        
        protected void loadRandNameTable()
        {
            string name = "RandName";
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathWord] + name;
            param.m_prefabName = name;
            param.m_loaded = onloaded;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            param.m_extName = "txt";
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onloaded(IDispatchObject resEvt)
        {
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            string text = m_res.getText("");

            if (text != null)
            {
                string[] lineSplitStr = { "\r\n" };
                m_nameList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}