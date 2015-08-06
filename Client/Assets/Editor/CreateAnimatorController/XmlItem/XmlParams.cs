using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace EditorTool
{
    public class XmlParams
    {
        protected List<XmlParam> m_paramList = new List<XmlParam>();
        //protected AnimatorControllerParameter[] m_parameters;
        protected XmlAnimatorController m_xmlAnimatorController;

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

        //public AnimatorControllerParameter[] parameters
        //{
        //    get
        //    {
        //        return m_parameters;
        //    }
        //    set
        //    {
        //        m_parameters = value;
        //    }
        //}

        public XmlAnimatorController xmlAnimatorController
        {
            get
            {
                return m_xmlAnimatorController;
            }
            set
            {
                m_xmlAnimatorController = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            clear();

            XmlNodeList paramsNodeList = elem.ChildNodes;
            XmlElement paramElem = null;
            XmlParam param;
            foreach (XmlNode paramNode in paramsNodeList)
            {
                paramElem = (XmlElement)paramNode;
                param = new XmlParam();
                m_paramList.Add(param);
                param.xmlParams = this;
                param.parseXml(paramElem);
            }
        }

        public void clear()
        {
            m_paramList.Clear();
            //m_parameters = null;
        }
    }
}