using BehaviorLibrary.Components;
using System.Collections.Generic;
using System.Xml;

namespace BehaviorLibrary
{
    /**
     * @brief 行为树管理器，存放所有的行为树
     */
    public class BehaviorTreeMgr
    {
        protected BTFactory m_BTFactory = new BTFactory();
        protected Dictionary<string, BehaviorTree> m_id2BTDic = new Dictionary<string,BehaviorTree>();

        public void loadBT()
        {
            string path = "test.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");
            XmlNodeList behaviorTemplateNode = rootNode.ChildNodes;
            XmlElement xmlElemTpl;
            XmlElement xmlElemBT;

            foreach (XmlNode node in behaviorTemplateNode)  // 树列表，包括树和其它信息
            {
                xmlElemTpl = (XmlElement)node;
                xmlElemBT = xmlElemTpl.SelectSingleNode("BehaviorTree") as XmlElement;

                m_id2BTDic[xmlElemBT.GetAttribute("name")] = new BehaviorTree(new BTRoot());
                m_BTFactory.parseXml(m_id2BTDic[xmlElemBT.GetAttribute("name")], xmlElemBT);
            }
        }
    }
}