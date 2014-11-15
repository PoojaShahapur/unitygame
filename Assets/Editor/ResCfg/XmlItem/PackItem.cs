using System.Xml;

namespace EditorTool
{
    class PackItem
    {
        public string m_path;       // 这个目录可以是仅仅一个文件名字，或者有部分目录和文件名字的
        public string m_resType;    // 资源类型

        public void parseXml(XmlElement elem)
        {
            m_path = elem.Attributes["path"].Value;
            if (elem.Attributes["restype"] != null)
            {
                m_resType = elem.Attributes["restype"].Value;
            }
        }
    }
}