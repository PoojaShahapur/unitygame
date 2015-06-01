using Mono.Xml;
using SDK.Common;
using SDK.Lib;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace BehaviorLibrary
{
    /**
     * @brief 行为树管理器，存放所有的行为树
     */
    public class BehaviorTreeMgr : ResMgrBase
    {
        protected BTAttrSys m_btAttrSys;
        protected BTFactory m_BTFactory;

        public BehaviorTreeMgr()
        {
            m_btAttrSys = new BTAttrSys();
            m_BTFactory = new BTFactory();

            regAttrItem();
        }

        public void regAttrItem()
        {
            BTAttrItem item = null;
            item = new BTAttrItem();
            item.m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAIPath], "Test2Ai.xml");
            m_btAttrSys.m_id2ItemDic[BTID.e1000] = item;
        }

        public BehaviorTree getBT(BTID id)
        {
            if (m_path2ResDic.ContainsKey(m_btAttrSys.m_id2ItemDic[id].m_path))
            {
                return (m_path2ResDic[m_btAttrSys.m_id2ItemDic[id].m_path] as BehaviorTreeRes).behaviorTree;
            }

            return null;
        }

        // 同步加载
        public void syncLoadBT(BTID id)
        {
            syncLoad<BehaviorTreeRes>(m_btAttrSys.m_id2ItemDic[id].m_path);
        }

        // 异步加载
        public void loadBT(BTID id)
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(m_btAttrSys.m_id2ItemDic[id].m_path, param);
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            load<BehaviorTreeRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public BehaviorTreeRes getAndSyncLoadBT(BTID id)
        {
            return getAndSyncLoad<BehaviorTreeRes>(m_btAttrSys.m_id2ItemDic[id].m_path);
        }

        public BehaviorTreeRes getAndLoadBT(BTID id)
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            LocalFileSys.modifyLoadParam(m_btAttrSys.m_id2ItemDic[id].m_path, param);
            param.m_loadNeedCoroutine = false;
            BehaviorTreeRes ret = getAndLoad<BehaviorTreeRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            return ret;
        }

        public void parseXml(string xmlStr, BehaviorTree behaviorTree)
        {
            SecurityParser xmlDoc = new SecurityParser();
            xmlDoc.LoadXml(xmlStr);

            SecurityElement rootNode = xmlDoc.ToXml();
            ArrayList behaviorTemplateNode = rootNode.Children;
            ArrayList behaviorTreeXmlList = null;
            SecurityElement xmlElemTpl;
            SecurityElement xmlElemBT;

            foreach (SecurityElement node in behaviorTemplateNode)  // 树列表，包括树和其它信息
            {
                xmlElemTpl = node;
                behaviorTreeXmlList = xmlElemTpl.Children;
                foreach (SecurityElement nodetree in behaviorTreeXmlList)
                {
                    xmlElemBT = nodetree;
                    m_BTFactory.parseXml(behaviorTree, xmlElemBT);
                }
            }
        }
    }
}