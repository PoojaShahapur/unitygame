using SDK.Common;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SDK.Lib
{
    public class MeshMgr : IMeshMgr
    {
        // ¼ÇÂ¼ÃÉÆ¤ÐÅÏ¢
        protected Dictionary<string, Dictionary<string, string[]> > m_skinDic = new Dictionary<string,Dictionary<string,string[]>>();

        public MeshMgr()
        {

        }

        public string[] getBonesListByName(string name)
        {
            if(m_skinDic["DefaultAvata"].ContainsKey(name))
            {
                return m_skinDic["DefaultAvata"][name];
            }

            return null;
        }

        public void loadSkinInfo()
        {
            LoadParam param = Ctx.m_instance.m_resLoadMgr.getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBeingPath] + "BonesList";
            param.m_loaded = onloaded;
            Ctx.m_instance.m_resLoadMgr.loadBundle(param);
        }

        public void onloaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
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
                m_skinDic[meshName] = new Dictionary<string, string[]>();

                itemSubMeshList = itemMesh.ChildNodes;
                foreach (XmlNode itemNode2f in itemSubMeshList)
                {
                    itemSubMesh = (XmlElement)itemNode2f;
                    subMeshName = itemSubMesh.Attributes["name"].Value;
                    bonesList = itemSubMesh.Attributes["bonelist"].Value;
                    m_skinDic[meshName][subMeshName] = bonesList.Split(',');
                }
            }
        }
    }
}