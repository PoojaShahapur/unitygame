using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Decorators;
using SDK.Common;
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

        public BTFactory()
        {
            registerBuild();
        }

        protected void registerBuild()
        {
            // Composites 组件注册
            m_id2CreateDic["IndexSelector"] = createIndexSelector;
            m_id2BuildDic["IndexSelector"] = buildIndexSelector;

            m_id2CreateDic["PartialSelector"] = createPartialSelector;
            m_id2BuildDic["PartialSelector"] = buildPartialSelector;

            m_id2CreateDic["PartialSequence"] = createPartialSequence;
            m_id2BuildDic["PartialSequence"] = buildPartialSequence;

            m_id2CreateDic["RandomSelector"] = createRandomSelector;
            m_id2BuildDic["RandomSelector"] = buildRandomSelector;

            m_id2CreateDic["Selector"] = createSelector;
            m_id2BuildDic["Selector"] = buildSelector;

            m_id2CreateDic["Sequence"] = createSequence;
            m_id2BuildDic["Sequence"] = buildSequence;

            m_id2CreateDic["StatefulSelector"] = createStatefulSelector;
            m_id2BuildDic["StatefulSelector"] = buildStatefulSelector;

            m_id2CreateDic["StatefulSequence"] = createStatefulSequence;
            m_id2BuildDic["StatefulSequence"] = buildStatefulSequence;

            // 条件组件注册
            m_id2CreateDic["Condition"] = createCondition;
            m_id2BuildDic["Condition"] = buildCondition;

            m_id2CreateDic["ConditionIdle"] = createConditionIdle;
            m_id2BuildDic["ConditionIdle"] = buildConditionIdle;

            // Decorators 组件注册
            m_id2CreateDic["Counter"] = createCounter;
            m_id2BuildDic["Counter"] = buildCounter;

            m_id2CreateDic["Inverter"] = createInverter;
            m_id2BuildDic["Inverter"] = buildInverter;

            m_id2CreateDic["RandomDecorator"] = createRandomDecorator;
            m_id2BuildDic["RandomDecorator"] = buildRandomDecorator;

            m_id2CreateDic["Repeater"] = createRepeater;
            m_id2BuildDic["Repeater"] = buildRepeater;

            m_id2CreateDic["RepeatUntilFail"] = createRepeatUntilFail;
            m_id2BuildDic["RepeatUntilFail"] = buildRepeatUntilFail;

            m_id2CreateDic["Succeeder"] = createSucceeder;
            m_id2BuildDic["Succeeder"] = buildSucceeder;

            m_id2CreateDic["Timer"] = createTimer;
            m_id2BuildDic["Timer"] = buildTimer;

            // Actions 组件注册
            m_id2CreateDic["BehaviorActionWander"] = createBehaviorActionWander;
            m_id2BuildDic["BehaviorActionWander"] = buildBehaviorActionWander;
        }

        public void parseXml(BehaviorTree btree, XmlNode btNode)
        {
            buildBT(btree, btNode as XmlElement);
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

        protected void buildBT(BehaviorTree btree, XmlElement btNode)
        {
            btree.name = UtilApi.getXmlAttrStr(btNode.Attributes["name"]);
        }

        public BehaviorComponent createIndexSelector()
        {
            IndexSelector indexSelector = new IndexSelector();
            return indexSelector;
        }

        public void buildIndexSelector(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createPartialSelector()
        {
            PartialSelector partialSelector = new PartialSelector();
            return partialSelector;
        }

        public void buildPartialSelector(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createPartialSequence()
        {
            PartialSequence partialSequence = new PartialSequence();
            return partialSequence;
        }

        public void buildPartialSequence(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createRandomSelector()
        {
            RandomSelector randomSelector = new RandomSelector();
            return randomSelector;
        }

        public void buildRandomSelector(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createSelector()
        {
            Selector selector = new Selector();
            return selector;
        }

        public void buildSelector(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createSequence()
        {
            Sequence sequence = new Sequence();
            return sequence;
        }

        public void buildSequence(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createStatefulSelector()
        {
            StatefulSelector statefulSelector = new StatefulSelector();
            return statefulSelector;
        }

        public void buildStatefulSelector(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createStatefulSequence()
        {
            StatefulSequence statefulSequence = new StatefulSequence();
            return statefulSequence;
        }

        public void buildStatefulSequence(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createCondition()
        {
            Condition condition = new Condition(null);
            return condition;
        }

        public void buildCondition(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createConditionIdle()
        {
            ConditionIdle conditionIdle = new ConditionIdle();
            return conditionIdle;
        }

        public void buildConditionIdle(BehaviorComponent btCmt, XmlNode btNode)
        {

        }


        public BehaviorComponent createCounter()
        {
            Counter counter = new Counter(0, null);
            return counter;
        }

        public void buildCounter(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createInverter()
        {
            Inverter inverter = new Inverter();
            return inverter;
        }

        public void buildInverter(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createRandomDecorator()
        {
            RandomDecorator randomDecorator = new RandomDecorator(0, null, null);
            return randomDecorator;
        }

        public void buildRandomDecorator(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createRepeater()
        {
            Repeater repeater = new Repeater(null);
            return repeater;
        }

        public void buildRepeater(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createRepeatUntilFail()
        {
            RepeatUntilFail repeatUntilFail = new RepeatUntilFail(null);
            return repeatUntilFail;
        }

        public void buildRepeatUntilFail(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createSucceeder()
        {
            Succeeder selector = new Succeeder(null);
            return selector;
        }

        public void buildSucceeder(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createTimer()
        {
            Timer timer = new Timer(null, 0, null);
            return timer;
        }

        public void buildTimer(BehaviorComponent btCmt, XmlNode btNode)
        {

        }

        public BehaviorComponent createBehaviorActionWander()
        {
            BehaviorActionWander behaviorActionWander = new BehaviorActionWander();
            return behaviorActionWander;
        }

        public void buildBehaviorActionWander(BehaviorComponent btCmt, XmlNode btNode)
        {

        }
    }
}