using SDK.Lib;
using System;
using System.IO;
using UnityEngine;

namespace SDK.Lib
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
            param.m_loadEventHandle = onLoadEventHandle;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, res.GetPath());

                string text = res.getText("");

                if (text != null)
                {
                    string[] lineSplitStr = { "\r\n" };
                    m_nameList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, res.GetPath());
            }

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), onLoadEventHandle);
        }
    }
}