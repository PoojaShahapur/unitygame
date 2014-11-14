using System.Xml;

namespace EditorTool
{
    class PackItem
    {
        public string m_name;
        public string m_path;

        public void parseXml(XmlElement elem)
        {
            m_name = elem.Attributes["name"].Value;
            m_path = elem.Attributes["path"].Value;
        }
    }
}