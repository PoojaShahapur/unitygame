using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class ResCfgData
    {
        public static ResCfgData m_ins = null;

        public List<PackType> m_packList = new List<PackType>();
        public List<Mesh> m_meshList = new List<Mesh>();
        public List<Mesh> m_skelSubMeshList = new List<Mesh>();
        public RootParam m_rootParam = new RootParam();
        public BuildTarget m_targetPlatform;

        public ResourcesCfgPackData m_pResourcesCfgPackData = new ResourcesCfgPackData();

        public static void Instance()
        {
            if (m_ins == null)
            {
                m_ins = new ResCfgData();
            }
        }

        protected ResCfgData()
        {
            
            m_targetPlatform = BuildTarget.StandaloneWindows;
        }

        public void parseXml()
        {
            ResCfgParse resCfgParse = new ResCfgParse();
            resCfgParse.parseXml(ExportUtil.getDataPath("Config/Tool/ResPackCfg.xml"), m_packList);
        }

        public void parseSkinsXml()
        {
            SkelMeshCfgParse skelMeshCfgParse = new SkelMeshCfgParse();
            skelMeshCfgParse.parseXml(ExportUtil.getDataPath("Config/Tool/ExportSkinsCfg.xml"), m_meshList);
            m_rootParam.m_outPath = skelMeshCfgParse.m_outPath;
        }

        public void parseSkelSubMeshPackXml()
        {
            SkelSubMeshPackParse skelSubMeshPackParse = new SkelSubMeshPackParse();
            skelSubMeshPackParse.parseXml(ExportUtil.getDataPath("Config/Tool/SkelSubMeshPackCfg.xml"), m_skelSubMeshList);
            m_rootParam.m_outPath = skelSubMeshPackParse.m_outPath;
            m_rootParam.m_tmpPath = skelSubMeshPackParse.m_tmpPath;
        }

        public void parseResourceXml()
        {
            ResourceCfgParse resourceCfgParse = new ResourceCfgParse();
            resourceCfgParse.parseXml(ExportUtil.getDataPath("Config/Tool/ResPackResourceCfg.xml"), m_pResourcesCfgPackData.m_resourceList);
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

        public void packResourceList()
        {
            m_pResourcesCfgPackData.m_destFullPath = ExportUtil.getStreamingDataPath("");
            m_pResourcesCfgPackData.m_destFullPath = ExportUtil.normalPath(m_pResourcesCfgPackData.m_destFullPath);
            ExportUtil.DeleteDirectory(m_pResourcesCfgPackData.m_destFullPath);
            ExportUtil.CreateDirectory(m_pResourcesCfgPackData.m_destFullPath);
            foreach (var item in m_pResourcesCfgPackData.m_resourceList)
            {
                item.packPack();
            }
        }
    }
}