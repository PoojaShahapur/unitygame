using SDK.Lib;
using System;
using System.IO;
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
            string name = "RandName.txt";
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathWord], name), param);
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
            IResItem m_res = resEvt as IResItem;                         // 类型转换
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, m_res.GetPath());

            string text = m_res.getText("");

            if (text != null)
            {
                string[] lineSplitStr = { "\r\n" };
                m_nameList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
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
    }
}