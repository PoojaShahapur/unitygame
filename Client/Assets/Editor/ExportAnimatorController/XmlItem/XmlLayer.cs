using System.Collections.Generic;
using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    public class XmlLayer
    {
        protected string m_name;
        protected List<XmlStateMachine> m_stateMachineList = new List<XmlStateMachine>();
        protected List<XmlStateMachineTransition> m_xmlStateMachineTransitionList = new List<XmlStateMachineTransition>();

        protected AnimatorControllerLayer m_animatorControllerLayer;
        protected XmlLayers m_xmlLayers;

        public List<XmlStateMachine> stateMachineList
        {
            get
            {
                return m_stateMachineList;
            }
            set
            {
                m_stateMachineList = value;
            }
        }

        public AnimatorControllerLayer animatorControllerLayer
        {
            get
            {
                return m_animatorControllerLayer;
            }
            set
            {
                m_animatorControllerLayer = value;
            }
        }

        public string name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public XmlLayers xmlLayers
        {
            get
            {
                return m_xmlLayers;
            }
            set
            {
                m_xmlLayers = value;
            }
        }

        public List<XmlStateMachineTransition> xmlStateMachineTransitionList
        {
            get
            {
                return m_xmlStateMachineTransitionList;
            }
            set
            {
                m_xmlStateMachineTransitionList = value;
            }
        }

        public void adjustFileName(string modelName)
        {
            foreach(var stateMachine in m_stateMachineList)
            {
                stateMachine.adjustFileName(modelName);
            }
        }

        public void parseXml(XmlElement elem)
        {
            clear();

            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);

            XmlNodeList stateMachineNodeList = elem.SelectNodes("Statemachine");
            XmlElement stateMachineElem = null;
            XmlStateMachine stateMachine;
            foreach (XmlNode stateMachineNode in stateMachineNodeList)
            {
                stateMachineElem = (XmlElement)stateMachineNode;
                stateMachine = new XmlStateMachine();
                stateMachine.layer = this;
                m_stateMachineList.Add(stateMachine);
                stateMachine.parseXml(stateMachineElem);
            }

            // 解析状态机转换
            XmlNodeList stateMachineTransitionNodeList = elem.SelectNodes("StateMachineTransition");
            XmlElement stateMachineTransitionElem = null;
            XmlStateMachineTransition stateMachineTransition;
            foreach (XmlNode stateMachineTransitionNode in stateMachineTransitionNodeList)
            {
                stateMachineTransitionElem = (XmlElement)stateMachineTransitionNode;
                stateMachineTransition = new XmlStateMachineTransition();
                stateMachineTransition.xmlLayer = this;
                m_xmlStateMachineTransitionList.Add(stateMachineTransition);
                stateMachineTransition.parseXml(stateMachineTransitionElem);
            }
        }

        public XmlStateMachine getXmlStateMachineByName(string name)
        {
            foreach(var item in m_stateMachineList)
            {
                if(item.name == name)
                {
                    return item;
                }
            }

            return null;
        }

        public void clear()
        {
            m_stateMachineList.Clear();
            m_xmlStateMachineTransitionList.Clear();
            m_animatorControllerLayer = null;
        }
    }
}