using SDK.Common;
using System.Security;
namespace SDK.Lib
{
    public class TerrainCfg
    {
        protected float m_width;
        protected float m_height;

        public float Width
        {
            get
            {
                return m_width;
            }
        }

        public float Height
        {
            get
            {
                return m_height;
            }
        }

        public void parse(SecurityElement xe)
        {
            string attr = "";
            UtilXml.getXmlAttrStr(xe, "size", ref attr);
            attr = attr.Substring(1, attr.Length - 2);
            char[] split = new char[1];
            split[0] = ',';
            string[] strarr = attr.Split(split);

            m_width = float.Parse(strarr[0]);
            m_height = float.Parse(strarr[2]);
        }
    }
}