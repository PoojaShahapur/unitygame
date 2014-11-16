using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace EditorTool
{
    class ResCfgData
    {
        public List<PackType> m_packList = new List<PackType>();
        public List<Mesh> m_meshList = new List<Mesh>();
        public string m_outPath = "";

        public void parseXml()
        {
            ResCfgParse resCfgParse = new ResCfgParse();
            resCfgParse.parseXml(ExportUtil.getDataPath("Config/Tool/ResPackCfg.xml"), m_packList);
        }

        public void parseSkelMeshXml()
        {
            SkelMeshCfgParse skelMeshCfgParse = new SkelMeshCfgParse();
            skelMeshCfgParse.parseXml(ExportUtil.getDataPath("Config/Tool/SkelMeshCfg.xml"), m_meshList);
            m_outPath = skelMeshCfgParse.m_outPath;
        }

        public void pack()
        {
            foreach (PackType packType in m_packList)
            {
                packType.packPack();
            }
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

            List<string> pathList = new List<string>();
            string xmlName = string.Format("{0}/{1}", m_outPath, "BoneList.xml");
            xmlName = ExportUtil.getDataPath(xmlName);

            xmlDocSave.Save(xmlName);
        }
    }
}