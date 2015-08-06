using System.Xml;

namespace EditorTool
{
    /**
     * @brief 配置的整个目录
     */
    public class XmlPath
    {
        protected string m_inPath;
        protected string m_outPath;
        protected string m_outExtName;

        protected XmlElement m_xmlTmp;      // Xml 模板

        public void parseXml(XmlElement elem)
        {
            m_inPath = ExportUtil.getXmlAttrStr(elem.Attributes["inpath"]);
            m_outPath = ExportUtil.getXmlAttrStr(elem.Attributes["outpath"]);
            m_outExtName = ExportUtil.getXmlAttrStr(elem.Attributes["outextname"]);

            m_xmlTmp = elem;

            addController();
        }

        public void addController()
        {
            string fullPath = ExportUtil.getWorkPath(m_inPath);
            ExportUtil.traverseFilesInOneDir(fullPath, onFindOneFile);
        }

        protected void onFindOneFile(string fullPath)
        {
            string ext = ExportUtil.getFileExt(fullPath);
            string nameNoExt = ExportUtil.getFileNameNoExt(fullPath);

            XmlAnimatorController controller = null;
            if (ExportUtil.FBX == ext)
            {
                if (!nameNoExt.Contains(ExportUtil.AT))      // 如果包含 @ ，就说明是一个动画
                {
                    m_xmlTmp.SetAttribute("outname", nameNoExt);
                    controller = new XmlAnimatorController();
                    ExportAnimatorControllerSys.m_instance.controllerList.Add(controller);
                    controller.parseXml(m_xmlTmp);

                    // 调整完整的文件名字
                    controller.adjustFileName(nameNoExt);
                }
            }
        }
    }
}