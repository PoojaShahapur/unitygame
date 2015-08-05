using EditorTool;
using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace CreateAnimatorController
{
    public enum EParaMType
    {
        eInt,       // 整型
    }

    public class AnimatorControllerCreateSys
    {
        static public AnimatorControllerCreateSys m_instance;
        protected XmlAnimatorController m_curXmlAnimatorController;         // 当前处理的动画控制器

        public static AnimatorControllerCreateSys instance()
        {
            if (m_instance == null)
            {
                m_instance = new AnimatorControllerCreateSys();
            }
            return m_instance;
        }

        protected List<XmlAnimatorController> m_controllerList = new List<XmlAnimatorController>();

        public XmlAnimatorController curXmlAnimatorController
        {
            get
            {
                return m_curXmlAnimatorController;
            }
            set
            {
                m_curXmlAnimatorController = value;
            }
        }

        public void clear()
        {
            m_controllerList.Clear();
        }

        // 解析 Xml
        public void parseXml()
        {
            string path = ExportUtil.getDataPath("Res/Config/Tool/CreateAnimatorController.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");

            XmlNodeList controllerNodeList = rootNode.ChildNodes;
            XmlElement controllerElem;
            XmlAnimatorController controller;

            foreach (XmlNode controllerNode in controllerNodeList)
            {
                controllerElem = (XmlElement)controllerNode;
                controller = new XmlAnimatorController();
                m_curXmlAnimatorController = controller;
                m_controllerList.Add(controller);
                controller.parseXml(controllerElem);
            }
        }

        // 打包 Controller
        public void exportControllerAsset()
        {
            foreach(var item in m_controllerList)
            {
                m_curXmlAnimatorController = item;
                RuntimeAnimatorController runtimeAsset = AnimatorControllerCreateUtil.BuildAnimationController(item);
                SOAnimatorController soAnimator = ScriptableObject.CreateInstance<SOAnimatorController>();
                soAnimator.addAnimator(item.controllerFullPath, runtimeAsset);

                // 创建预制，并且添加到编辑器中，以便进行检查
                AssetDatabase.CreateAsset(soAnimator, item.assetFullPath);
                //刷新编辑器
                AssetDatabase.Refresh();
            }
        }
    }

    public class XmlAnimatorController
    {
        protected string m_inPath;
        protected string m_outPath;
        protected string m_outName;
        protected string m_outExtName;

        protected Params m_params = new Params();
        protected Layers m_layers = new Layers();

        protected string m_controllerFullPath;      // 中间生成的控制器的目录
        protected string m_assetFullPath;

        public string inPath
        {
            get
            {
                return m_inPath;
            }
            set
            {
                m_inPath = value;
            }
        }

        public string outPath
        {
            get
            {
                return m_outPath;
            }
            set
            {
                m_outPath = value;
            }
        }

        public string outName
        {
            get
            {
                return m_outName;
            }
            set
            {
                m_outName = value;
            }
        }

        public string outExtName
        {
            get
            {
                return m_outExtName;
            }
            set
            {
                m_outExtName = value;
            }
        }

        public Params getParams
        {
            get
            {
                return m_params;
            }
            set
            {
                m_params  = value;
            }
        }

        public Layers layers
        {
            get
            {
                return m_layers;
            }
            set
            {
                m_layers = value;
            }
        }

        public string controllerFullPath
        {
            get
            {
                return m_controllerFullPath;
            }
            set
            {
                m_controllerFullPath = value;
            }
        }

        public string assetFullPath
        {
            get
            {
                return m_assetFullPath;
            }
            set
            {
                m_assetFullPath = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_inPath = ExportUtil.getXmlAttrStr(elem.Attributes["inpath"]);
            m_outPath = ExportUtil.getXmlAttrStr(elem.Attributes["outpath"]);
            m_outName = ExportUtil.getXmlAttrStr(elem.Attributes["outname"]);
            m_outExtName = ExportUtil.getXmlAttrStr(elem.Attributes["outextname"]);

            m_controllerFullPath = string.Format("{0}/{1}.controller", m_inPath, m_outName);
            m_assetFullPath = string.Format("{0}/{1}.{2}", m_outPath, m_outName, m_outExtName);

            XmlNode paramsNode = elem.SelectSingleNode("Params");
            m_params.parseXml(paramsNode as XmlElement);

            XmlNode layersNode = elem.SelectSingleNode("Layers");
            m_layers.parseXml(layersNode as XmlElement);
        }
    }

    public class Params
    {
        protected List<Param> m_paramList = new List<Param>();

        public List<Param> paramList
        {
            get
            {
                return m_paramList;
            }
            set
            {
                m_paramList = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            XmlNodeList paramsNodeList = elem.ChildNodes;
            XmlElement paramElem = null;
            Param param;
            foreach (XmlNode paramNode in paramsNodeList)
            {
                paramElem = (XmlElement)paramNode;
                param = new Param();
                m_paramList.Add(param);
                param.parseXml(paramElem);
            }
        }
    }

    public class Param
    {
        protected string m_name;
        protected string m_type;

        public string name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public string type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_type = ExportUtil.getXmlAttrStr(elem.Attributes["type"]);
        }
    }

    public class Layers
    {
        protected List<Layer> m_layerList = new List<Layer>();

        public List<Layer> layerList
        {
            get
            {
                return m_layerList;
            }
            set
            {
                m_layerList = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            XmlNodeList layersNodeList = elem.SelectNodes("Layer");
            XmlElement layerElem = null;
            Layer layer;
            foreach (XmlNode layerNode in layersNodeList)
            {
                layerElem = (XmlElement)layerNode;
                layer = new Layer();
                m_layerList.Add(layer);
                layer.parseXml(layerElem);
            }
        }
    }

    public class Layer
    {
        protected List<StateMachine> m_stateMachineList = new List<StateMachine>();

        public List<StateMachine> stateMachineList
        {
            get
            {
                return m_stateMachineList;
            }
            set
            {
                m_stateMachineList = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            XmlNodeList stateMachineNodeList = elem.SelectNodes("Statemachine");
            XmlElement stateMachineElem = null;
            StateMachine stateMachine;
            foreach (XmlNode stateMachineNode in stateMachineNodeList)
            {
                stateMachineElem = (XmlElement)stateMachineNode;
                stateMachine = new StateMachine();
                stateMachine.layer = this;
                m_stateMachineList.Add(stateMachine);
                stateMachine.parseXml(stateMachineElem);
            }
        }
    }

    public class StateMachine
    {
        protected List<State> m_stateList = new List<State>();
        protected List<Clip> m_clipList = new List<Clip>();
        protected List<XmlTransition> m_tranList = new List<XmlTransition>();

        protected Layer m_layer;                // 当前状态机所在的 Layer
        protected AnimatorStateMachine m_animatorStateMachine;      // 记录当前状态机

        public List<State> stateList
        {
            get
            {
                return m_stateList;
            }
            set
            {
                m_stateList = value;
            }
        }

        public AnimatorStateMachine animatorStateMachine
        {
            get
            {
                return m_animatorStateMachine;
            }
            set
            {
                m_animatorStateMachine = value;
            }
        }

        public Layer layer
        {
            get
            {
                return m_layer;
            }
            set
            {
                m_layer = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            XmlNodeList stateNodeList = elem.SelectNodes("Clip");
            XmlElement stateElem = null;
            Clip _clip;
            foreach (XmlNode stateNode in stateNodeList)
            {
                stateElem = (XmlElement)stateNode;
                _clip = new Clip();
                _clip.stateMachine = this;
                m_clipList.Add(_clip);
                _clip.parseXml(stateElem);
            }

            XmlNodeList tranNodeList = elem.SelectNodes("Transition");
            XmlElement tranElem = null;
            XmlTransition _tran;
            foreach (XmlNode stateNode in stateNodeList)
            {
                tranElem = (XmlElement)stateNode;
                _tran = new XmlTransition();
                _tran.stateMachine = this;
                m_tranList.Add(_tran);
                _tran.parseXml(stateElem);
            }
        }
    }

    public class Clip
    {
        protected string m_name;
        protected StateMachine m_stateMachine;  // 对应的状态机
        protected List<State> m_stateList = new List<State>();

        public StateMachine stateMachine
        {
            get
            {
                return m_stateMachine;
            }
            set
            {
                m_stateMachine = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);

            XmlNodeList stateNodeList = elem.SelectNodes("State");
            XmlElement stateElem = null;
            State state;
            foreach (XmlNode stateNode in stateNodeList)
            {
                stateElem = (XmlElement)stateNode;
                state = new State();
                state.stateMachine = m_stateMachine;
                m_stateList.Add(state);
                m_stateMachine.stateList.Add(state);
                state.parseXml(stateElem);
            }
        }
    }

    public class State
    {
        protected string m_motion;
        protected string m_fullMotion;

        protected List<Condition> m_condList = new List<Condition>();
        protected StateMachine m_stateMachine;  // 对应的状态机

        public List<Condition> condList
        {
            get
            {
                return m_condList;
            }
            set
            {
                m_condList = value;
            }
        }

        public string fullMotion
        {
            get
            {
                return m_fullMotion;
            }
            set
            {
                m_fullMotion = value;
            }
        }

        public StateMachine stateMachine
        {
            get
            {
                return m_stateMachine;
            }
            set
            {
                m_stateMachine = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_motion = ExportUtil.getXmlAttrStr(elem.Attributes["motion"]);
            m_fullMotion = string.Format("{0}/{1}", AnimatorControllerCreateSys.m_instance.curXmlAnimatorController.inPath, m_motion);

            XmlNodeList condNodeList = elem.SelectNodes("AnyCondition");
            XmlElement condElem = null;
            Condition cond;
            foreach (XmlNode condNode in condNodeList)
            {
                condElem = (XmlElement)condNode;
                cond = new Condition();
                m_condList.Add(cond);
                cond.parseXml(condElem);
            }
        }
    }

    public class Condition
    {
        public const string GREATER = "Greater";            // 大于
        public const string LESS = "Less";                  // 小于
        public const string EQUALS = "Equals";              // 等于
        public const string NOTEQUAL = "NotEqual";          // 不等于

        protected string m_name;
        protected string m_value;
        protected AnimatorConditionMode m_opMode;           // 操作模式

        public string name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public float getFloatValue()
        {
            //float ret = float.Parse(m_value);
            float ret = Convert.ToSingle(m_value);
            return ret;
        }

        public AnimatorConditionMode opMode
        {
            get
            {
                return m_opMode;
            }
            set
            {
                m_opMode = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_value = ExportUtil.getXmlAttrStr(elem.Attributes["value"]);
            string opMode = ExportUtil.getXmlAttrStr(elem.Attributes["OpMode"]);
            if(Condition.GREATER == opMode)
            {
                m_opMode = AnimatorConditionMode.Greater;
            }
            else if (Condition.LESS == opMode)
            {
                m_opMode = AnimatorConditionMode.Less;
            }
            else if (Condition.EQUALS == opMode)
            {
                m_opMode = AnimatorConditionMode.Equals;
            }
            else if (Condition.NOTEQUAL == opMode)
            {
                m_opMode = AnimatorConditionMode.NotEqual;
            }
        }
    }

    public class XmlTransition
    {
        protected string m_srcState;
        protected string m_destState;
        protected List<Condition> m_condList;

        protected StateMachine m_stateMachine;

        public StateMachine stateMachine
        {
            get
            {
                return m_stateMachine;
            }
            set
            {
                m_stateMachine = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            XmlNodeList condNodeList = elem.SelectNodes("Condition");
            XmlElement condElem = null;
            Condition cond;
            foreach (XmlNode condNode in condNodeList)
            {
                condElem = (XmlElement)condNode;
                cond = new Condition();
                m_condList.Add(cond);
                cond.parseXml(condElem);
            }
        }
    }
}