using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace EditorTool
{
    public class XmlParams
    {
        protected List<XmlParam> m_paramList = new List<XmlParam>();
        protected AnimatorControllerParameter[] m_parameters;

        public List<XmlParam> paramList
        {
            get
            {
                return m_paramList;
            }
            set
            {
                m_paramList = value;
            }
        }

        public AnimatorControllerParameter[] parameters
        {
            get
            {
                return m_parameters;
            }
            set
            {
                m_parameters = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            XmlNodeList paramsNodeList = elem.ChildNodes;
            XmlElement paramElem = null;
            XmlParam param;
            foreach (XmlNode paramNode in paramsNodeList)
            {
                paramElem = (XmlElement)paramNode;
                param = new XmlParam();
                m_paramList.Add(param);
                param.parseXml(paramElem);
            }
        }
    }
}