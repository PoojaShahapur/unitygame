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

        public XmlSkinMeshRoot m_xmlSkinMeshRoot;
        public XmlSkelSubMeshRoot m_xmlSkelSubMeshRoot;
        public BuildTarget m_targetPlatform;

        protected SkinAnimSys()
        {   
            m_targetPlatform = BuildTarget.StandaloneWindows;
            m_xmlSkinMeshRoot = new XmlSkinMeshRoot();
            m_xmlSkelSubMeshRoot = new XmlSkelSubMeshRoot();
        }

        public void parseSkinsXml()
        {
            m_xmlSkinMeshRoot.parseSkinsXml();
        }

        public void parseSkelSubMeshPackXml()
        {
            m_xmlSkelSubMeshRoot.parseSkelSubMeshPackXml();
        }

        public void exportBoneList()
        {
            m_xmlSkinMeshRoot.exportBoneList();
        }

        public void exportSkinsFile()
        {
            m_xmlSkinMeshRoot.exportSkinsFile();
        }

        public void skelSubMeshPackFile()
        {
            m_xmlSkelSubMeshRoot.skelSubMeshPackFile();
        }
    }
}