using System.Collections.Generic;
using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    public class XmlState
    {
        protected string m_motion;

        protected List<XmlCondition> m_condList = new List<XmlCondition>();
        protected XmlStateMachine m_stateMachine;  // 对应的状态机

        protected AnimatorState m_animatorState;

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

        public string motion
        {
            get
            {
                return m_motion;
            }
            set
            {
                m_motion = value;
            }
        }

        public AnimatorState animatorState
        {
            get
            {
                return m_animatorState;
            }
            set
            {
                m_animatorState = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            clear();

            m_motion = ExportUtil.getXmlAttrStr(elem.Attributes["motion"]);

            XmlNodeList condNodeList = elem.SelectNodes("AnyCondition");
            XmlElement condElem = null;
            XmlCondition cond;
            foreach (XmlNode condNode in condNodeList)
            {
                condElem = (XmlElement)condNode;
                cond = new XmlCondition();
                m_condList.Add(cond);
                cond.xmlState = this;
                cond.parseXml(condElem);
            }
        }

        public void clear()
        {
            m_condList.Clear();
            m_animatorState = null;
        }
    }
}