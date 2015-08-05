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

        public void parseXml(XmlElement elem)
        {
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
    }
}