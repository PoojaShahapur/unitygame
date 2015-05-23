using Mono.Xml;
using SDK.Common;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace SDK.Lib
{
    public class ModelMgr : ResMgrBase
    {
        // 记录蒙皮信息
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
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBeingPath] + "BonesList";
            param.m_loadEventHandle = onSkinLoadEventHandle;
            Ctx.m_instance.m_resLoadMgr.loadBundle(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public void onSkinLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            string text = res.getText("BoneList");
            SecurityParser xmlDoc = new SecurityParser();
            xmlDoc.LoadXml(text);

            SecurityElement rootNode = xmlDoc.ToXml();
            ArrayList itemMeshList = rootNode.Children;
            SecurityElement itemMesh;

            ArrayList itemSubMeshList;
            SecurityElement itemSubMesh;
            string meshName = "";
            string subMeshName = "";
            string bonesList = "";

            foreach (SecurityElement itemNode1f in itemMeshList)
            {
                itemMesh = itemNode1f;
                meshName = UtilApi.getXmlAttrStr(itemMesh, "name");
                m_skinDic[meshName] = new Dictionary<string, string[]>();

                itemSubMeshList = itemMesh.Children;
                foreach (SecurityElement itemNode2f in itemSubMeshList)
                {
                    itemSubMesh = itemNode2f;
                    subMeshName = UtilApi.getXmlAttrStr(itemSubMesh, "name");
                    bonesList = UtilApi.getXmlAttrStr(itemSubMesh, "bonelist");
                    m_skinDic[meshName][subMeshName] = bonesList.Split(',');
                }
            }
        }

        // 获取卡牌组的模型
        public ModelRes getCardGroupModel()
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_cardGroupModelAttrItem.m_path;

            return syncGet<ModelRes>(path) as ModelRes;
        }

        // 获取卡牌组中的卡牌的模型
        public ModelRes getGroupCardModel()
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_groupCardModelAttrItem.m_path;

            return syncGet<ModelRes>(path) as ModelRes;
        }

        // 获取 cost 模型
        public ModelRes getcostModel()
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_costModelAttrItem.m_path;

            return syncGet<ModelRes>(path) as ModelRes;
        }

        // 获取 minion 模型
        public ModelRes getMinionModel()
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_minionModelAttrItem.m_path;

            return syncGet<ModelRes>(path) as ModelRes;
        }

        // 获取 enemyCard 模型
        public ModelRes getEnemyCardModel()
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_enemyCardModelAttrItem.m_path;

            return syncGet<ModelRes>(path) as ModelRes;
        }

        // 获取 SceneCard 模型
        public ModelRes getSceneCardModel(CardType type)
        {
            if (type < CardType.eCARDTYPE_Total)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[(int)type] != null)
                {
                    string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_sceneCardModelAttrItemList[(int)type].m_path;

                    return syncGet<ModelRes>(path) as ModelRes;
                }
            }
            else
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem2, type.ToString());
            }

            return null;
        }

        public override void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            string path = res.GetPath();

            // 获取资源单独保存
            (m_path2ResDic[path] as ModelRes).m_go = res.getObject(res.getPrefabName()) as GameObject;
            m_path2ResDic[path].refCountResLoadResultNotify.loadEventDispatch.dispatchEvent(m_path2ResDic[path]);

            base.onLoadEventHandle(dispObj);
        }
    }
}