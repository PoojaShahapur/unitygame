using SDK.Common;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SDK.Lib
{
    public class MeshMgr : IMeshMgr
    {
        // ¼ÇÂ¼ÃÉÆ¤ÐÅÏ¢
        protected Dictionary<string, Dictionary<string, string> > m_skinDic = new Dictionary<string,Dictionary<string,string>>();

        public MeshMgr()
        {

        }

        public void loadSkinInfo()
        {
            LoadParam param = Ctx.m_instance.m_resMgr.getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBeingPath] + "BonesList.unity3d";
            param.m_loadedcb = onloaded;
            Ctx.m_instance.m_resMgr.loadBundle(param);
        }

        public void onloaded(EventDisp resEvt)
        {
            IRes res = resEvt.m_param as IRes;
            TextAsset text = res.getObject("BoneList") as TextAsset;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text.text);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            XmlNodeList itemMeshList = rootNode.ChildNodes;
            XmlElement itemMesh;

            XmlNodeList itemSubMeshList;
            XmlElement itemSubMesh;
            string meshName = "";
            string subMeshName = "";
            string bonesList = "";

            foreach (XmlNode itemNode1f in itemMeshList)
            {
                itemMesh = (XmlElement)itemNode1f;
                meshName = itemMesh.Attributes["name"].Value;
                m_skinDic[meshName] = new Dictionary<string, string>();

                itemSubMeshList = itemMesh.ChildNodes;
                foreach (XmlNode itemNode2f in itemSubMeshList)
                {
                    itemSubMesh = (XmlElement)itemNode2f;
                    subMeshName = itemSubMesh.Attributes["name"].Value;
                    bonesList = itemSubMesh.Attributes["bonelist"].Value;
                    m_skinDic[meshName][subMeshName] = bonesList;
                }
            }
        }
    }
}