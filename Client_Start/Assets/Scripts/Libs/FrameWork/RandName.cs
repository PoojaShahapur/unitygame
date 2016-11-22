using System;
using System.IO;

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
            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(Path.Combine(Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathWord], name));
            param.m_loadEventHandle = onLoadEventHandle;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.mInstance.mResLoadMgr.loadAsset(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.getLoadPath());

                string text = res.getText("");

                if (text != null)
                {
                    string[] lineSplitStr = { "\r\n" };
                    m_nameList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem1, res.getLoadPath());
            }

            // 卸载资源
            Ctx.mInstance.mResLoadMgr.unload(res.getLoadPath(), onLoadEventHandle);
        }
    }
}