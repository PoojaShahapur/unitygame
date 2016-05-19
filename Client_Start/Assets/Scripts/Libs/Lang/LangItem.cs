using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace SDK.Lib
{
    public class LangItem
    {
        public const string DefaultStr = "No Default Desc";
        public bool m_isInit;
        public List<string> m_strList;

        public void Init(SecurityElement xml)
        {
            m_isInit = true;
            m_strList = new List<string>(xml.Children.Count);

            SecurityElement xe;
            ArrayList xnl = xml.Children;
            int idx = 0;

            foreach (SecurityElement xn1 in xnl)
            {
                xe = xn1;
                //m_strList[Convert.ToInt32(xe.Name)] = xe.InnerText;
                m_strList[idx] = xe.Text;
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