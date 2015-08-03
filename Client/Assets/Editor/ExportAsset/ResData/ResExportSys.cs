using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class ResExportSys
    {
        public static ResExportSys m_instance = null;

        public List<PackType> m_packList = new List<PackType>();
        public BuildTarget m_targetPlatform;

        public ResourcesCfgPackData m_pResourcesCfgPackData = new ResourcesCfgPackData();
        public ExportResList m_exportResList = new ExportResList();

        public static void instance()
        {
            if (m_instance == null)
            {
                m_instance = new ResExportSys();
            }
        }

        protected ResExportSys()
        {   
            m_targetPlatform = BuildTarget.StandaloneWindows;
        }

        public void parseXml()
        {
            ResCfgParse resCfgParse = new ResCfgParse();
            resCfgParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/ResPackCfg.xml"), m_packList);
        }

        public void parseResourceXml()
        {
            ResourceCfgParse resourceCfgParse = new ResourceCfgParse();
            resourceCfgParse.parseXml(ExportUtil.getDataPath("Res/Config/Tool/ResPackResourceCfg.xml"), m_pResourcesCfgPackData.m_resourceList);
        }

        public void pack()
        {
            foreach (PackType packType in m_packList)
            {
                packType.packPack();
            }
        }

        public void packResourceList()
        {
            foreach (var item in m_pResourcesCfgPackData.m_resourceList)
            {
                item.packPack();
            }
        }
    }
}