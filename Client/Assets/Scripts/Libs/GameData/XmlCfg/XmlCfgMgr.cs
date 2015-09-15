using System.Collections.Generic;

namespace SDK.Lib
{
    public class XmlCfgMgr
    {
        public Dictionary<XmlCfgID, XmlCfgBase> m_id2CfgDic = new Dictionary<XmlCfgID,XmlCfgBase>();        // 商城
        private ResItem m_res;

        protected void loadCfg<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            T item = new T();
            m_id2CfgDic[id] = item;

            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(item.m_path, param);
            param.m_loadEventHandle = onLoadEventHandle;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            m_res = dispObj as ResItem;
            if (m_res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, m_res.GetPath());

                string text = m_res.getText("");
                if (text != null)
                {
                    m_id2CfgDic[getXmlCfgIDByPath(m_res.GetPath())].parseXml(text);
                }
            }
            else if (m_res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, m_res.GetPath());
            }

            Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath(), onLoadEventHandle);
        }

        protected XmlCfgID getXmlCfgIDByPath(string path)
        {
            foreach (KeyValuePair<XmlCfgID, XmlCfgBase> kv in m_id2CfgDic)
            {
                if (kv.Value.m_path == path)
                {
                    return kv.Key;
                }
            }

            return 0;
        }

        public T getXmlCfg<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            if (!m_id2CfgDic.ContainsKey(id))
            {
                loadCfg<T>(id);
            }

            return m_id2CfgDic[id] as T;
        }
    }
}