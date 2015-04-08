﻿using SDK.Lib;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SDK.Common
{
    public class XmlCfgMgr
    {
        public Dictionary<XmlCfgID, XmlCfgBase> m_id2CfgDic = new Dictionary<XmlCfgID,XmlCfgBase>();        // 商城
        private IResItem m_res;

        protected void loadCfg<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            T item = new T();
            m_id2CfgDic[id] = item;

            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = item.m_path;
            param.m_prefabName = item.m_prefabName;
            param.m_loaded = onloaded;
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            param.m_extName = "xml";
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onloaded(IDispatchObject resEvt)
        {
            m_res = resEvt as IResItem;                         // 类型转换
            string text = m_res.getText("");
            if (text != null)
            {
                m_id2CfgDic[getXmlCfgIDByPath(m_res.GetPath())].parseXml(text);
            }
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

        public XmlCfgBase getXmlCfg(XmlCfgID id)
        {
            if(XmlCfgID.eXmlMarketCfg == id)
            {
                if (!m_id2CfgDic.ContainsKey(id))
                {
                    loadCfg<XmlMarketCfg>(id);
                }
            }

            return m_id2CfgDic[id];
        }
    }
}