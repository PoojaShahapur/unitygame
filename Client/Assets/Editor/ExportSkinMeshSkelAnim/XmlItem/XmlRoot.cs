using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace EditorTool
{
    /**
     * @brief Xml 配置根节点
     */
    public class XmlRootBase
    {
        public string m_outPath = "";

        public ModelTypes m_modelTypes;
        public List<Mesh> m_meshList;
        public List<ModelPath> m_modelPathList;

        public XmlRootBase()
        {
            m_modelTypes = new ModelTypes();
            m_meshList = new List<Mesh>();
            m_modelPathList = new List<ModelPath>();
        }

        // 创建所需要的所有的子目录
        protected void createSubDir()
        {
            if (!string.IsNullOrEmpty(m_outPath))
            {
                m_modelTypes.createDir(m_outPath);
            }
            
            foreach (Mesh mesh in m_meshList)
            {
                if (!string.IsNullOrEmpty(mesh.skelMeshParam.m_outPath))
                {
                    m_modelTypes.createDir(mesh.skelMeshParam.m_outPath);
                }
            }
        }

        virtual public void clear()
        {
            m_modelTypes.clear();
            m_meshList.Clear();
            m_modelPathList.Clear();
        }
    }

    /**
     * @brief Xml 蒙皮网格导出
     */
    public class XmlSkinMeshRoot : XmlRootBase
    {
        public eExportFileType m_exportFileType;        // 导出的文件类型

        public XmlSkinMeshRoot()
        {
            m_exportFileType = eExportFileType.eOneMeshOneFile;
        }

        override public void clear()
        {
            m_exportFileType = eExportFileType.eOneMeshOneFile;

            base.clear();
        }

        public void parseSkinsXml()
        {
            SkinMeshCfgParse skelMeshCfgParse = new SkinMeshCfgParse();
            skelMeshCfgParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/ExportSkinsCfg.xml"), this);

            createSubDir();         // 创建出来所有的子目录，不用在执行中判断
        }

        public void exportBoneList()
        {
            XmlDocument xmlDocSave = new XmlDocument();
            xmlDocSave.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = xmlDocSave.CreateElement("Root");

            foreach (Mesh mesh in m_meshList)
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
            foreach (Mesh mesh in m_meshList)
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
            foreach (Mesh mesh in m_meshList)
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

            foreach (Mesh mesh in m_meshList)
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

    /**
     * @brief 导出子网格配置
     */
    public class XmlSubMeshRoot : XmlRootBase
    {
        public XmlSubMeshRoot()
        {
            
        }

        public void parseSkelSubMeshPackXml()
        {
            SubMeshCfgParse skelSubMeshPackParse = new SubMeshCfgParse();
            skelSubMeshPackParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/ExportSubMeshCfg.xml"), this);

            createSubDir();         // 创建出来所有的子目录，不用在执行中判断
        }

        public void subMeshPackFile()
        {
            foreach (Mesh mesh in m_meshList)
            {
                mesh.packSkelSubMesh();
            }
        }
    }

    /**
     * @briefe 导出骨骼配置
     */
    public class XmlSkeletonRoot : XmlRootBase
    {
        public XmlSkeletonRoot()
        {
            
        }

        public void parseSkeletonXml()
        {
            SkeletonCfgParse skeletonCfgParse = new SkeletonCfgParse();
            skeletonCfgParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/ExportSkeletonCfg.xml"), this);

            createSubDir();         // 创建出来所有的子目录，不用在执行中判断
        }

        public void exportSkeleton()
        {
            foreach (Mesh mesh in m_meshList)
            {
                mesh.packSkel();
            }
        }
    }
}