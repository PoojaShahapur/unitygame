using System.Collections.Generic;
using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    public class XmlStateMachine
    {
        protected List<XmlState> m_stateList = new List<XmlState>();            // 这些是有动画文件的状态
        protected List<XmlClip> m_clipList = new List<XmlClip>();

        protected List<XmlState> m_noResStateList = new List<XmlState>();

        protected List<XmlStateTransition> m_tranList = new List<XmlStateTransition>();

        protected string m_name;                    // 状态机的名字

        protected XmlLayer m_layer;                // 当前状态机所在的 Layer
        protected AnimatorStateMachine m_animatorStateMachine;      // 记录当前状态机

        public List<XmlState> stateList
        {
            get
            {
                return m_stateList;
            }
            set
            {
                m_stateList = value;
            }
        }

        public AnimatorStateMachine animatorStateMachine
        {
            get
            {
                return m_animatorStateMachine;
            }
            set
            {
                m_animatorStateMachine = value;
            }
        }

        public XmlLayer layer
        {
            get
            {
                return m_layer;
            }
            set
            {
                m_layer = value;
            }
        }

        public List<XmlClip> clipList
        {
            get
            {
                return m_clipList;
            }
            set
            {
                m_clipList = value;
            }
        }

        public List<XmlStateTransition> tranList
        {
            get
            {
                return m_tranList;
            }
            set
            {
                m_tranList = value;
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

        public List<XmlState> noResStateList
        {
            get
            {
                return m_noResStateList;
            }
            set
            {
                m_noResStateList = value;
            }
        }

        public void adjustFileName(string modelName)
        {
            foreach (var clip in m_clipList)
            {
                clip.adjustFileName(modelName);
            }
        }

        public void parseXml(XmlElement elem)
        {
            clear();

            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);

            // 解析没有动画文件的 State
            XmlNodeList noStateNodeList = elem.SelectNodes("State");
            XmlElement stateElem = null;
            XmlState state = null;
            foreach (XmlNode stateNode in noStateNodeList)
            {
                stateElem = (XmlElement)stateNode;
                state = new XmlState();
                state.stateMachine = this;
                m_noResStateList.Add(state);
                state.parseXml(stateElem);
            }

            // 解析有动画资源文件的 State
            XmlNodeList clipNodeList = elem.SelectNodes("Clip");
            XmlElement clipElem = null;
            XmlClip _clip;
            foreach (XmlNode clipNode in clipNodeList)
            {
                clipElem = (XmlElement)clipNode;
                _clip = new XmlClip();
                _clip.stateMachine = this;
                m_clipList.Add(_clip);
                _clip.parseXml(clipElem);
            }

            // 解析状态机中的转换
            XmlNodeList tranNodeList = elem.SelectNodes("Transition");
            XmlElement tranElem = null;
            XmlStateTransition _tran;
            foreach (XmlNode tranNode in tranNodeList)
            {
                tranElem = (XmlElement)tranNode;
                _tran = new XmlStateTransition();
                _tran.stateMachine = this;
                m_tranList.Add(_tran);
                _tran.parseXml(tranElem);
            }
        }

        public XmlState getXmlStateByName(string name_)
        {
            foreach (var item in m_stateList)
            {
                if (item.motion == name_)
                {
                    return item;
                }
            }

            return null;
        }

        public void clear()
        {
            m_stateList.Clear();
            m_clipList.Clear();
            m_tranList.Clear();

            m_animatorStateMachine = null;
        }
    }
}