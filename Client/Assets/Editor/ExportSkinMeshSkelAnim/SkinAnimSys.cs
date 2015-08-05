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
        public XmlSubMeshRoot m_xmlSubMeshRoot;
        public XmlSkeletonRoot m_xmlSkeletonRoot;
        public XmlSkelAnimControlRoot m_xmlSkelAnimControlRoot;
        public BuildTarget m_targetPlatform;

        protected SkinAnimSys()
        {   
            m_targetPlatform = BuildTarget.StandaloneWindows;
            m_xmlSkinMeshRoot = new XmlSkinMeshRoot();
            m_xmlSubMeshRoot = new XmlSubMeshRoot();
            m_xmlSkeletonRoot = new XmlSkeletonRoot();
            m_xmlSkelAnimControlRoot = new XmlSkelAnimControlRoot();
        }

        public void parseSkinsXml()
        {
            m_xmlSkinMeshRoot.clear();
            m_xmlSkinMeshRoot.parseSkinsXml();
        }

        public void parseSkelSubMeshPackXml()
        {
            m_xmlSubMeshRoot.clear();
            m_xmlSubMeshRoot.parseSkelSubMeshPackXml();
        }

        public void parseSkeletonCfgXml()
        {
            m_xmlSkeletonRoot.clear();
            m_xmlSkeletonRoot.parseSkeletonXml();
        }

        public void parseSkelAnimControllerXml()
        {

        }

        public void exportBoneList()
        {
            m_xmlSkinMeshRoot.exportBoneList();
        }

        public void exportSkinsFile()
        {
            m_xmlSkinMeshRoot.exportSkinsFile();
        }

        public void exportSubMeshPackFile()
        {
            m_xmlSubMeshRoot.subMeshPackFile();
        }

        public void exportSkeletonFile()
        {
            m_xmlSkeletonRoot.exportSkeleton();
        }
    }
}