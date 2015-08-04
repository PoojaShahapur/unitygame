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
                string xmlPath = string.Format("{0}/{1}/{2}", m_outPath, m_modelTypes.modelTypeDic[mesh.skelMeshParam.m_modelType], xmlName);
                xmlPath = ExportUtil.getDataPath(xmlPath);

                ExportUtil.deleteFile(xmlName);
                FileStream fileStream = new FileStream(xmlName, FileMode.Create);
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
            string xmlStr = "<?xml version='1.0' encoding='utf-8' ?>\n<Root>\n";

            foreach (Mesh mesh in m_skinMeshList)
            {
                mesh.exportMeshBoneFile(ref xmlStr);
            }

            xmlStr += "</Root>";

            string xmlName = string.Format("{0}/{1}", m_outPath, "BoneList.xml");
            xmlName = ExportUtil.getDataPath(xmlName);

            ExportUtil.deleteFile(xmlName);
            FileStream fileStream = new FileStream(xmlName, FileMode.Create);
            byte[] data = new UTF8Encoding().GetBytes(xmlStr);
            //开始写入
            fileStream.Write(data, 0, data.Length);

            //清空缓冲区、关闭流
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
        }

        protected void exportSkinsFile_OneTypeOneFile()
        {
            string xmlStr = "<?xml version='1.0' encoding='utf-8' ?>\n<Root>\n";

            foreach (Mesh mesh in m_skinMeshList)
            {
                mesh.exportMeshBoneFile(ref xmlStr);
            }

            xmlStr += "</Root>";

            string xmlName = string.Format("{0}/{1}", m_outPath, "BoneList.xml");
            xmlName = ExportUtil.getDataPath(xmlName);

            ExportUtil.deleteFile(xmlName);
            FileStream fileStream = new FileStream(xmlName, FileMode.Create);
            byte[] data = new UTF8Encoding().GetBytes(xmlStr);
            //开始写入
            fileStream.Write(data, 0, data.Length);

            //清空缓冲区、关闭流
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
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