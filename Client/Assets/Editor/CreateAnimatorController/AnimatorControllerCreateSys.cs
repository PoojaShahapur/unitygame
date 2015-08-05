using EditorTool;
using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EditorTool
{
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
}