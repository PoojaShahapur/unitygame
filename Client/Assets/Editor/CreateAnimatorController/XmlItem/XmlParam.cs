using System.Xml;
using UnityEngine;

namespace EditorTool
{
    public class XmlParam
    {
        public const string FLOAT = "Float";
        public const string INT = "Int";
        public const string BOOL = "Bool";
        public const string TRIGGER = "Trigger";

        protected string m_name;
        protected AnimatorControllerParameterType m_type;
        protected AnimatorControllerParameter m_animatorControllerParameter;

        protected XmlParams m_xmlParams;

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

        public AnimatorControllerParameterType type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        }

        public XmlParams xmlParams
        {
            get
            {
                return m_xmlParams;
            }
            set
            {
                m_xmlParams = value;
            }
        }

        public AnimatorControllerParameter animatorControllerParameter
        {
            get
            {
                return m_animatorControllerParameter;
            }
            set
            {
                m_animatorControllerParameter = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            clear();

            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            string typeStr = ExportUtil.getXmlAttrStr(elem.Attributes["type"]);

            if(FLOAT == typeStr)
            {
                m_type = AnimatorControllerParameterType.Float;
            }
            else if (INT == typeStr)
            {
                m_type = AnimatorControllerParameterType.Int;
            }
            else if (BOOL == typeStr)
            {
                m_type = AnimatorControllerParameterType.Bool;
            }
            else if (TRIGGER == typeStr)
            {
                m_type = AnimatorControllerParameterType.Trigger;
            }
        }

        public void clear()
        {
            m_animatorControllerParameter = null;
        }
    }
}