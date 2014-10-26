using System;
using System.Collections.Generic;
using System.Xml;

namespace SDK.Lib
{
    public class LangItem
    {
        public const string DefaultStr = "No Default Desc";
        public bool m_isInit;
        public List<string> m_strList;

        public void Init(XmlElement xml)
        {
            m_isInit = true;
            m_strList = new List<string>(xml.ChildNodes.Count);

            XmlElement xe;
            XmlNodeList xnl = xml.ChildNodes;
            int idx = 0;

            foreach (XmlNode xn1 in xnl)
            {
                xe = (XmlElement)xn1;
                //m_strList[Convert.ToInt32(xe.Name)] = xe.InnerText;
                m_strList[idx] = xe.InnerText;
                ++idx;
            }
        }

        public string GetText(int secIdx)
        {
            if(secIdx < m_strList.Count)
            {
                return m_strList[secIdx];
            }

            return DefaultStr;
        }
    }
}
