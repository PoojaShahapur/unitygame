using System.Collections.Generic;

namespace SDK.Lib
{
    public class XmlCfgMgr
    {
        public Dictionary<XmlCfgID, XmlCfgBase> mId2CfgDic = new Dictionary<XmlCfgID,XmlCfgBase>();        // 商城
        private ResItem m_res;

        protected void loadCfg<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            T item = new T();
            mId2CfgDic[id] = item;

            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(item.mPath);
            param.mLoadEventHandle = onLoadEventHandle;
            param.mLoadNeedCoroutine = false;
            param.mResNeedCoroutine = false;
            Ctx.mInstance.mResLoadMgr.loadAsset(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            m_res = dispObj as ResItem;
            if (m_res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, m_res.getLoadPath());

                string text = m_res.getText("");
                if (text != null)
                {
                    mId2CfgDic[getXmlCfgIDByPath(m_res.getLogicPath())].parseXml(text);
                }
            }
            else if (m_res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem1, m_res.getLoadPath());
            }

            Ctx.mInstance.mResLoadMgr.unload(m_res.getResUniqueId(), onLoadEventHandle);
        }

        protected XmlCfgID getXmlCfgIDByPath(string path)
        {
            foreach (KeyValuePair<XmlCfgID, XmlCfgBase> kv in mId2CfgDic)
            {
                if (kv.Value.mPath == path)
                {
                    return kv.Key;
                }
            }

            return 0;
        }

        public T getXmlCfg<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            if (!mId2CfgDic.ContainsKey(id))
            {
                loadCfg<T>(id);
            }

            return mId2CfgDic[id] as T;
        }
    }
}