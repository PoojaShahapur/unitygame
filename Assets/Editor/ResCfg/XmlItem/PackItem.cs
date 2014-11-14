using System.Xml;

namespace EditorTool
{
    class PackItem
    {
        public string m_path;

        public void parseXml(XmlElement elem)
        {
            m_path = elem.Attributes["path"].Value;
        }
    }
}