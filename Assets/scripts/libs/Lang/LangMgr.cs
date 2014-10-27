using System.Collections.Generic;
using System.Xml;
namespace SDK.Lib
{
    public class LangMgr
    {
        protected Dictionary<string, LangIdx> m_tag2ID = new Dictionary<string, LangIdx>();
        protected Dictionary<LangIdx, string> m_ID2Tag = new Dictionary<LangIdx, string>();

        protected LangItem[] m_arrArr = new LangItem[(int)LangIdx.eLIDTotal];
        protected XmlNodeList m_nodeList;

        public void RegisterTag()
        {
            m_tag2ID[LangTag.LTgScene] = LangIdx.eLIDScene;
            m_ID2Tag[LangIdx.eLIDScene] = LangTag.LTgScene;
        }

        //<?xml version="1.0" encoding="utf-8"?>
        //<msg>
	    //    <a>
		//        <0>数据结构</0>
	    //    </a>
        //</msg>
        public void Load()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"..\..\aaa.xml");

            //XmlElement xe;
            XmlNode xn = xmlDoc.SelectSingleNode("msg");
            XmlNodeList xnl = xn.ChildNodes;
            m_nodeList = xnl;

            //foreach (XmlNode xn1 in xnl)
            //{
            //    xe = (XmlElement)xn1;
            //    m_arrArr[(int)m_tag2ID[xe.Name]] = new LangItem();
            //    m_arrArr[(int)m_tag2ID[xe.Name]].Init(xe);
            //}
        }

        public void GetText(LangIdx idx, int secIdx)
        {
            if(m_arrArr[(int)idx] == null)      // 如果没有初始化
            {
                m_arrArr[(int)idx] = new LangItem();
                m_arrArr[(int)idx].Init(m_nodeList.Item((int)idx) as XmlElement);
            }

            m_arrArr[(int)idx].GetText(secIdx);
        }
    }
}