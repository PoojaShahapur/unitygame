using SDK.Common;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SDK.Lib
{
    public class ModelMgr : ResMgrBase, IModelMgr
    {
        // ��¼��Ƥ��Ϣ
        protected Dictionary<string, Dictionary<string, string[]> > m_skinDic = new Dictionary<string,Dictionary<string,string[]>>();

        public ModelMgr()
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
            param.m_loaded = onSkinLoaded;
            Ctx.m_instance.m_resLoadMgr.loadBundle(param);
        }

        public void onSkinLoaded(IDispatchObject resEvt)
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

        // ��ȡ�������ģ��
        public ModelRes getCardGroupModel()
        {
            string prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupModelAttrItem.m_prefabName;
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupModelAttrItem.m_path;

            return syncGet<ModelRes>(prefab, path) as ModelRes;
        }

        // ��ȡ�������еĿ��Ƶ�ģ��
        public ModelRes getGroupCardModel()
        {
            string prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_groupCardModelAttrItem.m_prefabName;
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_groupCardModelAttrItem.m_path;

            return syncGet<ModelRes>(prefab, path) as ModelRes;
        }

        // ��ȡ cost ģ��
        public ModelRes getcostModel()
        {
            string prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_costModelAttrItem.m_prefabName;
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_costModelAttrItem.m_path;

            return syncGet<ModelRes>(prefab, path) as ModelRes;
        }

        // ��ȡ minion ģ��
        public ModelRes getMinionModel()
        {
            string prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_minionModelAttrItem.m_prefabName;
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_minionModelAttrItem.m_path;

            return syncGet<ModelRes>(prefab, path) as ModelRes;
        }

        // ��ȡ enemyCard ģ��
        public ModelRes getEnemyCardModel()
        {
            string prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_enemyCardModelAttrItem.m_prefabName;
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_enemyCardModelAttrItem.m_path;

            return syncGet<ModelRes>(prefab, path) as ModelRes;
        }

        // ��ȡ SceneCard ģ��
        public ModelRes getSceneCardModel(EnSceneCardType type)
        {
            string prefab = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[(int)type].m_prefabName;
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[(int)type].m_path;

            return syncGet<ModelRes>(prefab, path) as ModelRes;
        }

        public override void onLoaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            string path = res.GetPath();

            // ��ȡ��Դ��������
            (m_path2ResDic[path] as ModelRes).m_go = res.getObject(res.getPrefabName()) as GameObject;
            if (m_path2ListenItemDic[path].m_loaded != null)
            {
                m_path2ListenItemDic[path].m_loaded(m_path2ResDic[path]);
            }

            base.onLoaded(resEvt);
        }
    }
}