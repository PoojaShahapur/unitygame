using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace EditorTool
{
    /**
     * @brief Xml 的根节点
     */
    public class XmlSkinMeshRoot
    {
        public string m_outPath = "";
        public eExportFileType m_exportFileType;        // 导出的文件类型

        public ModelTypes m_modelTypes;
        public List<Mesh> m_skinMeshList;
        public List<ModelPath> m_modelPathList;

        public XmlSkinMeshRoot()
        {
            m_modelTypes = new ModelTypes();
            m_skinMeshList = new List<Mesh>();
            m_modelPathList = new List<ModelPath>();
        }

        public void parseSkinsXml()
        {
            SkelMeshCfgParse skelMeshCfgParse = new SkelMeshCfgParse();
            skelMeshCfgParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/ExportSkinsCfg.xml"));

            createSubDir();         // 创建出来所有的子目录，不用在执行中判断
        }

        // 创建所需要的所有的子目录
        protected void createSubDir()
        {
            m_modelTypes.createDir(m_outPath);
            
            foreach (Mesh mesh in m_skinMeshList)
            {
                m_modelTypes.createDir(mesh.skelMeshParam.m_outPath);
            }
        }

        public void exportBoneList()
        {
            XmlDocument xmlDocSave = new XmlDocument();
            xmlDocSave.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = xmlDocSave.CreateElement("Root");

            foreach (Mesh mesh in m_skinMeshList)
            {
                mesh.exportMeshBone(xmlDocSave, root);
            }

            string xmlName = string.Format("{0}/{1}", m_outPath, "BoneList.xml");
            xmlName = ExportUtil.getDataPath(xmlName);
            xmlDocSave.Save(@xmlName);
        }

        public void exportSkinsFile()
        {
            if(eExportFileType.eOneMeshOneFile == m_exportFileType)
            {
                exportSkinsFile_OneMeshOneFile();
            }
            else if(eExportFileType.eOneSubMeshOneFile == m_exportFileType)
            {
                exportSkinsFile_OneSubMeshOneFile();
            }
            else if(eExportFileType.eOneTypeOneFile == m_exportFileType)
            {
                exportSkinsFile_OneTypeOneFile();
            }
        }

        // 一个 Mesh 一个配置文件
        protected void exportSkinsFile_OneMeshOneFile()
        {
            foreach (Mesh mesh in m_skinMeshList)
            {
                string xmlStr = "<?xml version='1.0' encoding='utf-8' ?>\n<Root>\n";

                mesh.exportMeshBoneFile(ref xmlStr);

                xmlStr += "</Root>";

                string xmlName = string.Format("{0}.xml", ExportUtil.getFileNameNoExt(mesh.skelMeshParam.m_name));
                string xmlPath = string.Format("{0}/{1}/{2}", m_outPath, m_modelTypes.modelTypeDic[mesh.skelMeshParam.m_modelType].subPath, xmlName);
                xmlPath = ExportUtil.getDataPath(xmlPath);

                ExportUtil.deleteFile(xmlPath);
                FileStream fileStream = new FileStream(xmlPath, FileMode.CreateNew);
                byte[] data = new UTF8Encoding().GetBytes(xmlStr);
                //开始写入
                fileStream.Write(data, 0, data.Length);

                //清空缓冲区、关闭流
                fileStream.Flush();
                fileStream.Close();
                fileStream.Dispose();
            }
        }

        protected void exportSkinsFile_OneSubMeshOneFile()
        {
            foreach (Mesh mesh in m_skinMeshList)
            {
                foreach (SubMesh subMesh in mesh.m_subMeshList)
                {
                    string xmlStr = "<?xml version='1.0' encoding='utf-8' ?>\n<Root>\n";
                    xmlStr += string.Format("    <Mesh name=\"{0}\" >\n", ExportUtil.getFileNameNoExt(mesh.skelMeshParam.m_name));

                    subMesh.exportSubMeshBoneFile(mesh.skelMeshParam, ref xmlStr);

                    xmlStr += "    </Mesh>\n";

                    xmlStr += "</Root>";

                    string xmlName = string.Format("{0}.xml", ExportUtil.getFileNameNoExt(subMesh.m_part));
                    string xmlPath = string.Format("{0}/{1}/{2}", m_outPath, m_modelTypes.modelTypeDic[mesh.skelMeshParam.m_modelType].subPath, xmlName);
                    xmlPath = ExportUtil.getDataPath(xmlPath);

                    ExportUtil.deleteFile(xmlPath);
                    FileStream fileStream = new FileStream(xmlPath, FileMode.CreateNew);
                    byte[] data = new UTF8Encoding().GetBytes(xmlStr);
                    //开始写入
                    fileStream.Write(data, 0, data.Length);

                    //清空缓冲区、关闭流
                    fileStream.Flush();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }

        protected void exportSkinsFile_OneTypeOneFile()
        {
            m_modelTypes.addXmlHeader();

            foreach (Mesh mesh in m_skinMeshList)
            {
                foreach (SubMesh subMesh in mesh.m_subMeshList)
                {
                    subMesh.exportSubMeshBoneFile(mesh.skelMeshParam, ref m_modelTypes.modelTypeDic[mesh.skelMeshParam.m_modelType].m_content);
                }
            }

            m_modelTypes.addXmlEnd();
            m_modelTypes.save2Files(m_outPath);
        }
    }

    public class XmlSkelSubMeshRoot
    {
        public string m_tmpPath = "";
        public List<Mesh> m_skelSubMeshList;

        public XmlSkelSubMeshRoot()
        {
            m_skelSubMeshList = new List<Mesh>();
        }

        public void parseSkelSubMeshPackXml()
        {
            SkelSubMeshPackParse skelSubMeshPackParse = new SkelSubMeshPackParse();
            skelSubMeshPackParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/SkelSubMeshPackCfg.xml"), m_skelSubMeshList);
        }

        public void skelSubMeshPackFile()
        {
            foreach (Mesh mesh in m_skelSubMeshList)
            {
                mesh.packSkelSubMesh();
            }
        }

    }
}