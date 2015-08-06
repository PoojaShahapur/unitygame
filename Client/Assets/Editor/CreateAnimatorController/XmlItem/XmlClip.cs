using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    public class XmlClip
    {
        protected string m_name;
        protected string m_fullMotion;

        protected XmlStateMachine m_stateMachine;  // 对应的状态机
        protected List<XmlState> m_stateList = new List<XmlState>();

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

        public string fullMotion
        {
            get
            {
                return m_fullMotion;
            }
            set
            {
                m_fullMotion = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            clear();

            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_fullMotion = string.Format("{0}/{1}", AnimatorControllerCreateSys.m_instance.curXmlAnimatorController.inPath, m_name);

            XmlNodeList stateNodeList = elem.SelectNodes("State");
            XmlElement stateElem = null;
            XmlState state;
            foreach (XmlNode stateNode in stateNodeList)
            {
                stateElem = (XmlElement)stateNode;
                state = new XmlState();
                state.stateMachine = m_stateMachine;
                m_stateList.Add(state);
                m_stateMachine.stateList.Add(state);
                state.parseXml(stateElem);
            }
        }

        public XmlState getXmlStateByName(string name_)
        {
            foreach(var item in m_stateList)
            {
                if(item.motion == name_)
                {
                    return item;
                }
            }

            return null;
        }

        public void clear()
        {
            m_stateList.Clear();
        }
    }
}