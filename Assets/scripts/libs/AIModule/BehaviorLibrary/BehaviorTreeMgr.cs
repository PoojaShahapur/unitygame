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

        public void loadBT()
        {
            LoadParam param = Ctx.m_instance.m_resMgr.getLoadParam();
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAIPath] + "AI";
            param.m_loadedcb = onloaded;
            Ctx.m_instance.m_resMgr.loadBundle(param);
        }

        public void onloaded(EventDisp resEvt)
        {
            IRes res = resEvt.m_param as IRes;
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