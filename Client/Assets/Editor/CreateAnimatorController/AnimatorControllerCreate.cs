using EditorTool;
using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
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

        public static AnimatorControllerCreateSys instance()
        {
            if (m_instance == null)
            {
                m_instance = new AnimatorControllerCreateSys();
            }
            return m_instance;
        }

        protected List<AnimatorControllerCreate> m_controllerList = new List<AnimatorControllerCreate>();

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
            AnimatorControllerCreate controller;

            foreach (XmlNode controllerNode in controllerNodeList)
            {
                controllerElem = (XmlElement)controllerNode;
                controller = new AnimatorControllerCreate();
                m_controllerList.Add(controller);
                controller.parseXml(controllerElem);
            }
        }

        // 打包 Controller
        public void exportControllerAsset()
        {
            foreach(var item in m_controllerList)
            {
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

    public class AnimatorControllerCreate
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
            m_params.parseXml(paramsNode as XmlElement, this);

            XmlNode layersNode = elem.SelectSingleNode("Layers");
            m_layers.parseXml(layersNode as XmlElement, this);
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

        public void parseXml(XmlElement elem, AnimatorControllerCreate controllerData)
        {
            XmlNodeList paramsNodeList = elem.ChildNodes;
            XmlElement paramElem = null;
            Param param;
            foreach (XmlNode paramNode in paramsNodeList)
            {
                paramElem = (XmlElement)paramNode;
                param = new Param();
                m_paramList.Add(param);
                param.parseXml(paramElem, controllerData);
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

        public void parseXml(XmlElement elem, AnimatorControllerCreate controllerData)
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

        public void parseXml(XmlElement elem, AnimatorControllerCreate controllerData)
        {
            XmlNodeList layersNodeList = elem.ChildNodes;
            XmlElement layerElem = null;
            Layer layer;
            foreach (XmlNode layerNode in layersNodeList)
            {
                layerElem = (XmlElement)layerNode;
                layer = new Layer();
                m_layerList.Add(layer);
                layer.parseXml(layerElem, controllerData);
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

        public void parseXml(XmlElement elem, AnimatorControllerCreate controllerData)
        {
            XmlNodeList stateMachineNodeList = elem.ChildNodes;
            XmlElement stateMachineElem = null;
            StateMachine stateMachine;
            foreach (XmlNode stateMachineNode in stateMachineNodeList)
            {
                stateMachineElem = (XmlElement)stateMachineNode;
                stateMachine = new StateMachine();
                m_stateMachineList.Add(stateMachine);
                stateMachine.parseXml(stateMachineElem, controllerData);
            }
        }
    }

    public class StateMachine
    {
        protected List<State> m_stateList = new List<State>();

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

        public void parseXml(XmlElement elem, AnimatorControllerCreate controllerData)
        {
            XmlNodeList stateNodeList = elem.ChildNodes;
            XmlElement stateElem = null;
            State state;
            foreach (XmlNode stateNode in stateNodeList)
            {
                stateElem = (XmlElement)stateNode;
                state = new State();
                m_stateList.Add(state);
                state.parseXml(stateElem, controllerData);
            }
        }
    }

    public class State
    {
        protected string m_motion;
        protected string m_fullMotion;

        protected List<Condition> m_condList = new List<Condition>();

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

        public void parseXml(XmlElement elem, AnimatorControllerCreate controllerData)
        {
            m_motion = ExportUtil.getXmlAttrStr(elem.Attributes["motion"]);
            m_fullMotion = string.Format("{0}/{1}", controllerData.inPath, m_motion);

            XmlNodeList condNodeList = elem.ChildNodes;
            XmlElement condElem = null;
            Condition cond;
            foreach (XmlNode condNode in condNodeList)
            {
                condElem = (XmlElement)condNode;
                cond = new Condition();
                m_condList.Add(cond);
                cond.parseXml(condElem, controllerData);
            }
        }
    }

    public class Condition
    {
        protected string m_name;
        protected string m_value;

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

        public void parseXml(XmlElement elem, AnimatorControllerCreate controllerData)
        {
            m_name = ExportUtil.getXmlAttrStr(elem.Attributes["name"]);
            m_value = ExportUtil.getXmlAttrStr(elem.Attributes["value"]);
        }
    }
}