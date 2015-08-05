using System.Collections.Generic;
using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    public class XmlLayer
    {
        protected List<XmlStateMachine> m_stateMachineList = new List<XmlStateMachine>();
        protected AnimatorControllerLayer m_animatorControllerLayer;

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

        public void parseXml(XmlElement elem)
        {
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
        }
    }
}