using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Composites;
using System;
using System.Collections.Generic;
using System.Xml;

namespace BehaviorLibrary
{
    /**
     * @brief 工厂基类，主要进行读写，Component 生成
     */
    public class BTFactory
    {
        protected Dictionary<string, Func<BehaviorComponent>> m_id2CreateDic = new Dictionary<string, Func<BehaviorComponent>>();
        protected Dictionary<string, Action<BehaviorComponent, XmlNode>> m_id2BuildDic = new Dictionary<string, Action<BehaviorComponent, XmlNode>>();

        protected void registerBuild()
        {
            m_id2CreateDic["Selector"] = createSelector;
            m_id2BuildDic["Selector"] = buildSelector;
        }

        public void parseXml(BehaviorTree btree, XmlNode btNode)
        {
            Stack<BehaviorComponent> stack = new Stack<BehaviorComponent>();
            depthTraverse(stack, btree.root, btNode);
        }

        protected void depthTraverse(Stack<BehaviorComponent> stack, BehaviorComponent parentNode, XmlNode btNode)
        {
            XmlNodeList btCmtNodeList = btNode.ChildNodes;
            BehaviorComponent btCmt;

            stack.Push(parentNode);
            foreach (XmlNode node in btCmtNodeList)
            {
                if (m_id2CreateDic.ContainsKey(node.Name))
                {
                    btCmt = m_id2CreateDic[node.Name].Invoke();
                    parentNode.addChild(btCmt);
                    if (m_id2BuildDic.ContainsKey(node.Name))
                    {
                        m_id2BuildDic[node.Name].Invoke(btCmt, node);
                    }
                    depthTraverse(stack, btCmt, node);
                }
            }
            stack.Pop();
        }

        public void buildSelector(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createSelector()
        {
            Selector selector = new Selector();
            return selector;
        }
    }
}