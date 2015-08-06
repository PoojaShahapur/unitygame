using System.Collections.Generic;
using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    /**
     * @brief 状态机
     */
    public class XmlStateMachineTransition
    {
        protected string m_parentStateMachineName;  // 父状态机
        protected string m_srcStateMachineName;     // 原状态机
        protected string m_destStateMachineName;    // 目标状态机
        protected string m_destStateName;           // 目标状态机的状态

        protected List<XmlCondition> m_condList = new List<XmlCondition>();

        protected AnimatorTransition m_animatorTransition;
        protected XmlLayer m_xmlLayer;

        public string srcStateMachineName
        {
            get
            {
                return m_srcStateMachineName;
            }
            set
            {
                m_srcStateMachineName = value;
            }
        }

        public string destStateMachineName
        {
            get
            {
                return m_destStateMachineName;
            }
            set
            {
                m_destStateMachineName = value;
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

        public AnimatorTransition animatorTransition
        {
            get
            {
                return m_animatorTransition;
            }
            set
            {
                m_animatorTransition = value;
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

        public XmlLayer xmlLayer
        {
            get
            {
                return m_xmlLayer;
            }
            set
            {
                m_xmlLayer = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            clear();

            m_parentStateMachineName = ExportUtil.getXmlAttrStr(elem.Attributes["parentstatemachine"]);
            m_srcStateMachineName = ExportUtil.getXmlAttrStr(elem.Attributes["srcstatemachine"]);
            m_destStateMachineName = ExportUtil.getXmlAttrStr(elem.Attributes["deststatemachine"]);
            m_destStateName = ExportUtil.getXmlAttrStr(elem.Attributes["deststate"]);

            XmlNodeList condNodeList = elem.SelectNodes("Condition");
            XmlElement condElem = null;
            XmlCondition cond;
            foreach (XmlNode condNode in condNodeList)
            {
                condElem = (XmlElement)condNode;
                cond = new XmlCondition();
                m_condList.Add(cond);
                cond.xmlStateMachineTransition = this;
                cond.parseXml(condElem);
            }
        }

        public void clear()
        {
            m_condList.Clear();
            m_animatorTransition = null;
        }
    }
}