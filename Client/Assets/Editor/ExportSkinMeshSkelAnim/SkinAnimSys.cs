using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;

namespace EditorTool
{
    public class SkinAnimSys
    {
        static public SkinAnimSys m_instance;

        public static SkinAnimSys instance()
        {
            if (m_instance == null)
            {
                m_instance = new SkinAnimSys();
            }
            return m_instance;
        }

        public List<Mesh> m_meshList = new List<Mesh>();
        public List<Mesh> m_skelSubMeshList = new List<Mesh>();
        public BuildTarget m_targetPlatform;
        public RootParam m_rootParam = new RootParam();

        protected SkinAnimSys()
        {   
            m_targetPlatform = BuildTarget.StandaloneWindows;
        }

        public void parseSkinsXml()
        {
            SkelMeshCfgParse skelMeshCfgParse = new SkelMeshCfgParse();
            skelMeshCfgParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/ExportSkinsCfg.xml"), m_meshList);
            m_rootParam.m_outPath = skelMeshCfgParse.m_outPath;
        }

        public void parseSkelSubMeshPackXml()
        {
            SkelSubMeshPackParse skelSubMeshPackParse = new SkelSubMeshPackParse();
            skelSubMeshPackParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/SkelSubMeshPackCfg.xml"), m_skelSubMeshList);
            m_rootParam.m_outPath = skelSubMeshPackParse.m_outPath;
            m_rootParam.m_tmpPath = skelSubMeshPackParse.m_tmpPath;
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

            string xmlName = string.Format("{0}/{1}", m_rootParam.m_outPath, "BoneList.xml");
            xmlName = ExportUtil.getDataPath(xmlName);
            xmlDocSave.Save(@xmlName);
        }

        public void exportSkinsFile()
        {
            string xmlStr = "<?xml version='1.0' encoding='utf-8' ?>\n<Root>\n";

            foreach (Mesh mesh in m_meshList)
            {
                mesh.exportMeshBoneFile(ref xmlStr);
            }

            xmlStr += "</Root>";

            string xmlName = string.Format("{0}/{1}", m_rootParam.m_outPath, "BoneList.xml");
            xmlName = ExportUtil.getDataPath(xmlName);

            FileStream sFile = new FileStream(xmlName, FileMode.Create);
            byte[] data = new UTF8Encoding().GetBytes(xmlStr);
            //开始写入
            sFile.Write(data, 0, data.Length);

            //清空缓冲区、关闭流
            sFile.Flush();
            sFile.Close();
        }

        public void skelSubMeshPackFile()
        {
            foreach (Mesh mesh in m_skelSubMeshList)
            {
                mesh.packSkelSubMesh(m_rootParam);
            }
        }
    }
}