using SDK.Common;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SDK.Lib
{
    public class LangMgr
    {
        protected LangID m_langID = LangID.zh_CN;           // ��ǰ���ԣ�Ĭ�ϼ�������
        protected XmlNodeList m_nodeList = null;                   // ������ xml �� <t> �б�
        protected Dictionary<LangID, LangAttrItem> m_ID2FileName = new Dictionary<LangID, LangAttrItem>();  // ���Ե��ļ����ֵ�ӳ��
        protected XmlNodeList m_tmpEleList;         // ��ʱ��Ԫ���б�
        protected XmlElement m_tmpEle;              // ��ʱ��Ԫ��
        protected bool m_isLoaded = false;                  // �����ļ��Ƿ����
        protected bool m_hasItem = false;

        public LangMgr()
        {
            m_ID2FileName[LangID.zh_CN] = new LangAttrItem();
            m_ID2FileName[LangID.zh_CN].m_prefabName = "zh_CN";
            m_ID2FileName[LangID.zh_CN].m_filePath = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathLangXml] + m_ID2FileName[LangID.zh_CN].m_prefabName;
        }

        public void getText(LangTypeId typeId, int itemIdx)
        {
            if (!m_isLoaded)
            {
                loadXml();
            }

            m_hasItem = false;

            if(null != m_nodeList)
            {
                if ((int)typeId < m_nodeList.Count)
                {
                    m_tmpEleList = m_nodeList[(int)typeId].ChildNodes as XmlNodeList;
                    if(itemIdx < m_tmpEleList.Count)
                    {
                        m_hasItem = true;
                        m_tmpEle = m_tmpEleList[itemIdx] as XmlElement;
                        Ctx.m_instance.m_shareData.m_retLangStr = m_tmpEle.InnerText;
                    }
                }
            }

            if (!m_hasItem)
            {
                Ctx.m_instance.m_shareData.m_retLangStr = "default string";
            }
        }

        //<?xml version="1.0" encoding="utf-8"?>
        //<!-- type ����һ������ item ����һ�˳��ǧ��Ҫ�ң���������  -->
        //<msg>
        //    <t>
        //        <i>���ݽṹ</i>
        //    </t>
        //</msg>
        public void loadXml()
        {
            m_isLoaded = true;
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            param.m_path = m_ID2FileName[m_langID].m_filePath;
            param.m_prefabName = m_ID2FileName[m_langID].m_prefabName;
            param.m_loaded = onloaded;
            param.m_extName = "xml";
            Ctx.m_instance.m_resLoadMgr.loadResources(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        // ����һ�������
        public void onloaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // ����ת��
            string text = res.getText(m_ID2FileName[m_langID].m_prefabName);
            if (text != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(text);

                XmlNode xn = xmlDoc.SelectSingleNode("msg");
                m_nodeList = xn.ChildNodes;
            }
        }
    }
}