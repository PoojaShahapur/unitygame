using System.Collections.Generic;
using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    public class XmlState
    {
        protected string m_motion;

        protected List<XmlCondition> m_anyCondList = new List<XmlCondition>();
        protected XmlStateMachine m_stateMachine;  // 对应的状态机

        protected AnimatorState m_animatorState;

        public List<XmlCondition> anyCondList
        {
            get
            {
                return m_anyCondList;
            }
            set
            {
                m_anyCondList = value;
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
                m_anyCondList.Add(cond);
                cond.xmlState = this;
                cond.parseXml(condElem);
            }
        }

        public void clear()
        {
            m_anyCondList.Clear();
            m_animatorState = null;
        }
    }
}