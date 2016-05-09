using Mono.Xml;
using SDK.Lib;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 行为树管理器，存放所有的行为树
     */
    public class BehaviorTreeMgr : InsResMgrBase
    {
        protected BTAttrSys m_btAttrSys;
        protected BTFactory m_BTFactory;
        protected Dictionary<BTID, BehaviorTree> m_id2BTDic;

        public BehaviorTreeMgr()
        {
            m_btAttrSys = new BTAttrSys();
            m_BTFactory = new BTFactory();
            m_id2BTDic = new Dictionary<BTID, BehaviorTree>();

            regAttrItem();
        }

        public void regAttrItem()
        {
            BTAttrItem item = null;
            item = new BTAttrItem();
            item.m_id = BTID.e1000;
            item.m_name = "1000";
            item.m_path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAIPath], "1000.xml");
            m_btAttrSys.m_id2ItemDic[BTID.e1000] = item;
        }

        public BehaviorTree getBT(BTID id)
        {
            if (!m_id2BTDic.ContainsKey(id))
            {
                BehaviorTreeRes res = getAndSyncLoadBT(id);
                this.unload(res.getResUniqueId(), null);
            }

            return m_id2BTDic[id];
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
            param.setPath(m_btAttrSys.m_id2ItemDic[id].m_path);
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
            param.setPath(m_btAttrSys.m_id2ItemDic[id].m_path);
            param.m_loadNeedCoroutine = false;
            BehaviorTreeRes ret = getAndLoad<BehaviorTreeRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);

            return ret;
        }

        public void parseXml(string xmlStr)
        {
            SecurityParser xmlDoc = new SecurityParser();
            xmlDoc.LoadXml(xmlStr);

            SecurityElement rootNode = xmlDoc.ToXml();
            ArrayList behaviorTemplateNode = rootNode.Children;
            ArrayList behaviorTreeXmlList = null;
            SecurityElement xmlElemTpl;
            SecurityElement xmlElemBT;
            BehaviorTree behaviorTree = null;
            string strId = "";
            BTID id = BTID.eNone;

            foreach (SecurityElement node in behaviorTemplateNode)  // 树列表，包括树和其它信息
            {
                xmlElemTpl = node;
                behaviorTreeXmlList = xmlElemTpl.Children;
                behaviorTree = new BehaviorTree(new BTRoot());

                UtilXml.getXmlAttrStr(node, "name", ref strId);
                id = m_btAttrSys.getBTIDByName(strId);
                m_id2BTDic[id] = behaviorTree;

                foreach (SecurityElement nodetree in behaviorTreeXmlList)
                {
                    xmlElemBT = nodetree;
                    m_BTFactory.parseXml(behaviorTree, xmlElemBT);
                }
            }
        }
    }
}