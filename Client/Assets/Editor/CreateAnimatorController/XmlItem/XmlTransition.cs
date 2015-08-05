using System.Collections.Generic;
using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    public class XmlTransition
    {
        protected string m_srcStateName;
        protected string m_destStateName;
        protected List<XmlCondition> m_condList = new List<XmlCondition>();

        protected XmlStateMachine m_stateMachine;
        // protected AnimatorTransition m_animatorTransition;
        protected AnimatorStateTransition m_animatorStateTransition;

        public XmlStateMachine stateMachine
        {
            get
            {
                return m_stateMachine;
            }
            set
            {
                m_stateMachine = value;
            }
        }

        public string srcStateName
        {
            get
            {
                return m_srcStateName;
            }
            set
            {
                m_srcStateName = value;
            }
        }

        public string destStateName
        {
            get
            {
                return m_destStateName;
            }
            set
            {
                m_destStateName = value;
            }
        }

        //public AnimatorTransition animatorTransition
        //{
        //    get
        //    {
        //        return m_animatorTransition;
        //    }
        //    set
        //    {
        //        m_animatorTransition = value;
        //    }
        //}

        public AnimatorStateTransition animatorStateTransition
        {
            get
            {
                return m_animatorStateTransition;
            }
            set
            {
                m_animatorStateTransition = value;
            }
        }

        public List<XmlCondition> condList
        {
            get
            {
                return m_condList;
            }
            set
            {
                m_condList = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_srcStateName = ExportUtil.getXmlAttrStr(elem.Attributes["srcstate"]);
            m_destStateName = ExportUtil.getXmlAttrStr(elem.Attributes["deststate"]);

            XmlNodeList condNodeList = elem.SelectNodes("Condition");
            XmlElement condElem = null;
            XmlCondition cond;
            foreach (XmlNode condNode in condNodeList)
            {
                condElem = (XmlElement)condNode;
                cond = new XmlCondition();
                m_condList.Add(cond);
                cond.parseXml(condElem);
            }
        }
    }
}