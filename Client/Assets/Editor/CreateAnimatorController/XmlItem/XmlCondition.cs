using System;
using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    public class XmlCondition
    {
        public const string GREATER = "Greater";            // 大于
        public const string LESS = "Less";                  // 小于
        public const string EQUALS = "Equals";              // 等于
        public const string NOTEQUAL = "NotEqual";          // 不等于

        protected string m_name;
        protected string m_value;
        protected AnimatorConditionMode m_opMode;           // 操作模式

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

        public float getFloatValue()
        {
            //float ret = float.Parse(m_value);
            float ret = Convert.ToSingle(m_value);
            return ret;
        }

        public AnimatorConditionMode opMode
        {
            get
            {
                return m_opMode;
            }
            set
            {
                m_opMode = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_value = ExportUtil.getXmlAttrStr(elem.Attributes["value"]);
            string _opMode = ExportUtil.getXmlAttrStr(elem.Attributes["OpMode"]);
            if (XmlCondition.GREATER == _opMode)
            {
                m_opMode = AnimatorConditionMode.Greater;
            }
            else if (XmlCondition.LESS == _opMode)
            {
                m_opMode = AnimatorConditionMode.Less;
            }
            else if (XmlCondition.EQUALS == _opMode)
            {
                m_opMode = AnimatorConditionMode.Equals;
            }
            else if (XmlCondition.NOTEQUAL == _opMode)
            {
                m_opMode = AnimatorConditionMode.NotEqual;
            }
        }
    }
}