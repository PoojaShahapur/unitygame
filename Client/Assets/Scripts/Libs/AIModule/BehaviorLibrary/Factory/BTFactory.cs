using SDK.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 工厂基类，主要进行读写，Component 生成
     */
    public class BTFactory
    {
        protected Dictionary<string, ComponentCreate> m_id2CreateDic = new Dictionary<string, ComponentCreate>();

        public BTFactory()
        {
            registerBuild();
        }

        protected void registerBuild()
        {
            // Composites 组件注册
            m_id2CreateDic[BTKey.Acrion] = new ActionCreate();
            m_id2CreateDic[BTKey.Inverter] = new InverterCreate();
            
            m_id2CreateDic[BTKey.IndexSelector] = new IndexSelectorCreate();
            m_id2CreateDic[BTKey.PartialSelector] = new PartialSelectorCreate();
            m_id2CreateDic[BTKey.PartialSequence] = new PartialSequenceCreate();
            m_id2CreateDic[BTKey.RandomSelector] = new RandomSelectorCreate();

            m_id2CreateDic[BTKey.Selector] = new SelectorCreate();
            m_id2CreateDic[BTKey.Sequence] = new SequenceCreate();
            m_id2CreateDic[BTKey.StatefulSelector] = new StatefulSelectorCreate();
            m_id2CreateDic[BTKey.StatefulSequence] = new StatefulSequenceCreate();

            m_id2CreateDic[BTKey.Condition] = new ConditionCreate();
            m_id2CreateDic[BTKey.Counter] = new CounterCreate();
            m_id2CreateDic[BTKey.RandomDecorator] = new RandomDecoratorCreate();
            m_id2CreateDic[BTKey.Repeater] = new RepeaterCreate();

            m_id2CreateDic[BTKey.RepeatUntilFail] = new RepeatUntilFailCreate();
            m_id2CreateDic[BTKey.Succeeder] = new SucceederCreate();
            m_id2CreateDic[BTKey.Timer] = new TimerCreate();
        }

        public void parseXml(BehaviorTree btree, SecurityElement btNode)
        {
            buildBT(btree, btNode);
            Stack<BehaviorComponent> stack = new Stack<BehaviorComponent>();
            depthTraverse(stack, btree.root, btNode);
        }

        protected void depthTraverse(Stack<BehaviorComponent> stack, BehaviorComponent parentNode, SecurityElement btNode)
        {
            ArrayList btCmtNodeList = btNode.Children;
            BehaviorComponent btCmt;
            string type;

            if (btCmtNodeList != null)
            {
                stack.Push(parentNode);
                foreach (SecurityElement node in btCmtNodeList)
                {
                    if (m_id2CreateDic.ContainsKey(node.Tag))
                    {
                        type = UtilApi.getXmlAttrStr(node, "type");
                        btCmt = m_id2CreateDic[node.Tag].createComp(type);
                        btCmt.behaviorTree = parentNode.behaviorTree;
                        parentNode.addChild(btCmt);
                        m_id2CreateDic[node.Tag].buildComp(type, btCmt, node);
                        depthTraverse(stack, btCmt, node);
                    }
                }
                stack.Pop();
            }
        }

        protected void buildBT(BehaviorTree btree, SecurityElement btNode)
        {
            btree.name = UtilApi.getXmlAttrStr(btNode, "name");
        }
    }
}