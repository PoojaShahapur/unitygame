using EditorTool;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AtlasPrefabSys
{
    public class CreateAtlasPrefabSys
    {
        static public CreateAtlasPrefabSys m_instance;

        protected AtlasXmlData m_atlasXmlData;

        public static CreateAtlasPrefabSys instance()
        {
            if (m_instance == null)
            {
                m_instance = new CreateAtlasPrefabSys();
            }
            return m_instance;
        }

        public CreateAtlasPrefabSys()
        {
            m_atlasXmlData = new AtlasXmlData();
        }

        public void clear()
        {
            m_atlasXmlData.clear();
        }

        public void parseXml()
        {
            m_atlasXmlData.parseXml();
        }

        public void exportPrefab()
        {
            m_atlasXmlData.exportPrefab();
        }

        public void exportAsset()
        {
            m_atlasXmlData.exportAsset();
        }
    }
}