using System.Xml;

namespace EditorTool
{
    /**
     * @brief 这个事模型的目录，这个目录下的所有的内容都要导出来
     */
    public class ModelPath
    {
        protected string m_resType;
        protected string m_inPath;
        protected string m_outPath;
        protected eModelType m_modelType;
        protected bool m_packSkel;

        public void parseXml(XmlElement elem)
        {
            m_resType = ExportUtil.getXmlAttrStr(elem.Attributes["restype"]);
            m_inPath = ExportUtil.getXmlAttrStr(elem.Attributes["inpath"]);
            m_outPath = ExportUtil.getXmlAttrStr(elem.Attributes["outpath"]);
            m_modelType = (eModelType)ExportUtil.getXmlAttrInt(elem.Attributes["part"]);

            addMesh();
        }

        protected void addMesh()
        {
            string fullPath = ExportUtil.getDataPath(m_inPath);
            ExportUtil.traverseFilesInOneDir(fullPath, onFindOneFile);
        }

        protected void onFindOneFile(string fullPath)
        {
            string fileName = ExportUtil.getFileNameWithExt(fullPath);
            Mesh mesh = new Mesh();
            mesh.skelMeshParam.m_name = fileName;
            mesh.skelMeshParam.m_inPath = m_inPath;
            mesh.skelMeshParam.m_outPath = m_outPath;
            mesh.skelMeshParam.m_resType = m_resType;
            mesh.skelMeshParam.m_packSkel = m_packSkel;

            SkinAnimSys.m_instance.m_xmlSkinMeshRoot.m_skinMeshList.Add(mesh);
        }
    }
}