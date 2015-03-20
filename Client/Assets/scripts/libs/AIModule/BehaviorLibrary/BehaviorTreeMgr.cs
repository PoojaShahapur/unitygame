using BehaviorLibrary.Components;
using SDK.Common;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace BehaviorLibrary
{
    /**
     * @brief 行为树管理器，存放所有的行为树
     */
    public class BehaviorTreeMgr : IBehaviorTreeMgr
    {
        protected BTFactory m_BTFactory = new BTFactory();
        protected Dictionary<string, BehaviorTree> m_id2BTDic = new Dictionary<string,BehaviorTree>();

        public IBehaviorTree getBTByID(string id)
        {
            if(m_id2BTDic.ContainsKey(id))
            {
                return m_id2BTDic[id];
            }

            return null;
        }

        public void loadBT()
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAIPath] + "AI";
            param.m_loaded = onloaded;
            Ctx.m_instance.m_resLoadMgr.loadBundle(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        public void onloaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;
            TextAsset text = res.getObject("TestAi") as TextAsset;
            parseXml(text.text);
            text = res.getObject("Test2Ai") as TextAsset;
            parseXml(text.text);
        }

        protected void parseXml(string xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            XmlNodeList behaviorTemplateNode = rootNode.ChildNodes;
            XmlNodeList behaviorTreeXmlList = null;
            XmlElement xmlElemTpl;
            XmlElement xmlElemBT;

            foreach (XmlNode node in behaviorTemplateNode)  // 树列表，包括树和其它信息
            {
                xmlElemTpl = (XmlElement)node;
                //xmlElemBT = xmlElemTpl.SelectSingleNode("BehaviorTree") as XmlElement;
                behaviorTreeXmlList = xmlElemTpl.ChildNodes;
                foreach (XmlNode nodetree in behaviorTreeXmlList)
                {
                    xmlElemBT = nodetree as XmlElement;
                    m_id2BTDic[xmlElemBT.GetAttribute("name")] = new BehaviorTree(new BTRoot());
                    m_BTFactory.parseXml(m_id2BTDic[xmlElemBT.GetAttribute("name")], xmlElemBT);
                }
            }
        }
    }
}